using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 오브젝트에 부착해야하는 스크립트
// 아이템 정보 저장
// 아이템 획득 시 InventoryVR의 리스트에 해당 아이템 정보를 추가 후 아이템 파괴
public class ItemPickUp : MonoBehaviour
{
    public Item item;
    public void PickUp()
    {
        InventoryVR.instance.AddtoInvenList(item);
        Destroy(gameObject);
    }
}
