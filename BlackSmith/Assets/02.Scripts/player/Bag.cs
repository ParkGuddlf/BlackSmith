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
    //�ڽ��� �̹��� ����
    //�ڽ��� ���� ����
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
                case ItemType.Type.equip://���������̸� �����Ⱥ��̰�
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
        //������ ���� ������ ������ �տ���� ���� �����ſ����� ��� �����Ѵ�.
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
        for (int i = 0; i < transform.parent.childCount; i++)//������ư�� ��ȣ�� �÷��̾ȿ� ������ �־��ش�
        {
            if (gameObject == transform.parent.GetChild(i).gameObject)
            {
                player.clickItemNum = i;
            }

        }
        //�տ��� ���� �޾ƿ;ߵȴ�
    }

    public void ChestToInventory()//������ â���� ������
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
        
        //�÷��̾��� �κ��丮���� ���� ������ ã�� �׹����� ���� 1���ø���
    }
    //���콺 �����̰������� �̸������� ��ũ���̸� ���սı������´�
}
