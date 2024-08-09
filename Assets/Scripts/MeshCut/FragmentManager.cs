using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Input;
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

    private FracturedObject FracturedObjectSource;
    private FracturedChunk[] chunks;
    private FracturedChunk lastChunk;
    
    public GameObject prefabToInstantiateAfterDie;
    private GameObject instantiatedGameObject;

    private bool isDied = false;
    private bool isFinished = false;
    private bool shouldInitializeAterStart = true;
    
    private GrabInteractor _grabInteractor;
    private DistanceGrabInteractor _distanceGrabInteractor;
    private Handedness currentHandedness;
    
    private void Awake()
    {
        InitializeChunkInfo();
        SetUpCompoundCollider();
    }

    private void Update()
    {
        if (shouldInitializeAterStart)
        {
            shouldInitializeAterStart = false;
            foreach (FracturedChunk chunk in chunks) chunk.gameObject.SetActive(true);
        }
        
        if (isDied)
        {
            if (_isGrabbed)
            {
                if      (currentHandedness == Handedness.Left)  OVRBrain.Instance.LeftHandObject  = instantiatedGameObject;
                else if (currentHandedness == Handedness.Right) OVRBrain.Instance.RightHandObject = instantiatedGameObject;
                
                instantiatedGameObject.transform.position = lastChunk.transform.position;
                instantiatedGameObject.transform.rotation = lastChunk.transform.rotation;
            }
            else 
            {
                if (!isFinished)
                {
                    isFinished = true;
                    
                    if      (currentHandedness == Handedness.Left)  OVRBrain.Instance.LeftHandObject  = null;
                    else if (currentHandedness == Handedness.Right) OVRBrain.Instance.RightHandObject = null;
                    
                    Destroy(this.gameObject);
                }
            }
        }
    }
    
    public void WhenSelect(PointerEvent pointerEvent)
    {
        StartCoroutine(IEWhenSelect(pointerEvent));
    }

    private IEnumerator IEWhenSelect(PointerEvent pointerEvent)
    {
        yield return new WaitForSeconds(0.5f);
        _isGrabbed = true;
        
        _grabInteractor = pointerEvent.Data as GrabInteractor;
        _distanceGrabInteractor = pointerEvent.Data as DistanceGrabInteractor;
        
        Handedness handedness = Handedness.Left;
        if      (_grabInteractor != null)         handedness = _grabInteractor.GetComponent<ControllerRef>().Handedness;
        else if (_distanceGrabInteractor != null) handedness = _distanceGrabInteractor.GetComponent<ControllerRef>().Handedness;

        if (_grabInteractor == null && _distanceGrabInteractor == null)
        {
            Debug.LogError("FragmentManager: Interactors in grab are null. Something wrong");
        }

        currentHandedness = handedness;
        
        if      (handedness == Handedness.Left)  OVRBrain.Instance.LeftHandObject  = gameObject;
        else if (handedness == Handedness.Right) OVRBrain.Instance.RightHandObject = gameObject;
                
        if      (handedness == Handedness.Left)  Debug.Log("Last grab hand is left");
        else if (handedness == Handedness.Right) Debug.Log("Last grab hand is right");
    }
    
    public void WhenUnselect(PointerEvent pointerEvent)
    {
        _grabInteractor = pointerEvent.Data as GrabInteractor;
        _distanceGrabInteractor = pointerEvent.Data as DistanceGrabInteractor;
        
        Handedness handedness = Handedness.Left;
        if      (_grabInteractor != null)         handedness = _grabInteractor.GetComponent<ControllerRef>().Handedness;
        else if (_distanceGrabInteractor != null) handedness = _distanceGrabInteractor.GetComponent<ControllerRef>().Handedness;

        if (_grabInteractor == null && _distanceGrabInteractor == null)
        {
            Debug.LogError("FragmentManager: Interactors in ungrab are null. Something wrong");
        }
        
        if      (handedness == Handedness.Left)  OVRBrain.Instance.LeftHandObject  = null;
        else if (handedness == Handedness.Right) OVRBrain.Instance.RightHandObject = null;
                
        if      (handedness == Handedness.Left)  Debug.Log("Last ungrab hand is left");
        else if (handedness == Handedness.Right) Debug.Log("Last ungrab hand is right");

        StopAllCoroutines();
        
        _isGrabbed = false;
        numDetachedFragments = 0;
        
        _grabInteractor = null;
        _distanceGrabInteractor = null;
    }

    private void InitializeChunkInfo()
    {
        FracturedObjectSource = this.GetComponent<FracturedObject>();
        chunks = GetComponentsInChildren<FracturedChunk>();
        foreach (FracturedChunk chunk in chunks)
        {
            if (chunk.IsSupportChunk)
            {
                numSupportFragments++;
                
                lastChunk = chunk;
                lastChunk.GetComponent<Collider>().isTrigger = true;
            }
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
    
    public void OnChunkCollision(FracturedChunk.CollisionInfo collisionInfo)
    {
        return;
    }
    
    public void OnDetachChunkCollision(FracturedChunk.CollisionInfo collisionInfo)
    {   
        float localControllerSpeed = GetMaxLocalControllerSpeed();
        SetControllerVibration(localControllerSpeed, 0.2f);
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
        foreach (FracturedChunk chunk in chunks)
        {
            if (chunk.IsSupportChunk)
            {
                FracturedChunk supportChunk = chunk;
                MeshCollider chunkCollider = supportChunk.GetComponent<MeshCollider>();
                chunkCollider.isTrigger = true;

                MeshCollider newCollider = this.gameObject.AddComponent<MeshCollider>();
                newCollider.sharedMesh = chunkCollider.sharedMesh;
                newCollider.convex = true;
                newCollider.isTrigger = true;
            }

            // Prevent chunk's collider is registered in interactable's CollectionItems
            chunk.gameObject.SetActive(false);
        }
    }

    private void Die()
    {
        isDied = true;
        numDetachedFragments = 0;
        
        lastChunk.gameObject.SetActive(false);
        instantiatedGameObject = Instantiate(prefabToInstantiateAfterDie, lastChunk.transform.position, lastChunk.transform.rotation);
        instantiatedGameObject.transform.SetParent(null);
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
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        if (collision.contacts == null || collision.contacts.Length == 0) return;
        
        FracturedChunk thisChunk = collision.GetContact(0).thisCollider.GetComponent<FracturedChunk>();
        
        if(collision.gameObject)
        {
            FracturedChunk otherChunk = collision.gameObject.GetComponent<FracturedChunk>();

            if(otherChunk)
            {
                if(otherChunk.GetComponent<Rigidbody>() == null && thisChunk.IsDetachedChunk == false)
                {
                    // Just intersecting with other chunk in kinematic state
                    return;
                }
            }
        }
        
        Debug.Log("FracturedChunk: collision.gameObject pass");

        float fMass = Mathf.Infinity; // If there is no rigidbody we consider it static
        if(collision.rigidbody) fMass = collision.rigidbody.mass;
        
        Debug.Log("Fractured Chunk: collision.rigidbody pass");

        if(thisChunk.IsDetachedChunk == false)
        {
            // Chunk still attached.
            // We are going to check if the collision is against a free chunk of the same object. This way we prevent chunks pushing each other out, we want to control
            // this only through the FractureObject.InterconnectionStrength variable

            bool bOtherIsFreeChunkFromSameObject = false;

            FracturedChunk otherChunk = collision.gameObject.GetComponent<FracturedChunk>();

            if(otherChunk != null)
            {
                if(otherChunk.IsDetachedChunk == true && otherChunk.FracturedObjectSource == FracturedObjectSource)
                {
                    bOtherIsFreeChunkFromSameObject = true;
                }
            }
            
            Debug.Log("Chunk Collision: " + collision.gameObject.name);
            
            if(bOtherIsFreeChunkFromSameObject == false && collision.relativeVelocity.magnitude > FracturedObjectSource.EventDetachMinVelocity && fMass > FracturedObjectSource.EventDetachMinMass && thisChunk.IsDestructibleChunk())
            {
                FracturedChunk.CollisionInfo collisionInfo = new FracturedChunk.CollisionInfo(thisChunk, collision, true);
                
                if (!Check(collision)) return;
                FracturedObjectSource.NotifyChunkCollision(collisionInfo);
                
                if (!CheckNumDetachedFragment()) return;
                FracturedObjectSource.NotifyDetachChunkCollision(collisionInfo);

                if(collisionInfo.bCancelCollisionEvent == false)
                {
                    Debug.Log("Fragment Manager: collisionInfo.bCancelCollisionEvent == false");
                    
                    List<FracturedChunk> listBreaks = new List<FracturedChunk>();

                    // Impact enough to make it detach. Compute random list of connected chunks that are detaching as well (we'll use the ConnectionStrength parameter).
                    listBreaks = thisChunk.ComputeRandomConnectionBreaks();
                    listBreaks.Add(thisChunk);
                    thisChunk.DetachFromObject();
                    
                    Debug.Log("Fragment Manager: thisChunk.DetachFromObject();");

                    foreach(FracturedChunk chunk in listBreaks)
                    {
                        collisionInfo.chunk = chunk;
                        collisionInfo.bIsMain = false;
                        collisionInfo.bCancelCollisionEvent = false;

                        if(chunk != thisChunk)
                        {
                            FracturedObjectSource.NotifyDetachChunkCollision(collisionInfo);
                        }

                        if(collisionInfo.bCancelCollisionEvent == false)
                        {
                            chunk.DetachFromObject();
                            chunk.GetComponent<Rigidbody>().AddExplosionForce(collision.relativeVelocity.magnitude * FracturedObjectSource.EventDetachExitForce, collision.contacts[0].point, 0.0f, FracturedObjectSource.EventDetachUpwardsModifier);
                        }
                    }
                }
            }
        }
        else
        {
            // Free chunk

            if(collision.relativeVelocity.magnitude > FracturedObjectSource.EventDetachedMinVelocity && fMass > FracturedObjectSource.EventDetachedMinMass)
            {
                FracturedObjectSource.NotifyFreeChunkCollision(new FracturedChunk.CollisionInfo(thisChunk, collision, true));
            }
        }
    }

}
