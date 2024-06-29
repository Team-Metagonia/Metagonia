using Script;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private bool isInventoryOpen = false;

    private void OnEnable()
    {
        if (KeyboardInputManager.Instance != null)
        {
            KeyboardInputManager.Instance.InventoryAction += ToggleInventory;
        }
        else
        {
            Debug.LogError("KeyboardInputManager.Instance is null. Ensure KeyboardInputManager is correctly set up.");
        }
    }

    private void OnDisable()
    {
        if (KeyboardInputManager.Instance != null)
        {
            KeyboardInputManager.Instance.InventoryAction -= ToggleInventory;
        }
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (isInventoryOpen)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }

    private void OpenInventory()
    {
        Debug.Log("Inventory opened");
        // 인벤토리를 열 때 필요한 처리들을 여기에 추가
    }

    private void CloseInventory()
    {
        Debug.Log("Inventory closed");
        // 인벤토리를 닫을 때 필요한 처리들을 여기에 추가
    }
}