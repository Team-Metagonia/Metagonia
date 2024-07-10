using UnityEngine;

public class JoystickControlledMovement : MonoBehaviour
{
    [Tooltip("Speed of the movement.")]
    [SerializeField]
    private float moveSpeed = 3.0f;

    [Tooltip("Speed of the rotation.")]
    [SerializeField]
    private float rotationSpeed = 100.0f;

    private void Update()
    {
        // Get left joystick input for movement
        Vector2 movementInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        
        // Calculate movement direction based on joystick input
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);

        // Move the object
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Get right joystick input for rotation
        Vector2 rotationInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // Rotate the object around the y-axis based on the horizontal input of the right joystick
        transform.Rotate(0, rotationInput.x * rotationSpeed * Time.deltaTime, 0);
    }
}