using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetRandomAttackMode : Action
{
    public SharedBool isAttacking; // 공유된 bool 변수
    public SharedInt attackMode; // 공유된 int 변수
    public int numberOfAttackModes = 3; // Attack 모드의 갯수

    private Animator animator;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate()
    {
        if (animator == null)
        {
            Debug.LogError("[SetRandomAttackMode] OnUpdate: Animator is null. Make sure the Animator component is attached.");
            return TaskStatus.Failure;
        }

        if (isAttacking.Value)
        {
            // AttackMode 설정 (랜덤 값)
            int mode = Random.Range(1, numberOfAttackModes + 1);
            attackMode.Value = mode;
            animator.SetInteger("AttackMode", mode);
            Debug.Log($"[SetRandomAttackMode] Set AttackMode to {mode}");
        }

        return TaskStatus.Success;
    }
}