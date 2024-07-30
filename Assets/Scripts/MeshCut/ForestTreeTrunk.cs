using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestTreeTrunk : Item, IDroppable
{
    public ForestTreeRoot other;

    [SerializeField] private int numsToDrop = 2;
    [SerializeField] private bool dropAll = true;

    [SerializeField] private float dieDelayTime = 2.5f;

    private bool isDying = false;
    private bool quit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable() 
    {
        DropItems();
    }
    
    private void OnApplicationQuit()
    {
        quit = true;
    }
    
    private void DropItem(GameObject itemToDrop)
    {
        if (quit) return;

        Vector2 randomPosition = Random.insideUnitCircle;

        Vector3 spawnPosition = this.transform.position;
        Quaternion spawnRotation = this.transform.rotation;

        Instantiate(itemToDrop, spawnPosition, spawnRotation);
    }
    
    public void DropItems()
    {
        if (quit) return;
        if (itemInfo == null) return;
        if (itemInfo.dropItems == null || itemInfo.dropItems.Length == 0) return;

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

    public void Die()
    {
        other.Die();
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDying) return;
        
        int terrainLayer = LayerMask.NameToLayer("Terrain");
        if (collision.gameObject.layer == terrainLayer)
        {
            isDying = true;
            StartCoroutine(DieAfterDelay(dieDelayTime));
        }
    }

    private IEnumerator DieAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Die();
    }
}
