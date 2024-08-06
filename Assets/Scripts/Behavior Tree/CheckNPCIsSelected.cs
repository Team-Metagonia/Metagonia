using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckNPCIsSelected : Conditional
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject with the SpearNPC component")]
    public SharedGameObject targetGameObject;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The desired isSelected state")]
    public bool desiredState;

    private SpearNPC spearNPC;

    public override void OnStart()
    {
        if (targetGameObject.Value != null)
        {
            spearNPC = targetGameObject.Value.GetComponent<SpearNPC>();
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (spearNPC == null)
        {
            Debug.LogWarning("SpearNPC component is missing");
            return TaskStatus.Failure;
        }

        if (spearNPC.isSelected == desiredState)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    public override void OnReset()
    {
        targetGameObject = null;
        desiredState = false;
    }
}