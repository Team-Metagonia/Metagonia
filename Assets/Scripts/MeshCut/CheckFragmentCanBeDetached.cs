using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFragmentCanBeDetached : MonoBehaviour
{
    private bool _isGrabbed = false;
    public bool IsGrabbed { get { return _isGrabbed; } }
    
    public string[] tagsAllowed;
    public float localControllerThresholdSpeed = 0.5f;

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
        // if (!CheckIfGrabbed()) return false;
        if (!CheckLocalControllerVelocity(localControllerThresholdSpeed)) return false;
        if (!CheckTagIsAllowed(collision, tagsAllowed)) return false;

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
        Vector3 leftHandVelocity  = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        Vector3 rightHandVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        float maxSpeed = Mathf.Max(leftHandVelocity.magnitude, rightHandVelocity.magnitude);
        
        if (maxSpeed > thresholdSpeed) Debug.Log("Local controller velocity is high enough: " + maxSpeed);
        else Debug.Log("Local controller velocity is low: " + maxSpeed);
        
        return maxSpeed > thresholdSpeed;
    }

    private bool CheckTagIsAllowed(Collision collision, string[] tags)
    {
        foreach (string objectTag in tags)
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
}
