using UnityEngine;

public class TreeInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem("Tree");

            

            // 추가적인 동작을 수행하고 싶다면 여기에 추가합니다.
        }
    }
}