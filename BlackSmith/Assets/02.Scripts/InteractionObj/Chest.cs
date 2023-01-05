using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    // 버튼을 눌러서 꺼내기를 누르면 가방안으로 물건이 들어온다

    public GameObject inventory;//창고물품
    bool hitSameBag = false;
    GameManager gm;
    public List<ItemType> sell_Itme;
    public int[] sell_Price;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    void Update()//창고에 물건 넣기
    {
        Debug.DrawRay(transform.position, transform.right * -1, Color.red, 0.5f);
        RaycastHit hitItem;//레이로 플레이어 유무검사
        bool item = Physics.Raycast(transform.position, transform.right * -1, out hitItem, 0.5f, LayerMask.GetMask("Player"));
        if (item)
        {
            inventory.transform.parent.gameObject.SetActive(true);
        }
        else
            inventory.transform.parent.gameObject.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)//판매존에 넣으면 자동으로 창고에 들어간다
    {
        if (other.gameObject.layer == 7)//중복인아이템 찾아서 추가한다
        {
            int bagSoltNum = 0;
            ItemType itemType = other.GetComponent<ItemPickUp>().item;

            Bag[] _childList = inventory.transform.GetComponentsInChildren<Bag>();
            //장비중복검색을 하고 참거짓을 보내준다
            for (int i = 0; i < inventory.transform.childCount; i++)
            {
                switch (itemType.type)
                {
                    case ItemType.Type.equip:
                        if (_childList[i].item == itemType)
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
                if (_childList[i].item != null && itemType.itemName == inventory.transform.GetChild(i).GetComponent<Bag>().item.itemName && !hitSameBag)//같은 아이템 있는지 검사
                {                                                                                                                                                                             //1번칸에 아이템갯수가 5개면 다음 칸으로 옮긴다 중복되는 장비가 있으면 다음칸으로 옮긴다
                    _childList[i].count += itemType.num;//장비를 중복해서 인벤토리에 못넣는다
                    bagSoltNum = i;
                    break;
                }
                else if (_childList[i].item == null)//없으면 가장낮은 널에 집어넣는다
                {
                    _childList[i].item = itemType;
                    _childList[i].count += itemType.num;
                    break;
                }
            }
            sell_Itme.Add(other.GetComponent<ItemPickUp>().item);//리스트에 넣는다
            Destroy(other.gameObject);
            hitSameBag = false;

        }

    }
    int totalPrice = 0;
    void SellPrice()
    {
        //물건 자체에 가격을 정해서 넣어서 개수에 가격을 곱해주기 Sellitem(Bag(가방정보))
        Bag[] _childList = inventory.transform.GetComponentsInChildren<Bag>();
        for (int i = 0; i < inventory.transform.childCount; i++)
        {
            if (_childList[i].item != null)
            {
                ItemSellPrice(_childList[i]);
            }
            totalPrice += _childList[i].count = 0;
        }
        print(totalPrice);
        gm.money += totalPrice;
        totalPrice = 0;
    }
    //아이템 판매
    void ItemSellPrice(Bag _bagitem)
    {
        totalPrice += _bagitem.count * _bagitem.item.price;
    }
    //판매버튼으로 물건 팔기
    public void SellBut()
    {
        SellPrice();
    }
}
