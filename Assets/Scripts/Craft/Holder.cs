using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Oculus.Interaction.HandGrab;

public class Holder : MonoBehaviour, ISelectHandler
{
    public GameObject HoldingObject;
    public InventorySlot HoldingSlot;
    public int itemQuantity;
    Button btn;

    public void InstantiateItem(bool defaultValue)
    {
        //if (HoldingSlot.currentItem != null) return;

        Debug.Log("Instantiating Object...");
        ItemSO iteminfo = HoldingObject.GetComponentInChildren<Item>().itemInfo;
        
        if (InventoryVR.instance.itemQuantityPairs[iteminfo] <= 0)
        {
            Debug.Log($"No {iteminfo.itemName} Left. Check Inventory number.");
            return;
        }

        // Instantiate Slot in Spawn Point
        
        
        // Instantiate Item in HoldingSlot
        Vector3 spawnPoint = CraftManager.Instance.itemSpawnPoint.position;
        GameObject resultItem = Instantiate(HoldingObject,spawnPoint,Quaternion.identity);

        // Insert Item in Slot
        //HoldingSlot.InsertItem(resultItem);

        

        // Attach Instantiated Items in Any Hand (Default - Left Hand)
        //HandGrabInteractable[] interactables = resultItem.GetComponentsInChildren<HandGrabInteractable>();
        //AttachToHand(interactables);

        InventoryVR.instance.RemoveFromStackableList(HoldingObject.GetComponentInChildren<Item>().itemInfo);
        Debug.Log("Instantiate Finished");
        //itemQuantity--;
    }

    void AttachToHand(HandGrabInteractable[] interactable)
    {
        if (OVRBrain.Instance.LeftHandObject == null)
        {
            Debug.Log("LeftHand ForceSelect");
            OVRBrain.Instance.leftHandGrabInteractor.ForceSelect(interactable[0], false);
        }
        else if (OVRBrain.Instance.RightHandObject == null)
        {
            Debug.Log("RightHand ForceSelect");
            OVRBrain.Instance.rightHandGrabInteractor.ForceSelect(interactable[1], false);
        }
        else Debug.Log("Hand Full. Release One of your Object to Spawn Items in your hand!");
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Toggle>().onValueChanged.AddListener(InstantiateItem);
        //HoldingSlot = GameObject.Find("WorkBenchInstantiateSlot").GetComponent<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
       
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(this.gameObject.name + " was selected");
    }
}
