using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CheckState : Conditional
{
    public SharedInt currentState;
    public int targetState;

    public override TaskStatus OnUpdate()
    {
        if (currentState == null)
        {
            Debug.LogError("[CheckState] OnUpdate: currentState is null. Make sure the variable is properly set.");
            return TaskStatus.Failure;
        }

        Debug.Log($"[CheckState] OnUpdate: currentState = {currentState.Value}, targetState = {targetState}");
        
        if (currentState.Value == targetState)
        {
            Debug.Log("[CheckState] OnUpdate: State matches. Returning Success.");
            return TaskStatus.Success;
        }

        Debug.Log("[CheckState] OnUpdate: State does not match. Returning Failure.");
        return TaskStatus.Failure;
    }
}
