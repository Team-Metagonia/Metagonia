using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    float Health { get; }
    void TakeDamage(DamageInfo damageInfo);
    void Die(HitInfo hitInfo);
}
