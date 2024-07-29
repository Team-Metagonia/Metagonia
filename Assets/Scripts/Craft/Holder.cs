using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Holder : MonoBehaviour, ISelectHandler
{
    public GameObject HoldingObject;
    public int itemQuantity;
    //Button btn;

    public void InstantiateItem(bool defaultValue)
    {
        Debug.Log("Instantiating Object...");
        ItemSO iteminfo = HoldingObject.GetComponent<Item>().itemInfo;
        if (InventoryVR.instance.itemQuantityPairs[iteminfo] <= 0)
        {
            Debug.Log($"No {iteminfo.itemName} Left. Check Inventory number.");
            return;

        }    
        Vector3 spawnPoint = CraftManager.Instance.itemSpawnPoint.position;
        Instantiate(HoldingObject,spawnPoint,Quaternion.identity);
        InventoryVR.instance.RemoveFromStackableList(HoldingObject.GetComponent<Item>().itemInfo);
        Debug.Log("Instantiate Finished");
        //itemQuantity--;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Toggle>().onValueChanged.AddListener(InstantiateItem);
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
