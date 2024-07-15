using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    // Current Slot GameObject - in order to get transform & position
    public GameObject itemSlot;

    // Current Item in Slot
    public Item currentItem;

    private void OnTriggerStay(Collider other)
    {
        //Initialize triggered item as new object go
        GameObject go = other.gameObject;

        if ((OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger)||OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger)) && IsItem(go))
        {
            Debug.Log("Item In Slot");
            
            InsertItem(go);
        }
    }

    void InsertItem(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.parent = itemSlot.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<ItemPickUp>().isInSlot = true;
        currentItem = obj.GetComponent<ItemPickUp>().item;
    }

    bool IsItem(GameObject obj)
    {
        return obj.TryGetComponent<ItemPickUp>(out ItemPickUp i); 
    }
}
