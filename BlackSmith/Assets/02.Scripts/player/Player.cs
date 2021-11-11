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
    public bool motion; //인벤토리 모션제약
    public bool smotion; // 나머지모션 제약
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
        if (GameManager.instance.isBagOn == false && motion == true) //가방끌때 바라보는 모션을 끈다
        {
            MotionOFF();
        }
        if (motion || smotion || !GameManager.instance.startGame || GameManager.instance.nextDate)//모션중일때 이동등 다른 동작을 멈춘다
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

        //이동
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        lookDirection = v * Vector3.back + h * Vector3.left;
        if ((Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0))
        {
            if (sound)
            {
                switch (hitSound.collider.name)//발판에따라 다른발소리
                {
                    case "Terrain":
                        audioSource.clip = audioClips[0];
                        break;
                    case "BSFloor":
                        audioSource.clip = audioClips[1];
                        break;
                }
            }
            if (!equiping)//손에 아무것도 없을때
                anim.SetBool("isWalk", true);
            else if (equiping)//장비 착용중일때
            {
                anim.SetBool("isWalk", true);
                anim.SetBool("Equiping", true);
            }
            if (sound && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);//회전
            if (!stop)
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("Equiping", false);
            audioSource.Stop();
        }

        //달리기
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
        if (Input.GetKeyDown(KeyCode.Space) && !equiping)//박수치는 모션
        {
            anim.SetBool("ExitMotion", false);
            anim.SetTrigger("doHandFree");
            anim.SetFloat("Blend", 1);
            smotion = true;
            Invoke("MotionOFF", 2);
        }
        if (GameManager.instance.isBagOn == true)//가방을 킬때 바라보는 모션
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

    void ChackItem()//아이템줍기
    {

        RaycastHit hitItem;//레이로 아이템검사
        bool item = Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.forward, out hitItem, 0.5f, LayerMask.GetMask("Item"));

        if (item)//중복인아이템 찾아서 추가한다
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                audioSource.Stop();//오디오정지
                audioSource.clip = audioClips[2];
                if (!audioSource.isPlaying)
                    audioSource.Play();
                int bagSoltNum = 0;
                ItemType itemType = hitItem.collider.GetComponent<ItemPickUp>().item;

                //장비중복검색을 하고 참거짓을 보내준다
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
                    if (inventory.transform.GetChild(i).GetComponent<Bag>().item != null && itemType.itemName == inventory.transform.GetChild(i).GetComponent<Bag>().item.itemName && !hitSameBag)//같은 아이템 있는지 검사
                    {                                                                                                                                                                             //1번칸에 아이템갯수가 5개면 다음 칸으로 옮긴다 중복되는 장비가 있으면 다음칸으로 옮긴다
                        inventory.transform.GetChild(i).GetComponent<Bag>().count += itemType.num;//장비를 중복해서 인벤토리에 못넣는다
                        bagSoltNum = i;
                        break;
                    }
                    else if (inventory.transform.GetChild(i).GetComponent<Bag>().item == null)//없으면 가장낮은 널에 집어넣는다
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
            item.SetActive(false);//손에들고있는거 전체 제거
        }
        switch (toolName)//이름이 같은거 찾아서 착용
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
    public void ThrowItem()//아이템버리기
    {
        if (equiping && toolInfo[toolInfoNum].GetComponent<ItemPickUp>().item.itemName == inventory.transform.GetChild(clickItemNum).GetComponent<Bag>().item.name)//장착중인템이면 못버리게한다     
            equipEqulThrowItem = true;           //장착거랑 같은지 비교하는문 참거짓으로 구분     
        else
            equipEqulThrowItem = false;

        RaycastHit hitItem;
        bool item = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward, out hitItem, 1, ~(LayerMask.GetMask("Player") | LayerMask.GetMask("Defult")));//판매대도 포함해서 버릴수있게한다

        if (item || inventory.transform.GetChild(clickItemNum).GetComponent<Bag>().item == null || equipEqulThrowItem)//아이템이 앞에있고 가방이 안비어있고 장착중인 아이템이면
            return;
        else if (toolName == inventory.transform.GetChild(clickItemNum).GetComponent<Bag>().item.name)
        {
            inventory.transform.GetChild(clickItemNum).GetComponent<Bag>().count--;//1개씩줄이고
            GameObject spwanItem = inventory.transform.GetChild(clickItemNum).GetComponent<Bag>().item.prefab;//프리펩을 게임오브젝트로 넣어서
            spwanItem.transform.position = transform.GetChild(0).transform.position;//플레이어앞에 생성
            Instantiate(spwanItem);
            spwanItem = null;//값을지운다
            clickItemNum = inventory.transform.childCount + 1; //인벤토리보다 1큰값으로 건들지 존재하지않은 칸을 누른듯 만든다
            toolName = null;
            equipEqulThrowItem = false;
        }


    }
    void MineAndCutTree()//채광 채집모션
    {
        RaycastHit hitItem;
        bool item = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward, out hitItem, 1, LayerMask.GetMask("Mine") | LayerMask.GetMask("Cut"));

        if (Input.GetKeyDown(KeyCode.Z) && equiping && (Mathf.Abs(h) == 0 || Mathf.Abs(v) == 0) && item)//장착중이고 움직임이 없고 해당레이를가진아이템이 있을경우
        {
            switch (hitItem.collider.gameObject.layer)
            {
                case 8://채광 나무밑둥
                    anim.SetTrigger("doMine");
                    smotion = true;
                    Invoke("MotionOFF", 1);
                    break;
                case 9://나무 자르기                    
                    anim.SetTrigger("doSmash");
                    smotion = true;
                    Invoke("MotionOFF", 1);
                    break;
            }
            toolInfo[toolInfoNum].GetComponent<BoxCollider>().enabled = true;
            //파티클도 보이게 변경
        }

    }


}
