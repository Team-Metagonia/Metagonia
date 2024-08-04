using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CheckIsStateChanged : Conditional
{
    public override TaskStatus OnUpdate()
    {
        // 글로벌 변수에서 IsStateChanged 값을 가져옴
        var isStateChanged = (SharedBool)GlobalVariables.Instance.GetVariable("IsStateChanged");

        if (isStateChanged == null)
        {
            Debug.LogError("[CheckIsStateChanged] OnUpdate: isStateChanged is null. Make sure the variable is properly set.");
            return TaskStatus.Failure;
        }

        Debug.Log($"[CheckIsStateChanged] OnUpdate: isStateChanged = {isStateChanged.Value}");
        
        if (isStateChanged.Value)
        {
            // 상태를 false로 변경
            isStateChanged.Value = false;
            Debug.Log("[CheckIsStateChanged] OnUpdate: StateChanged is true. Setting to false and returning Success.");
            return TaskStatus.Success;
        }

        Debug.Log("[CheckIsStateChanged] OnUpdate: StateChanged is false. Returning Failure.");
        return TaskStatus.Failure;
    }
}