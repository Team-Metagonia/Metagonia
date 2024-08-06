using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetLocalSpeedAction : Action
{
    public SharedFloat NewSpeed; // 새로 설정할 Speed 값

    public override void OnStart()
    {
        if (NewSpeed == null)
        {
            Debug.LogError("[SetLocalSpeedAction] OnStart: NewSpeed variable is null. Make sure the NewSpeed variable is properly set.");
            return;
        }
    }

    public override TaskStatus OnUpdate()
    {
        // Behavior Tree에서 Speed 변수 가져오기
        var speedVariable = (SharedFloat) Owner.GetVariable("Speed");

        if (speedVariable == null)
        {
            Debug.LogError("[SetLocalSpeedAction] OnUpdate: Local variable 'Speed' not found.");
            return TaskStatus.Failure;
        }

        // Speed 변수를 NewSpeed 값으로 설정
        speedVariable.Value = NewSpeed.Value;
        Debug.Log($"[SetLocalSpeedAction] OnUpdate: Setting Local Speed to {speedVariable.Value}");

        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        // 기본값으로 초기화
        NewSpeed = 0;
    }
}