using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Random = UnityEngine.Random;

public class ForestTree : Item, IDamagable, ISliceable
{
    public float Health { get; private set; }
    [SerializeField] private float initHealth = 9.9f;
    
    [SerializeField]
    private Material crossSectionalMaterial;

    [SerializeField]
    private float respawnCooldown = 10f;
    
    [SerializeField]
    private GameObject[] damageEffects;
    
    [SerializeField]
    public AudioClip[] soundEffect;
    
    private void Awake()
    {
        respawnCooldown = Mathf.Max(respawnCooldown, 0);
    }
    
    private void OnEnable()
    {
        Health = initHealth;
    }
    
    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void TakeDamage(DamageInfo damageInfo) 
    {
        Debug.Log("TakeDamage in ForestTree class");
        
        float amount = 1f; // Take discrete damage
        if (amount <= 0) return;

        Health -= amount;
        Health = Mathf.Max(Health, 0);
        
        ApplyDamageEffect(damageInfo.hitInfo);
        
        Debug.Log("Current Health: " + Health);

        if (Health <= 0) 
        {
            ProcessSlice(damageInfo.hitInfo);
            return;
        }
    }

    private void ApplyDamageEffect(HitInfo hitInfo)
    {
        if (damageEffects == null || damageEffects.Length == 0) return;

        int choice = Random.Range(0, damageEffects.Length);
        GameObject effectPrefab = damageEffects[choice];
        GameObject effect = Instantiate(effectPrefab, hitInfo.hitPosition, Quaternion.identity);
    }
    
    public void Die()
    {
        SpawnManager.Instance.RespawnAfterDelay(this.gameObject, respawnCooldown);
    }

    public void ProcessSlice(HitInfo hitInfo)
    {
        GameObject target = this.gameObject;
        SlicedHull hull = target.Slice(hitInfo.hitPosition, hitInfo.planeNormal);
        if (hull == null) return;

        GameObject trunk = hull.CreateUpperHull(target, crossSectionalMaterial);
        GameObject root = hull.CreateLowerHull(target, crossSectionalMaterial);

        SetUpSlicedComponent(trunk, root);
        // SetUpSlicedComponent(trunk, false);
        // SetUpSlicedComponent(root, true);

        Die();
    }

    public void SetUpSlicedComponent(GameObject slicedObject, bool isRoot) 
    {
        Rigidbody rigidBody = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;

        if (isRoot)
        {
            rigidBody.isKinematic = true;
            
        }
        else
        {
            float radius = 5.0f, power = 5.0f;
            rigidBody.AddExplosionForce(power, slicedObject.transform.position, radius, power);
        }
    }

    private void SetUpSlicedComponent(GameObject trunk, GameObject root)
    {
        // Trunk
        Rigidbody trunkRigidBody = trunk.AddComponent<Rigidbody>();
        MeshCollider trunkCollider = trunk.AddComponent<MeshCollider>();
        ForestTreeTrunk trunkComponent = trunk.AddComponent<ForestTreeTrunk>();
        trunkRigidBody.isKinematic = false;
        trunkCollider.convex = true;
        trunkCollider.isTrigger = false;
        
        // Root
        Rigidbody rootRigidBody = root.AddComponent<Rigidbody>();
        MeshCollider rootCollider = root.AddComponent<MeshCollider>();
        ForestTreeRoot rootComponent = root.AddComponent<ForestTreeRoot>();
        rootRigidBody.isKinematic = true;
        rootCollider.convex = true;
        rootCollider.isTrigger = false;
        
        // Both
        trunkComponent.other = rootComponent;
        trunkComponent.itemInfo = this.itemInfo;
        
        rootComponent.other = trunkComponent;
        rootComponent.itemInfo = this.itemInfo;
        
        float radius = 5.0f, power = 5.0f;
        trunkRigidBody.mass = 100f;
        trunkRigidBody.AddExplosionForce(power, trunkRigidBody.transform.position, radius, power);
    }
    
    
}
