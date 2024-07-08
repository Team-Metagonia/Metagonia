using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    // Current Slot GameObject - in order to get transform & position
    public GameObject itemSlot;

    // Current Item in Slot
    public Item currentItem;

    // Move code below to Item script in near future..
    // Amplitude of the bobbing effect
    public float amplitude = 0.5f;
    // Frequency of the bobbing effect
    public float frequency = 1f;

    // Initial position of the GameObject
    private Vector3 startPosition;

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
        //StartCoroutine(SlotItemMovement(obj));
    }

    IEnumerator SlotItemMovement(GameObject obj)
    {
        startPosition = transform.localPosition;

        while (obj.GetComponent<ItemPickUp>().isInSlot)
        {
            // Calculate the new Y position
            float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
            // Set the new position of the GameObject
            obj.transform.position = new Vector3(startPosition.x, newY, startPosition.z);

            // Wait for the next frame before continuing the loop
            yield return null;
        }

        //transform.position = startPosition;
    }

    bool IsItem(GameObject obj)
    {
        return obj.TryGetComponent<ItemPickUp>(out ItemPickUp i); 
    }
}
