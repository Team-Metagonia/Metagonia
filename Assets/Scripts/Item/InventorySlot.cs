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

    public void InsertItem(GameObject obj)
    {
        Debug.Log("Insert");
        if(obj.GetComponent<Rigidbody>().isKinematic==false)
        {
            obj.GetComponent<Rigidbody>().isKinematic = true;
        }
        obj.transform.parent = itemSlot.transform;
        obj.transform.localPosition = new Vector3(0,0,0);
        obj.transform.rotation = Quaternion.identity;
        obj.GetComponent<ItemPickUp>().isInSlot = true;
        currentItem = obj.GetComponent<ItemPickUp>().item;
    }

    bool IsItem(GameObject obj)
    {
        return obj.TryGetComponent<ItemPickUp>(out ItemPickUp i); 
    }
}
