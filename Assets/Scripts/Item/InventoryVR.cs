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
    [SerializeField] OVRPlayerController player;
    [SerializeField] Vector3 slotOffset;
    [SerializeField] Vector3 invenOffset;
    [SerializeField] InventoryUI ui;

    public static InventoryVR instance;
    public List<Item> UnStackableList = new List<Item>();
    public List<Item> StackableList = new List<Item>();

    public Dictionary<Item,int> itemQuantityPairs = new Dictionary<Item,int>();

    public ItemPickUp currentPicked;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public GameObject slotInven;
    //public GameObject slotAnchor;
    public GameObject inventory;
    bool UIActive;

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

    public void AddToUnStackableList(Item item)
    {
        UnStackableList.Add(item);
    }

    public void RemoveFromUnStackableList(Item item)
    {
        UnStackableList.Remove(item);
    }

    public void AddToStackableList(Item item)
    {
        StackableList.Add(item);

        if(itemQuantityPairs.ContainsKey(item))
        {
            itemQuantityPairs[item] += item.quantity;
            ui.UpdateHolder(item);
            return;
        }
        itemQuantityPairs.Add(item, item.quantity);
        ui.InstantiateHolder(item);
    }

    public void RemoveFromStackableList(Item item)
    {
        StackableList.Remove(item);
    }

    private void Start()
    {
        inventory.SetActive(false);
        UIActive = false;
    }

    private void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Three))
        {
            UIActive = !UIActive;
            inventory.SetActive(UIActive);
        }

        //slotInven.transform.position = player.transform.position + slotOffset;
        //inventory.transform.position = player.transform.position + invenOffset;
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

    public bool AlreadyInInventory(Item item)
    {
        return itemQuantityPairs.ContainsKey(item);
    }
}