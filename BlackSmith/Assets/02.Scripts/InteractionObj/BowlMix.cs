using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlMix : MonoBehaviour
{
    // 재작서에 재료를 알아내야된다
    // 제작서의 아이템타입을 검색한다 어떻게 찾을꺼냐
    // 제작서와 재료를 모두 배열에 넣고 제작서를 찾아서 내용을 알아낸다.
    // 제작서의 스트링배열의 이름과 배열의 아이템의 스트링값을 배열로 빼고 두개를 비교해서 같으면
    // 제작서의 결과물을 만들고 재료를 삭제한다
    // 제작서를 받아올 변수 아이템의 스트링값을 받아올 배열 제작스의 스트링값을 받아올 배열
    // 물건이나올 위치
    // 제작서의 집어 넣어야될거 최종결과물 이름과 프리팹 재료 배열
    [SerializeField] Scroll scroll; //조합서
    [SerializeField] List<ItemPickUp> selectlitem; //보울에 들어가있는 재료
    public bool start; // 시작버튼
    [SerializeField] Transform spawnPos; //완성품 나오는 위치
    public int oneTime = 0; //1회만 하게만드는 변수

    public float shakeAmount;//흔드는힘
    float shakeTime = 0.3f;//흔드는시간
    Vector3 initialPostion;//흔들리는 위치

    private void Start()
    {
        initialPostion = transform.position;
    }
    private void Update()
    {
        if (scroll != null)//조합서유무로 조합
        {
            //시작버튼을 누르면 ㅣㅅ작한다           
            if (start && oneTime == 0)
            {
                StartCoroutine(ShakeAni());
                oneTime++;
                for (int i = 0; i < scroll.itemRicipe.Length; i++)
                {                    
                    for (int j = 0; j < selectlitem.Count; j++)
                    {                        
                        if (scroll.itemRicipe[i] == selectlitem[j].item.itemName)
                        {
                            Destroy(selectlitem[j].gameObject);
                            selectlitem.RemoveAt(j);
                            if (selectlitem.Count <= 0)
                            {
                                print("asd");
                                GameObject finObject = Instantiate(scroll.mixPrefab, spawnPos.position, Quaternion.identity);
                                finObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 3 + Vector3.up * 3, ForceMode.Impulse);
                                Destroy(scroll.gameObject);
                                shakeTime = 0.3f;//흔들리는 시간 초기값으로 변경
                                transform.position = initialPostion;//흔들림 멈춤
                            }
                        }
                    }
                }
                for (int i = 0; i < selectlitem.Count; i++)//만든후 리스트삭제
                {
                    selectlitem.RemoveAt(i);
                }

            }          
            //스크롤의 배열과 리스트의 요소를 하나씩 비교해서 같으면 빼고 다빠져서 리스트의 길이가 0이되면 다삭제하고 최종 결과물을 만들어낸다
        }        

        //변수 초기화
        oneTime = 0;        
        start = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && other.tag != "Scroll")//리스트안에 재료 집어 넣기
        {
            selectlitem.Add(other.GetComponent<ItemPickUp>());
        }
        else if (other.tag == "Scroll")
        {
            scroll = other.GetComponent<Scroll>();
        }
    }
    IEnumerator ShakeAni()
    {
        while (shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * shakeAmount + initialPostion;
            yield return new WaitForSeconds(0.01f);
            shakeTime -= Time.deltaTime;
        }
    }
}
