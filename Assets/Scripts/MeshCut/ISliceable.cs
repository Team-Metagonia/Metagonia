using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISliceable
{
    void ProcessSlice(HitInfo hitInfo);
}