using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;
using System.Collections;

public class SpearNPC : MonoBehaviour, IAttackAgent
{
    public float attackRange = 10f;
    public float attackAngle = 45f;
    private Animator animator;
    public bool isSelected = false;
    private Coroutine resetCoroutine;
    private bool canToggle = true; // 플래그 추가

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No Animator component found on " + gameObject.name);
        }
    }

    public float AttackDistance()
    {
        return attackRange;
    }

    public float AttackAngle()
    {
        return attackAngle;
    }
    
    public bool CanAttack()
    {
        return true;
    }

    public void Attack(Vector3 targetPosition)
    {
        UpdateAnimator();
        Debug.Log("Attacking position: " + targetPosition);
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
            }
            animator.SetInteger("AnimationMode", 2);
            resetCoroutine = StartCoroutine(ResetAnimationMode());
        }
    }

    private IEnumerator ResetAnimationMode()
    {
        yield return new WaitForSeconds(0.1f);
        if (animator != null)
        {
            animator.SetInteger("AnimationMode", 0);
        }
    }
    
    public void ToggleSelection()
    {
        if (canToggle)
        {
            isSelected = !isSelected;
            Debug.Log($"isSelected toggled for: {gameObject.name} to {isSelected}");

            if (isSelected)
            {
                ChangeAnimatorParameterTemporarily();
            }

            // 쿨다운 시작
            StartCoroutine(ToggleCooldown());
        }
    }

    private void ChangeAnimatorParameterTemporarily()
    {
        if (animator != null)
        {
            StartCoroutine(ChangeParameterCoroutine());
        }
    }

    private IEnumerator ChangeParameterCoroutine()
    {
        animator.SetInteger("AnimationMode", 5);
        yield return new WaitForSeconds(0.1f);
        animator.SetInteger("AnimationMode", 1);
    }

    private IEnumerator ToggleCooldown()
    {
        canToggle = false;
        yield return new WaitForSeconds(0.1f);
        canToggle = true;
    }
}
