using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Item, IDamagable, IDroppable
{
    public float Health { get; private set; }
    
    [SerializeField] private int numsToDrop;
    [SerializeField] private float initHealth;
    [SerializeField] private bool dropAll;

    private bool quit = false;

    protected override void Awake()
    {
        base.Awake();
        Health = Mathf.Max(initHealth, Mathf.Epsilon);
    }

    private void OnEnable()
    {

    }

    private void OnDisable() 
    {
        DropItems();
    }


    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void TakeDamage(DamageInfo damageInfo) 
    {
        float amount = damageInfo.damage;
        if (amount <= 0) return;

        Health -= amount;
        Health = Mathf.Max(Health, 0);

        if (Health <= 0) 
        {       
            Die();
            return;
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    private void DropItem(GameObject itemToDrop)
    {
        if (quit) return;

        Vector2 randomPosition = Random.insideUnitCircle;
        
        Vector3 spawnPosition = new Vector3(
            this.transform.position.x + randomPosition.x, 
            this.transform.position.y, 
            this.transform.position.z + randomPosition.y
        );
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

    private void OnApplicationQuit()
    {
        quit = true;
    }
}
