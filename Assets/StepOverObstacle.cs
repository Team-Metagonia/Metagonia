using UnityEngine;

public class StepOverObstacle : MonoBehaviour
{
    [Tooltip("The maximum height of the obstacle the player can step over")]
    [SerializeField]
    private float maxStepHeight = 0.5f;

    [Tooltip("The speed at which the player will step over the obstacle")]
    [SerializeField]
    private float stepSpeed = 2.0f;

    [Tooltip("The layer mask to specify which layers to detect")]
    [SerializeField]
    private LayerMask terrainLayerMask;

    [Tooltip("The layer mask to specify which layers to exclude")]
    [SerializeField]
    private LayerMask playerLayerMask;

    private Rigidbody rigidBody;
    private bool isSteppingOver = false;
    private Vector3 targetPosition;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody component is missing.");
        }
    }

    void FixedUpdate()
    {
        if (isSteppingOver)
        {
            StepOver();
        }
        else
        {
            CheckStep();
        }
    }

    private void CheckStep()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        Debug.DrawRay(origin, transform.forward * 0.5f, Color.red); // Ray를 시각적으로 확인

        // Combine both terrain and player layers to ignore in the raycast
        LayerMask combinedMask = terrainLayerMask | playerLayerMask;

        // Raycast with layer mask that ignores both terrain and player layers
        if (Physics.Raycast(origin, transform.forward, out hit, 0.5f, ~combinedMask))
        {
            Debug.Log($"Hit: {hit.collider.name}, Height: {hit.collider.bounds.max.y}, Player Y: {transform.position.y}");

            float obstacleHeight = hit.collider.bounds.max.y - transform.position.y;
            Debug.Log($"Obstacle Height: {obstacleHeight}");

            if (obstacleHeight <= maxStepHeight)
            {
                Vector3 stepOrigin = new Vector3(hit.point.x, hit.collider.bounds.max.y + 0.1f, hit.point.z);
                Debug.DrawRay(stepOrigin, Vector3.down * 0.2f, Color.green); // Step을 시각적으로 확인

                if (Physics.Raycast(stepOrigin, Vector3.down, out RaycastHit stepHit, 0.2f, ~combinedMask))
                {
                    Debug.Log($"Step Hit: {stepHit.collider.name}, Step Point Y: {stepHit.point.y}");
                    isSteppingOver = true;
                    targetPosition = new Vector3(transform.position.x, stepHit.point.y, transform.position.z);
                }
            }
        }
    }

    private void StepOver()
    {
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, stepSpeed * Time.fixedDeltaTime);
        rigidBody.MovePosition(new Vector3(currentPosition.x, newPosition.y, currentPosition.z));

        Debug.Log($"Moving to: {targetPosition}, Current Position: {currentPosition}, New Position: {newPosition}");

        if (Mathf.Abs(currentPosition.y - targetPosition.y) < 0.05f)
        {
            isSteppingOver = false;
        }
    }
}
