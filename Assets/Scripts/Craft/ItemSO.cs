using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class ItemSO : ScriptableObject
{
    public enum ItemType { Material, Weapon }
    public int id;
    public string itemName;
    public int value;
    public int quantity;
    public ItemType type;
    public Sprite icon;
    public GameObject simplePrefab;

    public bool isSlot;
    public Vector3 slotRotation = Vector3.zero;
    public Slot currentSlot;

    public GameObject[] dropItems;
}
