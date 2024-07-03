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