using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class FragmentManager : MonoBehaviour
{
    private bool _isGrabbed = false;
    public bool IsGrabbed { get { return _isGrabbed; } }
    
    public string[] tagsAllowed;
    public float localControllerThresholdSpeed = 0.5f;
    
    public Rigidbody hiddenMesh;
    private FracturedChunk[] chunks;
    private int numFragments;
    private Dictionary<FracturedChunk, BoxCollider> chunkToColliderMap;
    
    private void Awake()
    {
        chunks = GetComponentsInChildren<FracturedChunk>();
        numFragments = chunks.Length;
        SetUpCompoundCollider();
    }
    
    public void WhenSelect()
    {
        StartCoroutine(IEWhenSelect());
    }

    public void WhenUnselect()
    {
        StopAllCoroutines();
        _isGrabbed = false;
    }

    public bool Check(Collision collision)
    {
        if (!CheckIfGrabbed()) return false;
        if (!CheckLocalControllerVelocity(localControllerThresholdSpeed)) return false;
        if (!CheckTagIsAllowed(collision)) return false;

        return true;
    }

    private bool CheckIfGrabbed()
    {
        if (IsGrabbed) Debug.Log("The stone is grabbed");
        else Debug.Log("The stone is not grabbed");
        
        return IsGrabbed;
    }
    
    private bool CheckLocalControllerVelocity(float thresholdSpeed)
    {
        float maxSpeed = GetMaxLocalControllerSpeed();
        
        if (maxSpeed > thresholdSpeed) Debug.Log("Local controller velocity is high enough: " + maxSpeed);
        else Debug.Log("Local controller velocity is low: " + maxSpeed);
        
        return maxSpeed > thresholdSpeed;
    }

    private bool CheckTagIsAllowed(Collision collision)
    {
        foreach (string objectTag in tagsAllowed)
        {
            if (collision.gameObject.CompareTag(objectTag))
            {
                Debug.Log("Tag Allowed");
                return true;
            }
        }

        Debug.Log("Tag not Allowed: " + collision.gameObject.tag);
        return false;
    }

    private IEnumerator IEWhenSelect()
    {
        yield return new WaitForSeconds(0.5f);
        _isGrabbed = true;
    }
    
    public void OnChunkCollision(FracturedChunk.CollisionInfo collisionInfo)
    {
        return;
    }
    
    public void OnDetachChunkCollision(FracturedChunk.CollisionInfo collisionInfo)
    {
        float localControllerSpeed = GetMaxLocalControllerSpeed();
        SetControllerVibration(localControllerSpeed, 0.2f);

        numFragments--;
        AdjustCollider(collisionInfo);
    }

    public void OnChunkDetach()
    {
        return;
    }

    private float GetMaxLocalControllerSpeed()
    {
        Vector3 leftHandVelocity  = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        Vector3 rightHandVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        float maxSpeed = Mathf.Max(leftHandVelocity.magnitude, rightHandVelocity.magnitude);
        
        return maxSpeed;
    }
    
    private void SetControllerVibration(float vibrationStrength, float vibrationDuration)
    {
        OVRInput.Controller[] controllers = { OVRInput.Controller.LTouch, OVRInput.Controller.RTouch };
        foreach (var controller in controllers)
        {
            OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, controller);
            StartCoroutine(StopVibrationAfterDelay(controller, vibrationDuration));   
        }
    }
        
    private IEnumerator StopVibrationAfterDelay(OVRInput.Controller controller, float delay)
    {
        yield return new WaitForSeconds(delay);
        OVRInput.SetControllerVibration(0, 0, controller);
    }

    private void SetUpCompoundCollider()
    {
        GameObject fragmentColliders = new GameObject("Fragment Colliders");
        fragmentColliders.transform.SetParent(hiddenMesh.transform);
        fragmentColliders.transform.localPosition = Vector3.zero;
        fragmentColliders.transform.localRotation = Quaternion.identity;
        fragmentColliders.transform.localScale = Vector3.one;

        chunkToColliderMap = new Dictionary<FracturedChunk, BoxCollider>();
        
        foreach (FracturedChunk chunk in chunks)
        {
            BoxCollider sourceCollider = chunk.GetComponent<BoxCollider>();
            BoxCollider destinationCollider = fragmentColliders.AddComponent<BoxCollider>();
            destinationCollider.center = sourceCollider.center + sourceCollider.transform.localPosition;
            destinationCollider.size = sourceCollider.size;
            destinationCollider.excludeLayers = 1 << LayerMask.NameToLayer("Fragment");
            
            chunkToColliderMap.Add(chunk, destinationCollider);
        }
    }

    private void AdjustCollider(FracturedChunk.CollisionInfo collisionInfo)
    {
        FracturedChunk detachedChunk = collisionInfo.chunk;
        BoxCollider colliderToDisable = chunkToColliderMap[detachedChunk];
        colliderToDisable.enabled = false;
    }
}
