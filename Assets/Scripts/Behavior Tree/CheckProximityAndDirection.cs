using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckProximityAndDirection : Conditional
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The target object to check distance and direction against")]
    public SharedGameObject targetObject;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum allowed distance to the target object")]
    public SharedFloat maxDistance;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("The maximum allowed angle to the target object")]
    public SharedFloat maxAngle;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("If true, check if the target is NOT within the distance and angle")]
    public SharedBool checkInverted;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("The update interval in seconds")]
    public SharedFloat updateInterval = 1f;

    private Transform targetTransform;
    private float nextUpdateTime;

    public override void OnAwake()
    {
        targetTransform = targetObject.Value != null ? targetObject.Value.transform : null;
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time < nextUpdateTime)
        {
            return TaskStatus.Running;
        }

        nextUpdateTime = Time.time + updateInterval.Value;

        if (targetTransform == null)
        {
            Debug.LogError("[CheckProximityAndDirection] OnUpdate: Target object is not set.");
            return TaskStatus.Failure;
        }

        Vector3 directionToTarget = targetTransform.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        bool isWithinDistance = distanceToTarget <= maxDistance.Value;
        bool isWithinAngle = angleToTarget <= maxAngle.Value;

        if (checkInverted.Value)
        {
            // Inverted check: return success if target is NOT within distance and angle
            if (!isWithinDistance || !isWithinAngle)
            {
                Debug.Log($"[CheckProximityAndDirection] OnUpdate: Target is not within distance or angle (Inverted Check). Distance: {distanceToTarget}, Angle: {angleToTarget}");
                return TaskStatus.Success;
            }
        }
        else
        {
            // Normal check: return success if target is within distance and angle
            if (isWithinDistance && isWithinAngle)
            {
                Debug.Log($"[CheckProximityAndDirection] OnUpdate: Target is within distance and angle. Distance: {distanceToTarget}, Angle: {angleToTarget}");
                return TaskStatus.Success;
            }
        }

        Debug.Log($"[CheckProximityAndDirection] OnUpdate: Condition not met. Distance: {distanceToTarget}, Angle: {angleToTarget}");
        return TaskStatus.Failure;
    }

    public override void OnReset()
    {
        targetObject = null;
        maxDistance = 0;
        maxAngle = 0;
        checkInverted = false;
        updateInterval = 1f;
    }
}
