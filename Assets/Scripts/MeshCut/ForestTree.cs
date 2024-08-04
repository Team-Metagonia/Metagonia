using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Random = UnityEngine.Random;

public class ForestTree : MonoBehaviour, IDamagable, ISliceable
{
    public ItemSO itemInfo;
    public int numsToDrop = 1;
    public bool dropAll = true;
    
    public float Health { get; private set; }
    [SerializeField] private float initHealth = 9.9f;
    
    [SerializeField]
    private Material crossSectionalMaterial;

    [SerializeField]
    private float respawnCooldown = 60f;
    
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
        Die();
    }

    private void SetUpSlicedComponent(GameObject trunk, GameObject root)
    {
        // Trunk
        Rigidbody trunkRigidBody = trunk.AddComponent<Rigidbody>();
        MeshCollider trunkCollider = trunk.AddComponent<MeshCollider>();
        ForestTreeTrunk trunkComponent = trunk.AddComponent<ForestTreeTrunk>();
        trunkRigidBody.isKinematic = false;
        trunkRigidBody.useGravity = true;
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
        trunkComponent.transform.position = this.transform.position;
        trunkComponent.respawnCooldown = this.respawnCooldown;
        
        rootComponent.other = trunkComponent;
        rootComponent.itemInfo = this.itemInfo;
        rootComponent.transform.position = this.transform.position;
        rootComponent.respawnCooldown = this.respawnCooldown;
        rootComponent.numsToDrop = this.numsToDrop;
        rootComponent.dropAll = this.dropAll;

        // Layer Setting
        trunk.layer = this.gameObject.layer;
        root.layer  = this.gameObject.layer;
        
        // int defaultLayer = LayerMask.NameToLayer("Default");
        // trunkCollider.excludeLayers = 1 << defaultLayer;
        // rootCollider.excludeLayers  = 1 << defaultLayer;
        
        // Explosion
        float radius = 1.0f, power = 5.0f;
        trunkRigidBody.mass = 1f;
        trunkRigidBody.AddExplosionForce(power, trunkRigidBody.transform.position, radius, power);
    }
}
