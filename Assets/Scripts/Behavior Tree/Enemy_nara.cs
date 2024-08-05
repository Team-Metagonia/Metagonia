using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;
using BehaviorDesigner.Runtime.Tasks.Movement; // Evade_nara 클래스가 정의된 네임스페이스 추가

public class Enemy_nara : MonoBehaviour, IDamageable
{
    public float health = 100f;
    private float damagePerSecond = 3f; // 초당 피해량
    private Animator animator;
    private bool isDead = false;
    private Evade_nara evadeScript;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        evadeScript = GetComponent<Evade_nara>();
    }

    private void Update()
    {
        if (!isDead)
        {
            ApplyContinuousDamage();
        }
        else
        {
            if (evadeScript != null)
            {
                evadeScript.SetIsDead(true); // 사망 시 즉시 이동 중지 호출
            }
        }
    }

    private void ApplyContinuousDamage()
    {
        Damage(damagePerSecond * Time.deltaTime);
    }

    public void Damage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    public bool IsAlive()
    {
        return health > 0f;
    }

    private void Die()
    {
        // 사망 처리 로직
        isDead = true;
        animator.SetBool("isDead", true);
        if (evadeScript != null)
        {
            evadeScript.SetIsDead(true); // SetIsDead 호출
        }
        Destroy(gameObject, 5f);
    }
}