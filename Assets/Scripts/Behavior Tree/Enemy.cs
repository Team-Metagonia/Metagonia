using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class Enemy : MonoBehaviour, IDamageable
{
    public float health = 100f;

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
        Destroy(gameObject);
    }
}
