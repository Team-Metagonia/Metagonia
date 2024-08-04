using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InventoryBag : MonoBehaviour
{
    public UnityEvent OnPickedUpToBag;

    private void OnTriggerStay(Collider other)
    {

        // 등뒤의 콜라이더에 닿은 상태로 물체를 놓으면 bag에 들어간다
        // InventorySlot에 해당 아이템이 자식 오브젝트로 들어간 상태의 오브젝트를 동적으로 생성함
        // 캔버스 Horizontal layoutGroup에 들어가게 해야할듯 자동으로 줄맞춰서 생성되게끔
        // 이렇게 해주면 InventorySlot의 원래 기능은 그냥 쓸 수 있을 듯 하다

        //Initialize triggered item as new object go
        GameObject go = other.gameObject;

        if (go.TryGetComponent<Item>(out Item i)  )
        {
            Debug.Log($"{go.name} is item");
            Debug.Log(i._currentHandInteractor);
            if ((OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger)) &&
                !i.isInBag)
            {
                // How to Check if the Item is currently in player's hand??
                Debug.Log($"{i.itemInfo.itemName} Item In Bag");
                i.isInBag = true;
                i.PickUp();
                OnPickedUpToBag?.Invoke();
                
            }
           
        }
    }
}
