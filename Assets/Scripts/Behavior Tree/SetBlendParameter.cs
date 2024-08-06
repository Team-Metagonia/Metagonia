using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetBlendParameter : Action
{
    public SharedFloat blendValue; // 블렌드 값
    private Animator animator;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate()
    {
        if (animator == null)
        {
            Debug.LogError("[SetBlendParameter] OnUpdate: Animator is null. Make sure the Animator component is attached.");
            return TaskStatus.Failure;
        }

        animator.SetFloat("Blend", blendValue.Value);
        return TaskStatus.Success;
    }
}