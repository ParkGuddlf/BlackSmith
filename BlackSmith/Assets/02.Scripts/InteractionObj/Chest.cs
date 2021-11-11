using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    // ��ư�� ������ �����⸦ ������ ��������� ������ ���´�

    public GameObject inventory;//â��ǰ
    bool hitSameBag = false;
    GameManager gm;
    public List<ItemType> sell_Itme;
    public int[] sell_Price;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    void Update()//â�� ���� �ֱ�
    {
        Debug.DrawRay(transform.position, transform.right * -1, Color.red, 0.5f);
        RaycastHit hitItem;//���̷� �÷��̾� �����˻�
        bool item = Physics.Raycast(transform.position, transform.right * -1, out hitItem, 0.5f, LayerMask.GetMask("Player"));
        if (item)
        {
            inventory.transform.parent.gameObject.SetActive(true);
        }
        else
            inventory.transform.parent.gameObject.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)//�Ǹ����� ������ �ڵ����� â�� ����
    {
        if (other.gameObject.layer == 7)//�ߺ��ξ����� ã�Ƽ� �߰��Ѵ�
        {
            int bagSoltNum = 0;
            ItemType itemType = other.GetComponent<ItemPickUp>().item;

            //����ߺ��˻��� �ϰ� �������� �����ش�
            for (int i = 0; i < inventory.transform.childCount; i++)
            {
                switch (itemType.type)
                {
                    case ItemType.Type.equip:
                        if (inventory.transform.GetChild(i).GetComponent<Bag>().item == itemType)
                        {
                            hitSameBag = true;
                            print("sss");
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
            sell_Itme.Add(other.GetComponent<ItemPickUp>().item);//����Ʈ�� �ִ´�
            Destroy(other.gameObject);
            hitSameBag = false;

        }

    }
    int k = 0;
    void SellPrice()
    {    
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            if (inventory.transform.GetChild(i).GetComponent<Bag>().item != null)
            {
                switch (inventory.transform.GetChild(i).GetComponent<Bag>().item.itemName)
                {
                    case "Wood":
                        k += inventory.transform.GetChild(i).GetComponent<Bag>().count * 20;
                        break;
                    case "stone":
                        k += inventory.transform.GetChild(i).GetComponent<Bag>().count * 20;
                        break;
                    case "Axe":
                        k += inventory.transform.GetChild(i).GetComponent<Bag>().count * 50;
                        break;
                    case "Pickax":
                        k += inventory.transform.GetChild(i).GetComponent<Bag>().count * 50;
                        break;
                    case "Hammer":
                        k += inventory.transform.GetChild(i).GetComponent<Bag>().count * 50;
                        break;
                }
            }
            inventory.transform.GetChild(i).GetComponent<Bag>().count = 0;
        }
        print(k);
        gm.money += k;
        k = 0;
    }
    //�ǸŹ�ư���� ���� �ȱ�
    public void SellBut()
    {
        SellPrice();
    }
}
