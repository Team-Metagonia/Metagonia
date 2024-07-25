using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CheckState : Conditional
{
    public int targetState;

    public override TaskStatus OnUpdate()
    {
        // 글로벌 변수에서 currentState 값을 가져옴
        var globalCurrentState = (SharedInt)GlobalVariables.Instance.GetVariable("currentState");

        if (globalCurrentState == null)
        {
            Debug.LogError("[CheckState] OnUpdate: globalCurrentState is null. Make sure the variable is properly set.");
            return TaskStatus.Failure;
        }

        Debug.Log($"[CheckState] OnUpdate: globalCurrentState = {globalCurrentState.Value}, targetState = {targetState}");
        
        if (globalCurrentState.Value == targetState)
        {
            Debug.Log("[CheckState] OnUpdate: State matches. Returning Success.");
            return TaskStatus.Success;
        }

        Debug.Log("[CheckState] OnUpdate: State does not match. Returning Failure.");
        return TaskStatus.Failure;
    }
}