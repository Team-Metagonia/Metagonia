using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetAnimatorParameters : Action
{
    public SharedBool isIdle; // 공유된 bool 변수
    public SharedBool isAttacking; // 공유된 bool 변수
    public SharedBool isCrouching; // 공유된 bool 변수
    public SharedBool isRunning; // 공유된 bool 변수

    private Animator animator;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        if (animator == null)
        {
            Debug.LogError("[SetAnimatorParameters] OnStart: Animator is null. Make sure the Animator component is attached.");
            return;
        }

        // IsIdle 설정
        animator.SetBool("IsIdle", isIdle.Value);
        Debug.Log($"[SetAnimatorParameters] Set IsIdle to {isIdle.Value}");

        // IsAttacking 설정
        animator.SetBool("IsAttacking", isAttacking.Value);
        Debug.Log($"[SetAnimatorParameters] Set IsAttacking to {isAttacking.Value}");

        // IsCrouching 설정
        animator.SetBool("IsCrouching", isCrouching.Value);
        Debug.Log($"[SetAnimatorParameters] Set IsCrouching to {isCrouching.Value}");

        // IsRunning 설정
        animator.SetBool("IsRunning", isRunning.Value);
        Debug.Log($"[SetAnimatorParameters] Set IsRunning to {isRunning.Value}");
    }

    public override TaskStatus OnUpdate()
    {
        // 이 태스크는 한 번만 실행되도록 설계되었습니다.
        return TaskStatus.Success;
    }
}