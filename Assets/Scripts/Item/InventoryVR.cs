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
    public static InventoryVR instance;
    public List<Item> UnStackableList = new List<Item>();
    public List<Item> StackableList = new List<Item>();

    public ItemPickUp currentPicked;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public GameObject Inventory;
    public GameObject Anchor;
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
    }

    public void RemoveFromStackableList(Item item)
    {
        StackableList.Remove(item);
    }

    private void Start()
    {
        //Inventory.SetActive(false);
        //UIActive = false;
    }

    private void Update()
    {

      
    }
}