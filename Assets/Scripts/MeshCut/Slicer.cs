using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

using UnityEngine.InputSystem;

public class Slicer : MonoBehaviour
{
    public Transform planeDebug;
    public GameObject target;
    private float cutForce;

    private void Start()
    {
        
    }

    public void Slice(GameObject target) 
    {
        SlicedHull hull = target.Slice(planeDebug.position, planeDebug.up);
        if (hull == null) return;
        
        GameObject upperHull = hull.CreateUpperHull(target);
        GameObject lowerHull = hull.CreateLowerHull(target);
    }

    public void SetUpSlicedComponent(GameObject slicedObject) 
    {
        Rigidbody rigidBody = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;

        rigidBody.AddExplosionForce(cutForce, slicedObject.transform.position, 1f);
    }

}
