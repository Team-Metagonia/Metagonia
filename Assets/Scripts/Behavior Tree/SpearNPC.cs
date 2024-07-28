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
        // 공격 가능 여부 로직
        // 예: 적이 일정 거리에 있고 공격할 수 있는 상태인지 확인
        return true;
    }

    public void Attack(Vector3 targetPosition)
    {
        // 공격 애니메이터 업데이트
        UpdateAnimator();

        // 공격 로직 구현
        Debug.Log("Attacking position: " + targetPosition);
        // 여기에 공격 애니메이션, 효과 등을 추가
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            // 이전에 실행 중이던 Coroutine을 중지
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
            }

            // AnimationMode를 2로 설정
            animator.SetInteger("AnimationMode", 2);

            // 새로운 Coroutine 시작
            resetCoroutine = StartCoroutine(ResetAnimationMode());
        }
    }

    private IEnumerator ResetAnimationMode()
    {
        // 0.1초 대기
        yield return new WaitForSeconds(0.1f);

        if (animator != null)
        {
            // AnimationMode를 0으로 설정
            animator.SetInteger("AnimationMode", 0);
        }
    }
}