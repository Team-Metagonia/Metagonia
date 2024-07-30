using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum HandDominance
{
    Left, Right, Both
}

public class MeleeWeapon : Item
{
    private bool isColliding;
    
    [SerializeField] private float hitPoint;

    [SerializeField] Transform headTransform;
    [SerializeField] Transform bodyTransform;
    [SerializeField] Transform tipTransform;
    public GameObject[] damageEffects;

    protected override void Awake()
    {
        isColliding = false;
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (isColliding) return;
        isColliding = true;
        
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        if (damagable != null) 
        {
            HandleDamagable(collision);
            return;
        }
    }

    private void OnCollisionExit(Collision collision) 
    {
        isColliding = false;
    }

    private void HandleDamagable(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

        DamageInfo damageInfo = CalculateAngleBasedDamage(collision);
        float damage = damageInfo.damage;

        if (damage <= 0) return;
        Debug.Log("Damage: " + damage);

        damagable.TakeDamage(damageInfo);
        ApplyDamageEffect(damageInfo);

        // Vector3 leftHandVelocity  = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        // Vector3 rightHandVelocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        // float speed = Mathf.Max(leftHandVelocity.magnitude, rightHandVelocity.magnitude);

        // Debug.Log("Speed: " + speed);


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

        Vector3 vec = tipTransform.position - headTransform.position;
        float damage = Vector3.Dot(-vec.normalized, normal.normalized);
        damage = Mathf.Max(damage, 0);
        damage *= hitPoint;
        
        // 0 <= damage <= 1

        // Get Plane Normal
        Vector3 x = headTransform.position - tipTransform.position;
        Vector3 y = headTransform.position - bodyTransform.position;
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
}
