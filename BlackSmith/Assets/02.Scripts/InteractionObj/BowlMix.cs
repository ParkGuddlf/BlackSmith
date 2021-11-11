using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlMix : MonoBehaviour
{
    // ���ۼ��� ��Ḧ �˾Ƴ��ߵȴ�
    // ���ۼ��� ������Ÿ���� �˻��Ѵ� ��� ã������
    // ���ۼ��� ��Ḧ ��� �迭�� �ְ� ���ۼ��� ã�Ƽ� ������ �˾Ƴ���.
    // ���ۼ��� ��Ʈ���迭�� �̸��� �迭�� �������� ��Ʈ������ �迭�� ���� �ΰ��� ���ؼ� ������
    // ���ۼ��� ������� ����� ��Ḧ �����Ѵ�
    // ���ۼ��� �޾ƿ� ���� �������� ��Ʈ������ �޾ƿ� �迭 ���۽��� ��Ʈ������ �޾ƿ� �迭
    // �����̳��� ��ġ
    // ���ۼ��� ���� �־�ߵɰ� ��������� �̸��� ������ ��� �迭
    [SerializeField] Scroll scroll; //���ռ�
    [SerializeField] List<ItemPickUp> selectlitem; //���￡ ���ִ� ���
    public bool start; // ���۹�ư
    [SerializeField] Transform spawnPos; //�ϼ�ǰ ������ ��ġ
    public int oneTime = 0; //1ȸ�� �ϰԸ���� ����

    public float shakeAmount;//������
    float shakeTime = 0.3f;//���½ð�
    Vector3 initialPostion;//��鸮�� ��ġ

    private void Start()
    {
        initialPostion = transform.position;
    }
    private void Update()
    {
        if (scroll != null)//���ռ������� ����
        {
            //���۹�ư�� ������ �Ӥ����Ѵ�           
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
                                shakeTime = 0.3f;//��鸮�� �ð� �ʱⰪ���� ����
                                transform.position = initialPostion;//��鸲 ����
                            }
                        }
                    }
                }
                for (int i = 0; i < selectlitem.Count; i++)//������ ����Ʈ����
                {
                    selectlitem.RemoveAt(i);
                }

            }          
            //��ũ���� �迭�� ����Ʈ�� ��Ҹ� �ϳ��� ���ؼ� ������ ���� �ٺ����� ����Ʈ�� ���̰� 0�̵Ǹ� �ٻ����ϰ� ���� ������� ������
        }        

        //���� �ʱ�ȭ
        oneTime = 0;        
        start = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && other.tag != "Scroll")//����Ʈ�ȿ� ��� ���� �ֱ�
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
