using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���¿��� ������ �޴��� ����
//������ �⺻���� �����
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class ItemType : ScriptableObject
{
    public enum Type { equip, used, etc }
    public string itemName;
    public Type type; //������Ÿ��
    public int num; //����
    public Sprite itemImage; //������ �̹���
    public GameObject prefab; //������ ������    
}
