using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class Ally : MonoBehaviour, IAttackAgent
{
    public float attackRange = 10f;
    public float attackAngle = 45f;

    public float AttackDistance()
    {
        return attackRange;
    }

    public bool CanAttack()
    {
        // 공격 가능 여부 로직
        // 예: 적이 일정 거리에 있고 공격할 수 있는 상태인지 확인
        return true;
    }

    public float AttackAngle()
    {
        return attackAngle;
    }

    public void Attack(Vector3 targetPosition)
    {
        // 공격 로직 구현
        Debug.Log("Attacking position: " + targetPosition);
        // 여기에 공격 애니메이션, 효과 등을 추가
    }
}