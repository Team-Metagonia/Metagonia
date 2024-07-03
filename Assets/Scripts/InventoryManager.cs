using UnityEngine;
using System.Collections.Generic;
using TMPro; // TMP(TextMeshPro)를 사용하기 위해 추가

public class InventoryManager : MonoBehaviour
{
    // 인벤토리 아이템 리스트
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    // TMP Text 참조
    public TMP_Text inventoryText; // 인스펙터에서 연결할 TMP Text

    // 싱글턴 인스턴스
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // 아이템을 인벤토리에 추가하는 메서드
    public void AddItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]++;
        }
        else
        {
            inventory[itemName] = 1;
        }

        UpdateInventoryUI(itemName);
    }

    // 인벤토리 내 아이템 개수 반환하는 메서드
    public int GetItemCount(string itemName)
    {
        return inventory.ContainsKey(itemName) ? inventory[itemName] : 0;
    }

    // TMP Text 업데이트 메서드
    private void UpdateInventoryUI(string itemName)
    {
        if (inventoryText != null)
        {
            inventoryText.text = $"{itemName}: {GetItemCount(itemName)}"; // 예시로 아이템 이름과 개수를 표시
        }
        else
        {
            Debug.LogWarning("InventoryText is not assigned. Cannot update UI.");
        }
    }
}