using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public GameObject particleEffect;
    private bool isColliding;

    [SerializeField] Transform headTransform;
    [SerializeField] Transform tipTransform;

    private void Awake()
    {
        isColliding = false;
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (isColliding) return;
        isColliding = true;
        
        Vector3 position = Vector3.zero;
        Vector3 normal = Vector3.zero;
        int cnt = 0;

        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.Log("Contact Point: " + contact.point);
            Debug.Log("Contact Normal: " + contact.normal);
            Debug.Log("Other Collider: " + contact.otherCollider.name);

            position += contact.point;
            normal += contact.normal;
            cnt += 1;
        }

        if (cnt == 0) return;
        
        position *= 1f / (float) cnt;
        normal *= 1f / (float) cnt;

        Vector3 vec = tipTransform.position - headTransform.position;
        float damage = Vector3.Dot(-vec.normalized, normal.normalized);

        // damage is large if it is closer to 1.0
        Debug.Log("Damage: " + damage);

        if (damage > 0f) Instantiate(particleEffect, position, Quaternion.identity);


        // Debug.Log("Average position: " + position);

        // OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        // OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        // OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        // OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);

        Vector3 leftHandVelocity  = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        Vector3 rightHandVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        float speed = Mathf.Max(leftHandVelocity.magnitude, rightHandVelocity.magnitude);

        Debug.Log("Speed: " + speed);


        // 양손 컨트롤러에 햅틱 피드백 제공
        float vibrationStrength = damage;
        float vibrationDuration = 0.1f;

        OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, OVRInput.Controller.LTouch);
        StartCoroutine(StopVibrationAfterDelay(OVRInput.Controller.LTouch, vibrationDuration));

        OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, OVRInput.Controller.RTouch);
        StartCoroutine(StopVibrationAfterDelay(OVRInput.Controller.RTouch, vibrationDuration));
    }

    private void OnCollisionExit(Collision collision) 
    {
        isColliding = false;
    }

    private IEnumerator StopVibrationAfterDelay(OVRInput.Controller controller, float delay)
    {
        yield return new WaitForSeconds(delay);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
