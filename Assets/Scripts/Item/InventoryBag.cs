using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InventoryBag : MonoBehaviour
{
    public UnityEvent OnPickUpToBag;

    private void OnTriggerStay(Collider other)
    {

        // ����� �ݶ��̴��� ���� ���·� ��ü�� ������ bag�� ����
        // InventorySlot�� �ش� �������� �ڽ� ������Ʈ�� �� ������ ������Ʈ�� �������� ������
        // ĵ���� Horizontal layoutGroup�� ���� �ؾ��ҵ� �ڵ����� �ٸ��缭 �����ǰԲ�
        // �̷��� ���ָ� InventorySlot�� ���� ����� �׳� �� �� ���� �� �ϴ�

        //Initialize triggered item as new object go
        GameObject go = other.gameObject;

        if ((OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger)) 
            && go.TryGetComponent<Item>(out Item i))
        {
            if (!i.distanceGrabbed) return;
            Debug.Log($"{i.itemInfo.name} Item In Bag");
            i.PickUp();
            OnPickUpToBag?.Invoke();
        }
    }
}
