using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int value;
    public Sprite icon;

    public bool isSlot;
    public Vector3 slotRotation = Vector3.zero;
    public Slot currentSlot;
    
}
