using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InventoryBag : MonoBehaviour
{
    public UnityEvent OnPickedUpToBag;

    private void OnTriggerStay(Collider other)
    {

        // ����� �ݶ��̴��� ���� ���·� ��ü�� ������ bag�� ����
        // InventorySlot�� �ش� �������� �ڽ� ������Ʈ�� �� ������ ������Ʈ�� �������� ������
        // ĵ���� Horizontal layoutGroup�� ���� �ؾ��ҵ� �ڵ����� �ٸ��缭 �����ǰԲ�
        // �̷��� ���ָ� InventorySlot�� ���� ����� �׳� �� �� ���� �� �ϴ�

        //Initialize triggered item as new object go
        GameObject go = other.gameObject;

        if (go.TryGetComponent<Item>(out Item i)  )
        {
            Debug.Log($"{go.name} is item");
            Debug.Log(i._currentHandInteractor);
            if((OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger)) && !i.isInBag )
            {
                Debug.Log($"{i.itemInfo.itemName} Item In Bag");
                i.isInBag = true;
                i.PickUp();
                OnPickedUpToBag?.Invoke();
            }
           
        }
    }
}
