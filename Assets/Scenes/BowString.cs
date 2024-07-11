using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unity Tutorial Working with Joints & Springs to Create Physical Cables
// https://www.youtube.com/watch?v=6CTKJ8Y43VU

[RequireComponent(typeof(LineRenderer))]
public class BowString : MonoBehaviour
{
    public Transform upperAnchorLocation;
    public Transform lowerAnchorLocation;
    private LineRenderer line;

    [Range(0, 1)] private float lineWidth = 0.01f;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 3;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
    }

    private void Update()
    {
        line.SetPosition(0, upperAnchorLocation.position);
        line.SetPosition(1, this.transform.position);
        line.SetPosition(2, lowerAnchorLocation.position);
    }
}
