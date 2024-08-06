// Script name: InventoryVR
// Script purpose: attaching a gameobject to a certain anchor and having the ability to enable and disable it.
// This script is a property of Realary, Inc

// 인벤토리 오브젝트(현재는 UI 캔버스로 간단하게 구현 이후 3D 오브젝트 가방? 같은거로 변경예정)를 앵커에 맞춰서 부착해주는 기능
// 버튼 눌러서 껐다켰다 하는 기능
// 인벤토리 내의 아이템들을 저장해두는 리스트를 가지고 있음
// 리스트는 씬 변경과는 상관없이 항상 살아있어야 하니 싱글톤

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryVR : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 slotOffset;
    [SerializeField] Vector3 invenOffset;
    [SerializeField] InventoryUI[] ui;

    public static InventoryVR instance;
    public List<ItemSO> UnStackableList = new List<ItemSO>();
    public List<ItemSO> StackableList = new List<ItemSO>();

    public Dictionary<ItemSO,int> itemQuantityPairs = new Dictionary<ItemSO,int>();

    public ItemPickUp currentPicked;
    public List<GameObject> slots = new List<GameObject>();
    public GameObject quickSlotParent;
    public GameObject slotObject;
    public GameObject inventory;
    public GameObject quickSlotInventory;
    public bool UIActive;

    private void Awake()
    {
        if (instance==null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    public void AddToUnStackableList(ItemSO item, GameObject obj)
    {
        Debug.Log("Added to unstackable list");
        UnStackableList.Add(item);

        // Create slot Object
        GameObject slot = Instantiate(slotObject,quickSlotParent.transform);

        // Create New Item Prefab to put in Slot or Do not Destroy Original obj and just put it in??
        GameObject newItem = Instantiate(item.completePrefab);
        slot.GetComponentInChildren<InventorySlot>().InsertItem(newItem);

        slots.Add(slot);
    }

    public void RemoveFromUnStackableList(ItemSO item)
    {
        UnStackableList.Remove(item);
    }

    public void AddToStackableList(ItemSO item)
    {
        StackableList.Add(item);

        if(itemQuantityPairs.ContainsKey(item))
        {
            itemQuantityPairs[item] += item.quantity;
            foreach(InventoryUI i in ui)
            {
                i.UpdateHolder(item);
            }
            return;
        }
        itemQuantityPairs.Add(item, item.quantity);
        foreach(InventoryUI i in ui)
        {
            i.InstantiateHolder(item);
        }
    }

    public void RemoveFromStackableList(ItemSO item)
    {
        StackableList.Remove(item);

        if(itemQuantityPairs.ContainsKey(item)) 
        {
            itemQuantityPairs[item] -= item.quantity;
            foreach (InventoryUI i in ui)
            {
                i.UpdateHolder(item);
            }
            return;
        }
    }

    public void DeactivateQuickSlot()
    {
        quickSlotInventory.SetActive(false);
    }

    private void Start()
    {
        quickSlotInventory.SetActive(false);
        inventory.SetActive(false);
        UIActive = false;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Debug.Log("Pressed");
        }
        if(OVRInput.GetDown(OVRInput.Button.Three))
        {
            UIActive = !UIActive;
            inventory.SetActive(UIActive);
            quickSlotInventory.SetActive(UIActive);
        }

        quickSlotInventory.transform.position = player.transform.position + slotOffset;
        inventory.transform.position = player.transform.position + invenOffset;
    }

    [ContextMenu("Print Dictionary")]
    public void PrintDictionary()
    {
        foreach(var item in itemQuantityPairs) 
        {
            Debug.Log(item.Key);
            Debug.Log(item.Value);
        }
    }

    public bool AlreadyInInventory(ItemSO item)
    {
        return itemQuantityPairs.ContainsKey(item);
    }
}