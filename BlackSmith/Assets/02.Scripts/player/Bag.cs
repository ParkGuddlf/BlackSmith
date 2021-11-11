using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour
{
    Player player;

    public ItemType item;
    public int count;
    public Image itemImage;

    [SerializeField] Image bagImage;
    [SerializeField] Text textCount;
    //자신의 이미지 변경
    //자식의 개수 변경
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (item != null)
        {
            switch (item.type)
            {
                case ItemType.Type.equip://장비아이템이면 개수안보이게
                    bagImage.gameObject.SetActive(true);                    
                    bagImage.sprite = item.itemImage;
                    if (count > 1)
                        count = 1;
                    textCount.text = " ";
                    break;
                case ItemType.Type.used:
                    bagImage.gameObject.SetActive(true);
                    textCount.gameObject.SetActive(true);
                    bagImage.sprite = item.itemImage;
                    textCount.text = count.ToString();
                    break;
                case ItemType.Type.etc:
                    bagImage.gameObject.SetActive(true);
                    textCount.gameObject.SetActive(true);
                    bagImage.sprite = item.itemImage;
                    textCount.text = count.ToString();
                    break;
            }
        }
        if (item != null && count == 0)
        {
            item = null;
            bagImage.gameObject.SetActive(false);
            textCount.gameObject.SetActive(false);
        }
        if (count == 0)
            textCount.gameObject.SetActive(false);
        else
            textCount.gameObject.SetActive(true);
    }
    public void ItemInfo()
    {
        if (item == null)
            return;
        //누르면 착용 버리기 나오고 손에든거 끄고 누른거에대한 장비만 장착한다.
        switch (item.type)
        {
            case ItemType.Type.equip:
                player.toolName = item.name;
                break;
            case ItemType.Type.used:
                player.toolName = item.name;
                break;
            case ItemType.Type.etc:
                player.toolName = item.name;
                break;
        }
        for (int i = 0; i < transform.parent.childCount; i++)//누른버튼의 번호를 플레이안에 변수에 넣어준다
        {
            if (gameObject == transform.parent.GetChild(i).gameObject)
            {
                player.clickItemNum = i;
            }

        }
        //손에든 값을 받아와야된다
    }

    public void ChestToInventory()//아이템 창고에서 꺼내기
    {
        if (item == null)
            return;
        switch (item.type)
        {
            case ItemType.Type.equip:
                player.toolName = item.name;
                break;
            case ItemType.Type.used:
                player.toolName = item.name;
                break;
            case ItemType.Type.etc:
                player.toolName = item.name;
                break;
        }
        for (int i = 0; i < player.inventory.transform.childCount; i++)
        {
            if (player.inventory.transform.GetChild(i).GetComponent<Bag>().item == item)
            {
                count--;
                player.inventory.transform.GetChild(i).GetComponent<Bag>().count++;
            }
        }
        
        //플레이어의 인벤토리에서 같은 물건을 찾고 그물건의 값을 1개올린다
    }
    //마우스 가까이가져가면 이름나오고 스크롤이면 조합식까지나온다
}
