using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//에셋에서 아이템 메뉴를 생성
//아이템 기본값을 만든다
[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class ItemType : ScriptableObject
{
    public enum Type { equip, used, etc }
    public string itemName;
    public Type type; //아이템타입
    public int num; //개수
    public Sprite itemImage; //아이템 이미지
    public GameObject prefab; //아이템 프리펩    
}
