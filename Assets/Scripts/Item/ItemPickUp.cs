using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ������Ʈ�� �����ؾ��ϴ� ��ũ��Ʈ
// ������ ���� ����
// ������ ȹ�� �� InventoryVR�� ����Ʈ�� �ش� ������ ������ �߰� �� ������ �ı�
public class ItemPickUp : MonoBehaviour
{
    public Item item;
    public void PickUp()
    {
        InventoryVR.instance.AddtoInvenList(item);
        Destroy(gameObject);
    }
}
