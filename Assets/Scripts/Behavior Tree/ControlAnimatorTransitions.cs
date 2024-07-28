using UnityEngine;

public class ControlAnimatorTransitions : MonoBehaviour
{
    private Animator animator;
    private bool canTransitionToAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("[ControlAnimatorTransitions] Start: Animator is null. Make sure the Animator component is attached.");
        }
        canTransitionToAttack = true;
    }

    void Update()
    {
        if (animator != null)
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Attack 상태로 전이할 수 있는지 여부를 설정
            if (stateInfo.IsName("Idle") || stateInfo.IsName("Run") || stateInfo.IsName("Crouch"))
            {
                canTransitionToAttack = true;
            }
            else if (StateContainsAttack(stateInfo))
            {
                canTransitionToAttack = false;
            }

            // Animator 파라미터 설정
            animator.SetBool("canTransitionToAttack", canTransitionToAttack);
        }
    }

    bool StateContainsAttack(AnimatorStateInfo stateInfo)
    {
        return stateInfo.IsName("Attack");
    }
}