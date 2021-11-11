using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine_Cut_Object : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;//������������
    [SerializeField] Transform spawnPoint;//�����ճ����� ����
    [SerializeField] GameObject lowLevel;//��ȯ�� ���� ������Ʈ
    [SerializeField] string tagName; // �±׸� ��Ʈ������ ����
    [SerializeField] string objectSys; //������Ʈ���
    int dirNum;// �������
    Vector3 prefabVec; // ������Ƣ����� ����
    int hp; // ������Ʈ ü������ Ƚ������
    int spawnNum;
    Animator anim;

    public float shakeAmount;//������
    float shakeTime = 0.3f;//���½ð�
    Vector3 initialPostion;//��鸮�� ��ġ

    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        initialPostion = transform.position;
        switch (objectSys)
        {
            case "spawnOther":
                hp = 5;
                break;
            case "break":
                if (gameObject.tag == "Wood")
                    hp = 1;
                else
                    hp = 3;
                break;
        }
    }
    private void Update()
    {
        if (hp == 0)
        {           
            switch (objectSys)
            {
                case "spawnOther":
                    if (spawnNum < 1)
                    {                        
                        Instantiate(lowLevel, transform.position, Quaternion.identity) ;
                        spawnNum++;
                    }
                    else if (spawnNum > 0)
                    {
                        
                        Destroy(gameObject);
                    }
                    break;
                case "break":
                    Destroy(gameObject);
                    break;
            }
            
        }
        Animation();
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == tagName)//������ ������
        {
            audioSource.Play();
            dirNum = Random.Range(0, 4);//���� 4����
            print(other.name);
            other.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(ShakeAni());
            GameObject spawnPrefab = Instantiate(itemPrefab, spawnPoint.position,Quaternion.identity);//break�� ���ڸ���ȯ
            Rigidbody spawnPrefabRigi = spawnPrefab.GetComponent<Rigidbody>();
            if (objectSys == "spawnOther")
            {
                switch (dirNum)
                {
                    case 0:
                        prefabVec = spawnPoint.forward * 3 + Vector3.up * 3;
                        break;
                    case 1:
                        prefabVec = spawnPoint.right * 3 + Vector3.up * 3;
                        break;
                    case 2:
                        prefabVec = spawnPoint.forward * -3 + Vector3.up * 3;
                        break;
                    case 3:
                        prefabVec = spawnPoint.right * -3 + Vector3.up * 3;
                        break;
                }
                spawnPrefabRigi.AddForce(prefabVec, ForceMode.Impulse);
            }
            hp--;            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        audioSource.Stop();
    }
    void Animation()
    {
        switch (hp)//ü���� ������ ��鸮�� �ִϸ��̼�
        {
            case 0:
                
                break;
            default:
                //��鸮�� �ִϸ��̼�
                if (shakeTime < 0) 
                {           
                    shakeTime = 0.3f;
                    transform.position = initialPostion;
                }
                break;
        }
    }
    IEnumerator ShakeAni()
    {
        while(shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * shakeAmount + initialPostion;
            yield return new WaitForSeconds(0.01f);
            shakeTime -= Time.deltaTime;
        }        
    }
}
