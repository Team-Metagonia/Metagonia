using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class ForestTree : MonoBehaviour, IDamagable, ISliceable
{
    public float Health { get; private set; }
    [SerializeField] private float initHealth;

    public Material crossSectionalMaterial;

    private void Awake()
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

        float amount = damageInfo.damage;
        if (amount <= 0) return;

        Health -= amount;
        Health = Mathf.Max(Health, 0);
        
        Debug.Log("Current Health: " + Health);

        if (Health <= 0) 
        {
            ProcessSlice(damageInfo.hitInfo);
            return;
        }
    }

    public void Die() 
    {
        Destroy(this.gameObject);
    }

    public void ProcessSlice(HitInfo hitInfo)
    {
        GameObject target = this.gameObject;
        SlicedHull hull = target.Slice(hitInfo.hitPosition, hitInfo.planeNormal);
        if (hull == null) return;

        GameObject upperHull = hull.CreateUpperHull(target);
        GameObject lowerHull = hull.CreateLowerHull(target);

        SetUpSlicedComponent(upperHull, false);
        SetUpSlicedComponent(lowerHull, true);

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
            float radius = 5.0f;
            float power = 10.0f;
            // TODO: use speed as cutForce
            rigidBody.AddExplosionForce(power, slicedObject.transform.position, 5f, 10f);
        }
    }

}
