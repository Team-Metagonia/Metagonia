using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class LithicReduction : MonoBehaviour, IDamagable, ISliceable
{
    public float Health { get; private set; }
    private float initHealth = 0.1f;

    public Material crossSectionalMaterial;
    // public GameObject blueprintStone;

    public BlueprintStone blueprintStone;

    // private MeshCollider blueprintStoneCollider;
    // private Transform origin;

    // private float epsilon = 0.05f;

    private bool isCarveFinished = false;

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
        if (isCarveFinished) 
        {
            Debug.Log("Carve Finished -> Early Return in Take Damage");
            return;
        }

        Debug.Log("Take Damage work");

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
        if (isCarveFinished) return;
        
        GameObject target = this.gameObject;
        int numSlice = blueprintStone.GetFaceCount() / 10;
        for (int i = 0; i < numSlice; i++) 
        {
            HitInfo hit = blueprintStone.GetRemainingTriangleHitInfo();
            if (hit == null) 
            {
                Debug.Log("There is no remaining Triangles!");
                isCarveFinished = true;
                return;
            }

            Vector3 centroid = hit.hitPosition;
            Vector3 normal = hit.planeNormal;

            // Slice 실행
            SlicedHull hull = target.Slice(centroid, normal);
            if (hull == null) continue;

            GameObject upperHull, lowerHull;

            if (crossSectionalMaterial != null) 
            {
                upperHull = hull.CreateUpperHull(target, crossSectionalMaterial);
                lowerHull = hull.CreateLowerHull(target, crossSectionalMaterial);
            }
            else
            {
                upperHull = hull.CreateUpperHull(target);
                lowerHull = hull.CreateLowerHull(target);
            }

            SetUpSlicedComponent(upperHull, false);
            SetUpSlicedComponent(lowerHull, true);
            SetUpBlueprintStone(lowerHull);
            
            Destroy(target);
            target = lowerHull;
        }
    }



    // private void ProcessSliceLegacy(HitInfo hitInfo)
    // {
    //     GameObject target = this.gameObject;
    //     blueprintStoneCollider = this.blueprintStone.GetComponent<MeshCollider>();
    //     origin = blueprintStone.transform;
        
    //     SlicedHull hull;

    //     // Hit 이 일어난 포인트에서 법선 방향으로 Ray를 발사한다
    //     // 1. Ray가 내부 Stone 에 맞은 경우 해당 지점의 접평면을 사용
    //     // 2. Ray가 내부 Stone 에 맞지 않은 경우 현재 지점으로부터 가장 가까운 지점의 접평면을 사용

    //     HitInfo rayHitInfo = CheckRayHit(hitInfo, blueprintStoneCollider);
    //     if (rayHitInfo != null) hull = ProcessRayHit(rayHitInfo, blueprintStoneCollider);
    //     else hull = ProcessRayNotHit(hitInfo, blueprintStoneCollider);
        
    //     if (rayHitInfo != null) Debug.Log("Ray hit exists => slice using tangent vector");
    //     else Debug.Log("No ray hit => just slice");

    //     if (hull == null) return;
    //     if (hull != null) Debug.Log("Hull exist => Do slice");

    //     // Upper Hull, Lower Hull setup
    //     GameObject upperHull, lowerHull; 

    //     if (crossSectionalMaterial != null) 
    //     {
    //         upperHull = hull.CreateUpperHull(target, crossSectionalMaterial);
    //         lowerHull = hull.CreateLowerHull(target, crossSectionalMaterial);
    //     }
    //     else
    //     {
    //         upperHull = hull.CreateUpperHull(target);
    //         lowerHull = hull.CreateLowerHull(target);
    //     }

    //     SetUpSlicedComponent(upperHull);
    //     SetUpSlicedComponent(lowerHull);

    //     Die();
    // }

    // private HitInfo CheckRayHit(HitInfo hitInfo, MeshCollider other)
    // {
    //     if (IsPointInsideMesh(hitInfo.hitPosition, other)) 
    //     {
    //         Debug.Log("Point is inside the mesh in Check Ray Hit");
    //         return null;
    //     }

    //     Vector3 point = hitInfo.hitPosition;
    //     Vector3 direction = -hitInfo.hitNormal;
	// 	RaycastHit[] hits = Physics.RaycastAll(point, direction, Mathf.Infinity);

	// 	foreach(RaycastHit hit in hits) 
    //     {
	// 		if (hit.collider != other) continue;
    //         if (!CheckDistance(hitInfo.hitPosition, hit.point)) 
    //         {
    //             Debug.Log("Short distance in check ray hit");
    //             continue;
    //         }
            
    //         return new HitInfo(hit.point, hit.normal, hit.normal);
	// 	}

    //     return null;
    // }

    // private SlicedHull ProcessRayHit(HitInfo hitInfo, MeshCollider other)
    // {
    //     GameObject target = this.gameObject;
    //     SlicedHull hull = target.Slice(hitInfo.hitPosition, hitInfo.planeNormal);
    //     return hull;
    // }

    // private SlicedHull ProcessRayNotHit(HitInfo hitInfo, MeshCollider other)
    // {
    //     if (IsPointInsideMesh(hitInfo.hitPosition, other))
    //     {
    //         Debug.Log("Point is inside the mesh in Process Ray Not Hit");
    //         return null;
    //     }

    //     SlicedHull hull = null;
    //     GameObject target = this.gameObject;

    //     Debug.Log("Plane Hit case");
        
    //     Vector3 closestPoint = other.ClosestPoint(hitInfo.hitPosition);
    //     if (!CheckDistance(hitInfo.hitPosition, closestPoint))
    //     {
    //         Debug.Log("Short distance in plane hit");
    //         return null;
    //     }

    //     Vector3 direction = closestPoint - hitInfo.hitPosition;
    //     RaycastHit[] hits = Physics.RaycastAll(hitInfo.hitPosition, direction, Mathf.Infinity);

    //     foreach(RaycastHit hit in hits) 
    //     {
    //         if (hit.collider != other) continue;
            
    //         hull = target.Slice(hit.point, hit.normal);
    //         return hull;
    //     }

    //     return hull;
    // }

    private void SetUpSlicedComponent(GameObject slicedObject, bool isRoot)
    {
        // Collider and Rigidbody
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;

        Rigidbody rigidBody = slicedObject.AddComponent<Rigidbody>();
        rigidBody.isKinematic = false;

        if (!isRoot) return;

        LithicReduction lithicReduction = slicedObject.AddComponent<LithicReduction>();
        lithicReduction.blueprintStone = this.blueprintStone;
        if (crossSectionalMaterial != null) lithicReduction.crossSectionalMaterial = this.crossSectionalMaterial;
        
        // Freeze Position and Rotation 
        rigidBody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        // blueprintStone.InjectTargetTransform(slicedObject.transform);

        // Set Grabbable
        SetUpGrabbable(slicedObject);
    }

    // private void SetUpSlicedComponent(GameObject slicedObject) 
    // {
    //     LithicReduction lithicReduction = slicedObject.AddComponent<LithicReduction>();
    //     lithicReduction.blueprintStone = this.blueprintStone;
    //     if (crossSectionalMaterial != null) lithicReduction.crossSectionalMaterial = this.crossSectionalMaterial;

    //     // Collider and Rigidbody
    //     MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
    //     collider.convex = true;

    //     Rigidbody rigidBody = slicedObject.AddComponent<Rigidbody>();
    //     rigidBody.isKinematic = false;

    //     bool isRoot = IsPointInsideMesh(origin.position, collider);
    //     if (!isRoot) return;
        
    //     // Freeze Position and Rotation 
    //     rigidBody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    //     blueprintStone.GetComponent<BlueprintStone>().InjectTargetTransform(slicedObject.transform);

    //     // Set Grabbable
    //     SetUpGrabbable(slicedObject);
    // }

    // private bool IsPointInsideMesh(Vector3 point, MeshCollider other)
    // {
    //     // How to detect which side of a face a ray hit? I.e., Raycast only Backfaces?
    //     // return other.bounds.Contains(point);

    //     // All Mesh Colliders are assumed to be convex
    //     // so we can expect the center of bound is inside of the mesh
    //     Vector3 center = other.bounds.center;
    //     Vector3 direction = center - point;
	// 	RaycastHit[] hits = Physics.RaycastAll(point, direction);
        
	// 	foreach(RaycastHit hit in hits) 
    //     {
	// 		if (hit.collider != other) continue;
            
    //         // we hit it so we were outside it
    //         Debug.Log("The Point is outside the mesh!");
    //         return false;
	// 	}
		
	// 	// no hits probably means we're inside it
    //     Debug.Log("The Point is inside the mesh!");
	// 	return true;
    // }

    // private bool CheckDistance(Vector3 first, Vector3 second)
    // {
    //     float dist = Vector3.Distance(first, second);
    //     return (dist > epsilon);
    // }

    private void SetUpGrabbable(GameObject parent)
    {
        // Add Grab Interaction
        GameObject interaction = new GameObject("Hand Grab Interaction");
        interaction.transform.SetParent(parent.transform);
        interaction.transform.localPosition = Vector3.zero;
        interaction.transform.localRotation = Quaternion.identity;

        Rigidbody rigidBody = parent.GetComponent<Rigidbody>();
        if (rigidBody == null) rigidBody = parent.AddComponent<Rigidbody>();

        Grabbable grabbable = interaction.AddComponent<Grabbable>();
        HandGrabInteractable handGrabInteractable = interaction.AddComponent<HandGrabInteractable>();
        
        // Grabbable Injection
        ITransformer oneGrabTransformer = interaction.AddComponent<OneGrabRotateTransformer>() as ITransformer;
        grabbable.InjectOptionalTargetTransform(parent.transform);
        grabbable.InjectOptionalOneGrabTransformer(oneGrabTransformer);

        // ITransformer twoGrabTransformer = interaction.AddComponent<TwoGrabFreeTransformer>() as ITransformer;
        // grabbable.InjectOptionalTwoGrabTransformer(twoGrabTransformer) causes Null Reference Error ...
        // To solve this, we can Initialize transformer
        // i.e. twoGrabTransformer.Initialize(grabbable) 
        // But this causes weird behavior ...

        // HandGrabInteractable Injection
        handGrabInteractable.InjectRigidbody(rigidBody);
        handGrabInteractable.InjectOptionalPointableElement(grabbable);
        HandGrabInteractable.Registry.Register(handGrabInteractable);
    }

    private void SetUpBlueprintStone(GameObject parent)
    {
        this.blueprintStone.gameObject.transform.SetParent(parent.transform);
    }
}
