using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestTreeRoot : MonoBehaviour, IDroppable
{
    public ItemSO itemInfo;
    public ForestTreeTrunk other;
    public float respawnCooldown;
    
    public int numsToDrop = 1;
    public bool dropAll = true;
    
    private bool quit = false;
    
    private void OnApplicationQuit()
    {
        quit = true;
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void Die()
    {
        DropItems();
        Destroy(this.gameObject);
    }
    
    public void DropItems()
    {
        if (quit) return;
        if (itemInfo == null)
        {
            Debug.Log("itemInfo of ForestTreeRoot is null");
            return;    
        }

        if (itemInfo.dropItems == null || itemInfo.dropItems.Length == 0)
        {
            Debug.Log("DropItems are null or DropItems Length is 0");
            return;   
        }

        if (dropAll) 
        {
            for (int i = 0; i < itemInfo.dropItems.Length; i++)
            {
                GameObject itemToDrop = itemInfo.dropItems[i];
                DropItem(itemToDrop);
            }
            return;
        }
        
        if (numsToDrop < 0) numsToDrop = 1;
        for (int i = 0; i < numsToDrop; i++)
        {
            int choice = Random.Range(0, itemInfo.dropItems.Length);
            GameObject itemToDrop = itemInfo.dropItems[choice];
            DropItem(itemToDrop);
        }
    }
    
    private void DropItem(GameObject itemToDrop)
    {
        if (quit) return;

        Vector2 randomPosition = Random.insideUnitCircle;

        Vector3 spawnPosition = this.transform.position;
        spawnPosition.x += randomPosition.x;
        spawnPosition.z += randomPosition.y;
        
        Quaternion spawnRotation = this.transform.rotation;

        GameObject go = Instantiate(itemToDrop, spawnPosition, spawnRotation);
        Debug.Log("Item Dropped! " + go.transform.position);
    }
}
