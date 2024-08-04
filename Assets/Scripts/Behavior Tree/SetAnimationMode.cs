using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetAnimationMode : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject with the Animator component")]
    public SharedGameObject targetGameObject;
    
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The AnimationMode value to set")]
    public SharedInt animationMode;

    private Animator animator;

    public override void OnStart()
    {
        var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        animator = currentGameObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("[SetAnimationMode] OnStart: No Animator component found on " + currentGameObject.name);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (animator == null)
        {
            return TaskStatus.Failure;
        }

        animator.SetInteger("AnimationMode", animationMode.Value);
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        targetGameObject = null;
        animationMode = 0;
    }
}