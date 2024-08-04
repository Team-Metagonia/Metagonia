using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetMoveMode : Action
{
    private Animator animator;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        if (animator == null)
        {
            Debug.LogError("[SetMoveMode] OnStart: Animator is null. Make sure the Animator component is attached.");
            return;
        }
    }

    public override TaskStatus OnUpdate()
    {
        bool animatorIsRunning = animator.GetBool("IsRunning");
        bool animatorIsCrouching = animator.GetBool("IsCrouching");

        if (animatorIsRunning || animatorIsCrouching)
        {
            // 둘 중 하나라도 켜져 있으면 IsIdle과 IsAttacking을 끔
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsAttacking", false);
            Debug.Log("[SetMoveMode] OnUpdate: Animator state is Running or Crouching. Set IsIdle and IsAttacking to false.");
        }
        else
        {
            // 둘 다 꺼져 있으면 IsRunning을 켜고 나머지를 끔
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsCrouching", false);
            Debug.Log("[SetMoveMode] OnUpdate: Both IsRunning and IsCrouching are false, setting IsRunning to true and others to false.");
        }

        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        // 기본값으로 초기화
        // 필요 시 구현할 수 있습니다.
    }
}