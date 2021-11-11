using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollATM : MonoBehaviour
{
    //무작위 아이템 스크롤이나온다
    //하루에 몇개나올지 랜덤 의뢰개수
    //의뢰개수가 0개면 안나오게
    //다음날이면 새로운 의뢰가온다
    public bool setScroll = true;//의로 받은지 참거짓
    int missionNum;//그날 의뢰 개수
    public List<GameObject> QuestNum;//스크롤을 의뢰개수만큼 넣어준다.
    public GameObject[] scrollPrefab;//스크롤프리팹넣어주기
    public int spawnNum; //소환개수
    void Update()
    {
        if (setScroll)//새로운 배열 또는 리스트에 스크롤 프리팹을 넣어준다
        {
            QuestNum.RemoveAll(T=>T);
            missionNum = Random.Range(1, 4);
            for (int i = 0; i < missionNum; i++)
            {
                QuestNum.Add(scrollPrefab[Random.Range(0, scrollPrefab.Length)]);
            }
            setScroll = false;
        }
        if(transform.GetChild(0).transform.childCount == 0)//한개만들고 줍고 다시만들기
            SpwanScroll();//플레이어가 가까이와서 Z를 누르면 옆에 생성된다
    }
    void SpwanScroll()
    {
        RaycastHit playerHit;
        bool isHit = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward, out playerHit, 1, LayerMask.GetMask("Player"));
        if (Input.GetKeyDown(KeyCode.Z) && isHit && spawnNum < QuestNum.Count)//소환개수를 하나늘려주면서 옆에 소환하고
        {
            GameObject scroll = Instantiate(QuestNum[spawnNum], transform.GetChild(0).transform.position, Quaternion.identity);
            scroll.transform.SetParent(transform.GetChild(0).transform);
            spawnNum++;            
        }
    }
}
