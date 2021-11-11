using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollATM : MonoBehaviour
{
    //������ ������ ��ũ���̳��´�
    //�Ϸ翡 ������� ���� �Ƿڰ���
    //�Ƿڰ����� 0���� �ȳ�����
    //�������̸� ���ο� �Ƿڰ��´�
    public bool setScroll = true;//�Ƿ� ������ ������
    int missionNum;//�׳� �Ƿ� ����
    public List<GameObject> QuestNum;//��ũ���� �Ƿڰ�����ŭ �־��ش�.
    public GameObject[] scrollPrefab;//��ũ�������ճ־��ֱ�
    public int spawnNum; //��ȯ����
    void Update()
    {
        if (setScroll)//���ο� �迭 �Ǵ� ����Ʈ�� ��ũ�� �������� �־��ش�
        {
            QuestNum.RemoveAll(T=>T);
            missionNum = Random.Range(1, 4);
            for (int i = 0; i < missionNum; i++)
            {
                QuestNum.Add(scrollPrefab[Random.Range(0, scrollPrefab.Length)]);
            }
            setScroll = false;
        }
        if(transform.GetChild(0).transform.childCount == 0)//�Ѱ������ �ݰ� �ٽø����
            SpwanScroll();//�÷��̾ �����̿ͼ� Z�� ������ ���� �����ȴ�
    }
    void SpwanScroll()
    {
        RaycastHit playerHit;
        bool isHit = Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward, out playerHit, 1, LayerMask.GetMask("Player"));
        if (Input.GetKeyDown(KeyCode.Z) && isHit && spawnNum < QuestNum.Count)//��ȯ������ �ϳ��÷��ָ鼭 ���� ��ȯ�ϰ�
        {
            GameObject scroll = Instantiate(QuestNum[spawnNum], transform.GetChild(0).transform.position, Quaternion.identity);
            scroll.transform.SetParent(transform.GetChild(0).transform);
            spawnNum++;            
        }
    }
}
