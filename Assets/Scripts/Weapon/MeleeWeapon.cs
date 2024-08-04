using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

enum HandDominance
{
    Left, Right, Both
}

public class MeleeWeapon : Item
{
    private bool isColliding;
    private bool isRapidColliding = false;

    [SerializeField] private bool ignoreHitDirection = true;
    
    [SerializeField] private float hitPoint;

    [SerializeField] Transform headTransform;
    [SerializeField] Transform bodyTransform;
    [SerializeField] Transform tipTransform;
    public GameObject[] damageEffects;

    [SerializeField] private float thresholdLocalControllerSpeed = 1f;
    
    private Vector3 previousHeadPosition;
    private Vector3 previousBodyPosition;
    private Vector3 previousTipPosition;

    private Quaternion previousRotation;


    protected override void Awake()
    {
        isRapidColliding = false;
    }

    private void LateUpdate()
    {
        previousHeadPosition = this.headTransform.position;
        previousBodyPosition = this.bodyTransform.position;
        previousTipPosition  = this.tipTransform.position;

        previousRotation = this.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision) 
    {
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
        {
            this.transform.rotation = previousRotation;
            //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
            //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            // StartCoroutine(F());
            
            HandleDamagable(collision);
            return;
        }
    }
    
    

    private void OnCollisionExit(Collision collision) 
    {
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        
        Debug.Log("OnCollisionExit: " + collision.gameObject.name);
    }

    private void HandleDamagable(Collision collision)
    {
        if (isRapidColliding) return;
        
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

        DamageInfo damageInfo = CalculateAngleBasedDamage(collision);
        float damage = damageInfo.damage;

        if (ignoreHitDirection)
        {
            damage = hitPoint;
            damageInfo.damage = damage;
        }
        
        if (damage <= 0) return;
        Debug.Log("Damage: " + damage);

        float speed = GetMaxLocalControllerSpeed();
        if (speed <= thresholdLocalControllerSpeed) return;

        damagable.TakeDamage(damageInfo);
        ApplyDamageEffect(damageInfo);
        PreventRapidCollision();

        // Give Haptics
        float vibrationStrength = (hitPoint > 0) ? damage / hitPoint : 0;
        // float vibrationDuration = 0.1f;
        // UseControllerVibration(vibrationStrength, vibrationDuration);
        UseControllerVibrationWhileColliding(vibrationStrength, HandDominance.Both);
        
        
    }

    #region Calculate Damage

    DamageInfo CalculateAngleBasedDamage(Collision collision)
    {
        Vector3 position = Vector3.zero;
        Vector3 normal = Vector3.zero;
        int cnt = 0;

        foreach (ContactPoint contact in collision.contacts)
        {
            position += contact.point;
            normal += contact.normal;
            cnt += 1;
        }

        if (cnt == 0) return new DamageInfo();
        
        position *= 1f / (float) cnt;
        normal *= 1f / (float) cnt;

        // Vector3 vec = tipTransform.position - headTransform.position;
        Vector3 vec = previousTipPosition - previousHeadPosition;
        float damage = Vector3.Dot(-vec.normalized, normal.normalized);
        damage = Mathf.Max(damage, 0);
        damage *= hitPoint;
        
        // 0 <= damage <= 1

        // Get Plane Normal
        // Vector3 x = headTransform.position - tipTransform.position;
        // Vector3 y = headTransform.position - bodyTransform.position;
        Vector3 x = previousHeadPosition - previousTipPosition;
        Vector3 y = previousHeadPosition - previousBodyPosition;
        
        x = x.normalized;
        y = y.normalized;
        Vector3 planeNormal = Vector3.Cross(x, y);
        if (planeNormal.y < 0) planeNormal = -planeNormal;

        float cos = Vector3.Dot(planeNormal, Vector3.up);
        float epsilon = 0.0001f;
        cos = Mathf.Clamp(cos, epsilon, 1f - epsilon);

        float theta = Mathf.Acos(cos);

        // Use Mirrored Plane Normal if theta is too large
        if (theta > Mathf.PI / 4f)
        {
            float xzDist = Mathf.Sqrt(planeNormal.x * planeNormal.x + planeNormal.z * planeNormal.z);
            float newY = xzDist * Mathf.Tan(theta);
            planeNormal.y = newY;
            planeNormal = planeNormal.normalized;
        }

        return new DamageInfo(damage, new HitInfo(position, normal, planeNormal));
    }

    #endregion

    #region Damage Effect

    void ApplyDamageEffect(DamageInfo damageInfo)
    {
        Vector3 position = damageInfo.hitInfo.hitPosition;
        if (damageEffects == null || damageEffects.Length == 0) return;

        int choice = Random.Range(0, damageEffects.Length);
        GameObject damageEffect = damageEffects[choice];
        Instantiate(damageEffect, position, Quaternion.identity);
    }

    #endregion

    #region Vibration

    private void UseControllerVibration(float vibrationStrength, float vibrationDuration, HandDominance dominance)
    {
        // Process Left Hand
        if (dominance == HandDominance.Left || dominance == HandDominance.Both)
        {
            OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, OVRInput.Controller.LTouch);
            StartCoroutine(StopVibrationAfterDelay(OVRInput.Controller.LTouch, vibrationDuration));
        }

        // Process Right Hand
        if (dominance == HandDominance.Right || dominance == HandDominance.Both)
        {
            OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, OVRInput.Controller.RTouch);
            StartCoroutine(StopVibrationAfterDelay(OVRInput.Controller.RTouch, vibrationDuration));
        }
    }

    private void UseControllerVibrationWhileColliding(float vibrationStrength, HandDominance dominance)
    {
        // Process Left Hand
        if (dominance == HandDominance.Left || dominance == HandDominance.Both)
        {
            OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, OVRInput.Controller.LTouch);
            StartCoroutine(StopVibrationAfterCollisionExit(OVRInput.Controller.LTouch));
        }

        // Process Right Hand
        if (dominance == HandDominance.Right || dominance == HandDominance.Both)
        {
            OVRInput.SetControllerVibration(vibrationStrength, vibrationStrength, OVRInput.Controller.RTouch);
            StartCoroutine(StopVibrationAfterCollisionExit(OVRInput.Controller.RTouch));
        }
    }

    private IEnumerator StopVibrationAfterDelay(OVRInput.Controller controller, float delay)
    {
        yield return new WaitForSeconds(delay);
        OVRInput.SetControllerVibration(0, 0, controller);
    }

    private IEnumerator StopVibrationAfterCollisionExit(OVRInput.Controller controller)
    {
        while (isColliding) 
        {
            yield return null;
        }
    
        OVRInput.SetControllerVibration(0, 0, controller);
    }
    
    #endregion
    
    #region Local Controller Speed

    private float GetMaxLocalControllerSpeed()
    {
        Vector3 leftHandVelocity  = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        Vector3 rightHandVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        float maxSpeed = Mathf.Max(leftHandVelocity.magnitude, rightHandVelocity.magnitude);
        return maxSpeed;
    }
    
    #endregion

    #region Prevent Rapid Collision

    private void PreventRapidCollision()
    {
        StartCoroutine(IEPreventRapidCollision());
    }
    
    private IEnumerator IEPreventRapidCollision()
    {
        isRapidColliding = true;
        yield return new WaitForSeconds(0.5f);
        isRapidColliding = false;
    }

    #endregion

    private IEnumerator F()
    {
        yield return new WaitForSeconds(0.1f);

        var originalConstraints = this.GetComponent<Rigidbody>().constraints; 
        var unfreezePosition = ~RigidbodyConstraints.FreezePosition;  
        this.GetComponent<Rigidbody>().constraints = originalConstraints & unfreezePosition;
    }
}
