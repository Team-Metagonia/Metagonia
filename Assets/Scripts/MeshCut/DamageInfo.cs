using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
    public float damage;
    public HitInfo hitInfo;

    public DamageInfo()
    {
        this.damage = 0;
        this.hitInfo = new HitInfo();
    }

    public DamageInfo(float damage, HitInfo hitInfo)
    {
        this.damage = damage;
        this.hitInfo = hitInfo;
    }
}

