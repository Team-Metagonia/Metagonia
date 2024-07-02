using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftStickMovement : MonoBehaviour
{
    public float speed = 3.0f;

    void Update()
    {
        // Get the input from the left thumbstick
        Vector2 leftThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Calculate movement vector based on thumbstick input
        Vector3 movement = new Vector3(leftThumbstick.x, 0, leftThumbstick.y) * speed * Time.deltaTime;

        // Apply movement to the object's position
        transform.Translate(movement, Space.World);
    }
}
