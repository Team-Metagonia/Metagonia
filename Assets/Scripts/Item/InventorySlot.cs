using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public GameObject itemSlot;
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

        if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger) && IsItem(go))
        {

            Debug.Log("Item In Slot");
            
            InsertItem(go);
        }

        //if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && IsItem(go))
        //{

        //    Debug.Log("Item Out Slot");

        //    TakeOutItem(go);
        //}
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    GameObject go = other.gameObject;
    //    if (IsItem(go))
    //    {
    //        Debug.Log("Item Out Slot");
    //        go.GetComponent<ItemPickUp>().isInSlot = false;
    //        //go.GetComponent<Rigidbody>().useGravity = true;
    //        //go.GetComponent<Rigidbody>().isKinematic = false;
    //    }
    //}

    void TakeOutItem(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().isKinematic = false;
    }

    void InsertItem(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.parent = itemSlot.transform;
        obj.transform.position = itemSlot.transform.position;
        obj.GetComponent<ItemPickUp>().isInSlot = true;
        StartCoroutine(SlotItemMovement(obj));
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
