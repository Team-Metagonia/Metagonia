using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Oculus.Interaction;
using Oculus.Interaction.Input;
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

    private int numFragments;
    private int numSupportFragments = 0;
    private Dictionary<FracturedChunk, Collider> chunkToColliderMap;

    private int numDetachedFragments = 0;
    public  int maxDetachedFragments = 3;
    public float collisionCooldown = 0.5f;
    
    public Transform root;
    public Rigidbody hiddenMesh;
    private FracturedChunk[] chunks;
    private FracturedChunk lastChunk;
    
    public GameObject prefabToInstantiateAfterDie;
    private GameObject instantiatedGameObject;

    private bool isDied = false;
    private bool isDying = false;
    
    private GrabInteractor _grabInteractor;
    private DistanceGrabInteractor _distanceGrabInteractor;

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    private GrabInteractor _lastGrabInteractor;
    private DistanceGrabInteractor _lastDistanceGrabInteractor;
    
    private void Awake()
    {
        InitializeChunkInfo();
        SetUpCompoundCollider();
    }

    private void Update()
    {
        if (isDied)
        {
            if (_isGrabbed)
            {
                instantiatedGameObject.transform.position = lastChunk.transform.position;
                instantiatedGameObject.transform.rotation = lastChunk.transform.rotation;
            
                // Handedness
                _lastGrabInteractor = _grabInteractor;
                _lastDistanceGrabInteractor = _distanceGrabInteractor;
                
                Handedness handedness = Handedness.Left;
                if (_grabInteractor != null)         handedness = _grabInteractor.GetComponent<ControllerRef>().Handedness;
                else if (_distanceGrabInteractor != null) handedness = _distanceGrabInteractor.GetComponent<ControllerRef>().Handedness;

                if (_grabInteractor == null && _distanceGrabInteractor == null)
                {
                    Debug.LogError("FragmentManage: Interactors are null. Something wrong");
                }
                if      (handedness == Handedness.Left)  OVRBrain.Instance.LeftHandObject  = instantiatedGameObject;
                else if (handedness == Handedness.Right) OVRBrain.Instance.RightHandObject = instantiatedGameObject;
                
                if      (handedness == Handedness.Left)  Debug.Log("Last grab hand is left");
                else if (handedness == Handedness.Right) Debug.Log("Last grab hand is right");
            }
            else if (!isDying)
            {
                lastPosition = lastChunk.transform.position;
                lastRotation = lastChunk.transform.rotation;

                instantiatedGameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                
                hiddenMesh.gameObject.SetActive(false);
                Debug.Log("HiddenMesh SetActive false => Check Item Collision work");
                
                StartCoroutine(IEDestroyNextFrame(root.gameObject));
                Debug.Log("Root GameObject Destroy => Check Item Collision");
                
                // Handedness
                Handedness lastHandedness = Handedness.Left;
                if (_lastGrabInteractor != null)
                    lastHandedness = _lastGrabInteractor.GetComponent<ControllerRef>().Handedness;
                else if (_lastDistanceGrabInteractor != null)
                    lastHandedness = _lastDistanceGrabInteractor.GetComponent<ControllerRef>().Handedness;
                
                if      (lastHandedness == Handedness.Left)  OVRBrain.Instance.LeftHandObject  = null;
                else if (lastHandedness == Handedness.Right) OVRBrain.Instance.RightHandObject = null;
                
                if      (lastHandedness == Handedness.Left)  Debug.Log("Last ungrab hand is left");
                else if (lastHandedness == Handedness.Right) Debug.Log("Last ungrab hand is right");
                
                
            }
        }
    }
    
    public void WhenSelect(PointerEvent pointerEvent)
    {
        StartCoroutine(IEWhenSelect(pointerEvent));
    }

    public void WhenUnselect()
    {
        StopAllCoroutines();
        _isGrabbed = false;

        _grabInteractor = null;
        _distanceGrabInteractor = null;
        
        numDetachedFragments = 0;
    }

    private void InitializeChunkInfo()
    {
        chunks = GetComponentsInChildren<FracturedChunk>();
        foreach (FracturedChunk chunk in chunks)
        {
            if (chunk.IsSupportChunk)
            {
                numSupportFragments++;
                
                lastChunk = chunk;
                lastChunk.GetComponent<Collider>().isTrigger = true;
            }

            chunk.gameObject.layer = LayerMask.NameToLayer("Fragment");
        }
        
        numFragments = chunks.Length;
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

    private IEnumerator IEWhenSelect(PointerEvent pointerEvent)
    {
        yield return new WaitForSeconds(0.5f);
        _isGrabbed = true;
        
        _grabInteractor = pointerEvent.Data as GrabInteractor;
        _distanceGrabInteractor = pointerEvent.Data as DistanceGrabInteractor;
    }
    
    public void OnChunkCollision(FracturedChunk.CollisionInfo collisionInfo)
    {
        return;
    }
    
    public void OnDetachChunkCollision(FracturedChunk.CollisionInfo collisionInfo)
    {   
        float localControllerSpeed = GetMaxLocalControllerSpeed();
        SetControllerVibration(localControllerSpeed, 0.2f);
        AdjustCollider(collisionInfo);
    }

    public void OnChunkDetach()
    {
        numFragments--;
        if (numFragments <= numSupportFragments) Die();

        CountDetachedFragment(collisionCooldown);
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
        fragmentColliders.gameObject.layer = LayerMask.NameToLayer("Ignore Collision");

        chunkToColliderMap = new Dictionary<FracturedChunk, Collider>();
        
        foreach (FracturedChunk chunk in chunks)
        {
            BoxCollider sourceCollider = chunk.GetComponent<BoxCollider>();
            BoxCollider destinationCollider = fragmentColliders.AddComponent<BoxCollider>();
            destinationCollider.excludeLayers = 
                (1 << LayerMask.NameToLayer("Fragment")) |
                (1 << LayerMask.NameToLayer("Ignore Collision"));
            
            destinationCollider.center = sourceCollider.center + sourceCollider.transform.localPosition;
            destinationCollider.size = sourceCollider.size;
            
            chunkToColliderMap.Add(chunk, destinationCollider);
        }

        /*foreach (FracturedChunk chunk in chunks)
        {
            GameObject newCollider = new GameObject("Fragment Collider");
            newCollider.transform.SetParent(fragmentColliders.transform);
            newCollider.transform.localPosition = chunk.transform.localPosition;
            newCollider.transform.localRotation = chunk.transform.localRotation;
            newCollider.transform.localScale = chunk.transform.localScale;
            newCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Collision");
            
            MeshCollider destinationCollider = newCollider.AddComponent<MeshCollider>();
            destinationCollider.excludeLayers = 
                (1 << LayerMask.NameToLayer("Fragment")) |
                (1 << LayerMask.NameToLayer("Ignore Collision"));
            
            destinationCollider.sharedMesh = chunk.GetComponent<MeshFilter>().sharedMesh;
            destinationCollider.convex = true;
            
            chunkToColliderMap.Add(chunk, destinationCollider);
        }*/
    }

    private void AdjustCollider(FracturedChunk.CollisionInfo collisionInfo)
    {
        FracturedChunk detachedChunk = collisionInfo.chunk;
        Collider colliderToDisable = chunkToColliderMap[detachedChunk];
        colliderToDisable.enabled = false;
    }

    private void Die()
    {
        isDied = true;
        numDetachedFragments = 0;
        
        lastChunk.gameObject.SetActive(false);
        instantiatedGameObject = Instantiate(prefabToInstantiateAfterDie, lastChunk.transform.position, lastChunk.transform.rotation);
        instantiatedGameObject.transform.SetParent(null);
    }
    
    private IEnumerator IEDestroyNextFrame(GameObject rootGameObject)
    {
        isDying = true;

        yield return null;
            
        instantiatedGameObject.transform.position = lastPosition;
        instantiatedGameObject.transform.rotation = lastRotation;   
        
        Destroy(rootGameObject);
    }

    private void CountDetachedFragment(float duration)
    {
        StartCoroutine(IECountDetachedFragment(duration));
    }
    
    private IEnumerator IECountDetachedFragment(float duration)
    {
        numDetachedFragments++;
        yield return new WaitForSeconds(duration);
        numDetachedFragments--;
    }

    public bool CheckNumDetachedFragment()
    {
        Debug.Log("Num Detach: " + numDetachedFragments);
        Debug.Log("Num Max: " + maxDetachedFragments);
        if (numDetachedFragments >= maxDetachedFragments)
        {
            Debug.Log("Check work");
        }
        return numDetachedFragments < maxDetachedFragments;
    }
}
