// Script name: InventoryVR
// Script purpose: attaching a gameobject to a certain anchor and having the ability to enable and disable it.
// This script is a property of Realary, Inc

// �κ��丮 ������Ʈ(����� UI ĵ������ �����ϰ� ���� ���� 3D ������Ʈ ����? �����ŷ� ���濹��)�� ��Ŀ�� ���缭 �������ִ� ���
// ��ư ������ �����״� �ϴ� ���
// �κ��丮 ���� �����۵��� �����صδ� ����Ʈ�� ������ ����
// ����Ʈ�� �� ������� ������� �׻� ����־�� �ϴ� �̱���

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

        slotInven.transform.position = player.transform.position + slotOffset;
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

    public bool AlreadyInInventory(Item item)
    {
        return itemQuantityPairs.ContainsKey(item);
    }
}