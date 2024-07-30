using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetIsATRunning : Action
{
    public SharedBool isATRunning; // 공유된 bool 변수

    private Animator animator;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        if (animator == null)
        {
            Debug.LogError("[SetIsATRunning] OnStart: Animator가 없습니다. Animator 컴포넌트를 확인하세요.");
            return;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (animator != null)
        {
            animator.SetBool("IsATRunning", isATRunning.Value);
            Debug.Log($"[SetIsATRunning] OnUpdate: IsATRunning을 {isATRunning.Value}로 설정합니다.");
        }

        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        // 기본값으로 초기화
        isATRunning = false;
    }
}