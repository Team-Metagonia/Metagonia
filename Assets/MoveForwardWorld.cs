using UnityEngine;

public class MoveForwardWorld : MonoBehaviour
{
    [Tooltip("The speed at which the object moves forward along the world Z axis.")]
    [SerializeField]
    private float moveSpeed = 5.0f; // meters per second

    void FixedUpdate()
    {
        // Calculate the new position
        Vector3 newPosition = transform.position + Vector3.forward * moveSpeed * Time.deltaTime;

        // Update the object's position
        transform.position = newPosition;
    }
}