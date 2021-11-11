using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;

    [Header("Move------------------")]
    public float rotationSpeed = 100;
    float h;
    float v;
    float speed = 2;
    Vector3 lookDirection;

    [Header("Motion------------------")]
    public bool motion; //�κ��丮 �������
    public bool smotion; // ��������� ����
    public GameObject[] toolInfo;
    int toolInfoNum;
    public GameObject toolHand;

    [Header("HandTool------------------")]
    bool isHand;
    public string toolName;
    public bool equiping;


    [Header("Bag---------------------")]
    public GameObject inventory;
    public int clickItemNum;

    AudioSource audioSource;
    [Header("Sound---------------------")]
    [SerializeField] AudioClip[] audioClips;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        toolInfo = new GameObject[toolHand.transform.childCount];

    }
    void Start()
    {
        for (int i = 0; i < toolHand.transform.childCount; i++)
        {
            toolInfo[i] = toolHand.transform.GetChild(i).gameObject;
        }
    }

    void Update()
    {
        if (GameManager.instance.isBagOn == false && motion == true) //������� �ٶ󺸴� ����� ����
        {
            MotionOFF();
        }
        if (motion || smotion || !GameManager.instance.startGame || GameManager.instance.nextDate)//������϶� �̵��� �ٸ� ������ �����
        {
            return;
        }
        HandFreeMotion();
        Move();
        ChackItem();
        MineAndCutTree();
    }

    void Move()
    {
        RaycastHit hit;
        bool stop = Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), transform.forward, out hit, 0.3f, ~LayerMask.GetMask("Default"));
        RaycastHit hitSound;
        bool sound = Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), transform.up * -1, out hitSound, 0.5f, LayerMask.GetMask("Ground"));

        //�̵�
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        lookDirection = v * Vector3.back + h * Vector3.left;
        if ((Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0))
        {
            if (sound)
            {
                switch (hitSound.collider.name)//���ǿ����� �ٸ��߼Ҹ�
                {
                    case "Terrain":
                        audioSource.clip = audioClips[0];
                        break;
                    case "BSFloor":
                        audioSource.clip = audioClips[1];
                        break;
                }
            }
            if (!equiping)//�տ� �ƹ��͵� ������
                anim.SetBool("isWalk", true);
            else if (equiping)//��� �������϶�
            {
                anim.SetBool("isWalk", true);
                anim.SetBool("Equiping", true);
            }
            if (sound && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);//ȸ��
            if (!stop)
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("Equiping", false);
            audioSource.Stop();
        }

        //�޸���
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 4;
            anim.SetBool("isRun", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 2;
            anim.SetBool("isRun", false);
        }
    }
    #region HandFreeMotion
    void HandFreeMotion()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !equiping)//�ڼ�ġ�� ���
        {
            anim.SetBool("ExitMotion", false);
            anim.SetTrigger("doHandFree");
            anim.SetFloat("Blend", 1);
            smotion = true;
            Invoke("MotionOFF", 2);
        }
        if (GameManager.instance.isBagOn == true)//������ ų�� �ٶ󺸴� ���
        {
            anim.SetTrigger("doHandFree");
            anim.SetBool("ExitMotion", false);
            anim.SetFloat("Blend", 0);
            motion = true;
        }

    }
    void MotionOFF()
    {
        smotion = false;
        motion = false;
        anim.SetBool("ExitMotion", true);
    }
    #endregion

    bool hitSameBag = false;

    void ChackItem()//�������ݱ�
    {

        RaycastHit hitItem;//���̷� �����۰˻�
        bool item = Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.forward, out hitItem, 0.5f, LayerMask.GetMask("Item"));

        if (item)//�ߺ��ξ����� ã�Ƽ� �߰��Ѵ�
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                audioSource.Stop();//���������
                audioSource.clip = audioClips[2];
                if (!audioSource.isPlaying)
                    audioSource.Play();
                int bagSoltNum = 0;
                ItemType itemType = hitItem.collider.GetComponent<ItemPickUp>().item;

                //����ߺ��˻��� �ϰ� �������� �����ش�
                for (int i = 0; i < inventory.transform.childCount; i++)
                {
                    switch (itemType.type)
                    {
                        case ItemType.Type.equip:
                            if (inventory.transform.GetChild(i).GetComponent<Bag>().item == itemType)
                            {
                                hitSameBag = true;
                            }
                            break;
                        case ItemType.Type.used:
                            break;
                        case ItemType.Type.etc:
                            break;
                    }
                }

                for (int i = 0; i < inventory.transform.childCount; i++)
                {
                    if (inventory.transform.GetChild(i).GetComponent<Bag>().item != null && itemType.itemName == inventory.transform.GetChild(i).GetComponent<Bag>().item.itemName && !hitSameBag)//���� ������ �ִ��� �˻�
                    {                                                                                                                                                                             //1��ĭ�� �����۰����� 5���� ���� ĭ���� �ű�� �ߺ��Ǵ� ��� ������ ����ĭ���� �ű��
                        inventory.transform.GetChild(i).GetComponent<Bag>().count += itemType.num;//��� �ߺ��ؼ� �κ��丮�� ���ִ´�
                        bagSoltNum = i;
                        break;
                    }
                    else if (inventory.transform.GetChild(i).GetComponent<Bag>().item == null)//������ ���峷�� �ο� ����ִ´�
                    {
                        inventory.transform.GetChild(i).GetComponent<Bag>().item = itemType;
                        inventory.transform.GetChild(i).GetComponent<Bag>().count += itemType.num;
                        break;
                    }
                }

                Destroy(hitItem.collider.gameObject);
                hitSameBag = false;

            }
        }
    }

    public void EquipON()
    {
        foreach (var item in toolInfo)
        {
            item.SetActive(false);//�տ�����ִ°� ��ü ����
        }
        switch (toolName)//�̸��� ������ ã�Ƽ� ����
        {
            case "Axe":
                toolInfo[0].SetActive(true);
                toolInfoNum = 0;
                equiping = true;
                break;
            case "Pickax":
                toolInfo[1].SetActive(true);
                toolInfoNum = 1;
                equiping = true;
                break;
            case "Hammer":
                toolInfo[2].SetActive(true);
                toolInfoNum = 2;
                equiping = true;
                break;
            case "Shovel":
                break;
            case "Sword":
                break;
            case null:
                foreach (var item in toolInfo)
                {
                    item.SetActive(false);
                    equiping = false;
                    toolInfoNum = toolInfo.Length + 1;
                }
                break;
        }
        toolName = null;
    }
    bool equipEqulThrowItem = false;
    public void ThrowItem()//�����۹�����
    {
        if (equiping && toolInfo[toolInfoNum].GetComponent<ItemPickUp>().item.itemName == inventory.transform.GetChild(clickItemNum).GetComponent<Bag>().item.name)//�����������̸� ���������Ѵ�     
            equipEqulThrowItem = true;           //�����Ŷ� ������ ���ϴ¹� ���������� ����     
        else
            equipEqulThrowItem = false;

        RaycastHit hitItem;
        bool item = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward, out hitItem, 1, ~(LayerMask.GetMask("Player") | LayerMask.GetMask("Defult")));//�ǸŴ뵵 �����ؼ� �������ְ��Ѵ�

        if (item || inventory.transform.GetChild(clickItemNum).GetComponent<Bag>().item == null || equipEqulThrowItem)//�������� �տ��ְ� ������ �Ⱥ���ְ� �������� �������̸�
            return;
        else if (toolName == inventory.transform.GetChild(clickItemNum).GetComponent<Bag>().item.name)
        {
            inventory.transform.GetChild(clickItemNum).GetComponent<Bag>().count--;//1�������̰�
            GameObject spwanItem = inventory.transform.GetChild(clickItemNum).GetComponent<Bag>().item.prefab;//�������� ���ӿ�����Ʈ�� �־
            spwanItem.transform.position = transform.GetChild(0).transform.position;//�÷��̾�տ� ����
            Instantiate(spwanItem);
            spwanItem = null;//���������
            clickItemNum = inventory.transform.childCount + 1; //�κ��丮���� 1ū������ �ǵ��� ������������ ĭ�� ������ �����
            toolName = null;
            equipEqulThrowItem = false;
        }


    }
    void MineAndCutTree()//ä�� ä�����
    {
        RaycastHit hitItem;
        bool item = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward, out hitItem, 1, LayerMask.GetMask("Mine") | LayerMask.GetMask("Cut"));

        if (Input.GetKeyDown(KeyCode.Z) && equiping && (Mathf.Abs(h) == 0 || Mathf.Abs(v) == 0) && item)//�������̰� �������� ���� �ش緹�̸������������� �������
        {
            switch (hitItem.collider.gameObject.layer)
            {
                case 8://ä�� �����ص�
                    anim.SetTrigger("doMine");
                    smotion = true;
                    Invoke("MotionOFF", 1);
                    break;
                case 9://���� �ڸ���                    
                    anim.SetTrigger("doSmash");
                    smotion = true;
                    Invoke("MotionOFF", 1);
                    break;
            }
            toolInfo[toolInfoNum].GetComponent<BoxCollider>().enabled = true;
            //��ƼŬ�� ���̰� ����
        }

    }


}
