using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInfo
{
    public Vector3 hitPosition;
    public Vector3 hitNormal;
    public Vector3 planeNormal;

    public HitInfo()
    {
        this.hitPosition = Vector3.zero;
        this.hitNormal = Vector3.zero;
        this.planeNormal = Vector3.zero;
    }

    public HitInfo(Vector3 hitPosition, Vector3 hitNormal, Vector3 planeNormal)
    {
        this.hitPosition = hitPosition;
        this.hitNormal = hitNormal;
        this.planeNormal = planeNormal;
    }
}

