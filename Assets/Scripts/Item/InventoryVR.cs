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
    public List<Item> inventoryList = new List<Item>();
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

    public void AddtoInvenList(Item item)
    {
        inventoryList.Add(item);
    }

    public void RemoveFromInvenList(Item item)
    {
        inventoryList.Remove(item);
    }

    private void Start()
    {
        Inventory.SetActive(false);
        UIActive = false;
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            UIActive = !UIActive;
            Inventory.SetActive(UIActive);
        }
        //if (UIActive)
        //{
        //    Inventory.transform.position = Anchor.transform.position;
        //    Inventory.transform.eulerAngles = new Vector3(Anchor.transform.eulerAngles.x + 15, Anchor.transform.eulerAngles.y, 0);
        //}
    }
}