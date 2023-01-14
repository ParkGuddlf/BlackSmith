using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mine_Cut_Object : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;//아이쳄프리팹
    [SerializeField] Transform spawnPoint;//프리팹나오는 지점
    [SerializeField] GameObject lowLevel;//소환될 하위 오브젝트
    [SerializeField] string tagName; // 태그를 스트링으로 쓰기
    [SerializeField] string objectSys; //오브젝트방식
    int dirNum;// 방향결정
    Vector3 prefabVec; // 프리팹튀어나오는 방향
    [SerializeField] int hp; // 오브젝트 체력으로 횟수제한
    int spawnNum;
    Animator anim;

    public float shakeAmount;//흔드는힘
    float shakeTime = 0.3f;//흔드는시간
    Vector3 initialPostion;//흔들리는 위치

    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        initialPostion = transform.position;
        switch (objectSys)
        {
            case "spawnOther":
                hp = 4;
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
                        Instantiate(lowLevel, transform.position, Quaternion.identity);
                        Destroy(gameObject);
                   
                    break;
                case "break":
                    Destroy(gameObject);
                    break;
            }

        }
        Animation();

    }

    bool[] list_IsSpown = new bool[] { false, false, false, false };

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == tagName)//도끼로 찍을때
        {
            audioSource.Play();

            if (list_IsSpown.Where(n => n == true).LongCount() >= list_IsSpown.Length)
                return;
            for (int i = 0; i < list_IsSpown.Length; i++)
            {
                if (list_IsSpown[i] == false)
                {
                    list_IsSpown[i] = true;
                    dirNum = i;
                    break;
                }
            }
            print(other.name);
            other.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(ShakeAni());
            GameObject spawnPrefab = Instantiate(itemPrefab, spawnPoint.position,Quaternion.identity);//break면 제자리소환
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
        switch (hp)//체력이 있으면 흔들리는 애니메이션
        {
            case 0:
                
                break;
            default:
                //흔들리는 애니메이션
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
