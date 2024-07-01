using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject ItemSlot;
    public Image slotImage;
    // Start is called before the first frame update
    void Start()
    {
        slotImage = GetComponentInChildren<Image>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (ItemSlot != null) return;

        GameObject obj = other.gameObject;

        if (!IsItem(obj)) return;
        
        if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
        {
            Debug.Log("Item Picked");
            other.GetComponent<ItemPickUp>().PickUp();
        }
    }

    bool IsItem(GameObject obj)
    {
        return obj.GetComponent<ItemPickUp>();
    }

    void InsertItem(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
