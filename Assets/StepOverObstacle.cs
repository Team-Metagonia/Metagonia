using UnityEngine;

public class StepOverObstacle : MonoBehaviour
{
    /// <summary>
    /// Maximum height of steps that can be stepped over.
    /// </summary>
    [Tooltip("최대 넘어갈 수 있는 계단 높이")]
    [SerializeField]
    private float stepHeight = 0.5f;

    /// <summary>
    /// Distance to check in front of the character for steps.
    /// </summary>
    [Tooltip("계단을 확인할 앞쪽 거리")]
    [SerializeField]
    private float stepCheckDistance = 0.3f;

    /// <summary>
    /// Speed to step over the obstacle.
    /// </summary>
    [Tooltip("장애물을 넘는 속도")]
    [SerializeField]
    private float stepOverSpeed = 2.0f;

    /// <summary>
    /// Starting height of the low raycast.
    /// </summary>
    [Tooltip("Low 레이캐스트 시작 높이")]
    [SerializeField]
    private float lowRaycastHeight = 0.01f;

    /// <summary>
    /// LayerMask to specify which layers the raycasts should not collide with.
    /// </summary>
    [Tooltip("레이캐스트가 충돌하지 않을 레이어")]
    [SerializeField]
    private LayerMask excludeLayerMask;

    /// <summary>
    /// Reference to the Rigidbody component.
    /// </summary>
    private Rigidbody _rigidbody;

    /// <summary>
    /// Reference to the Collider component.
    /// </summary>
    private Collider _collider;

    private bool _isSteppingOver;
    private Vector3 _stepTargetPosition;
    private Collider _initialStepCollider;
    private Vector3 _stepDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (_isSteppingOver)
        {
            StepOver();
        }
        else
        {
            CheckForStep();
        }
    }

    private void CheckForStep()
    {
        Vector3[] directions = Get8Directions();
        Vector3 originLow = transform.position + Vector3.up * lowRaycastHeight;
        Vector3 originHigh = transform.position + Vector3.up * (lowRaycastHeight + stepHeight);
        int layerMask = ~excludeLayerMask; // excludeLayerMask에서 제외한 레이어들만 포함

        foreach (var direction in directions)
        {
            bool lowHit = Physics.Raycast(originLow, direction, out RaycastHit hitLow, stepCheckDistance, layerMask);
            bool highHit = Physics.Raycast(originHigh, direction, out RaycastHit hitHigh, stepCheckDistance, layerMask);

            Debug.Log("Low Raycast hit at position: " + (lowHit ? hitLow.point.ToString() : "no hit") + " with collider: " + (lowHit ? hitLow.collider.name : "null"));
            Debug.Log("High Raycast hit at position: " + (highHit ? hitHigh.point.ToString() : "no hit") + " with collider: " + (highHit ? hitHigh.collider.name : "null"));

            // 첫 번째 레이캐스트는 충돌하고, 두 번째 레이캐스트가 충돌하지 않았거나 두 레이캐스트가 다른 콜라이더와 충돌한 경우
            if (lowHit && (!highHit || hitLow.collider != hitHigh.collider))
            {
                Debug.Log("Low and High Raycast hit different colliders or High Raycast hit nothing. Low hit: " + hitLow.collider.name + ", High hit: " + (highHit ? hitHigh.collider.name : "null"));
                Vector2 leftStickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                if (leftStickInput != Vector2.zero) // OVR 왼쪽 스틱 입력이 있을 때만 계단을 넘음
                {
                    Debug.Log("Initiating step over. Target position: " + _stepTargetPosition);
                    _isSteppingOver = true;
                    _stepTargetPosition = new Vector3(transform.position.x, hitLow.point.y + _collider.bounds.extents.y, transform.position.z) + direction * stepCheckDistance;
                    _initialStepCollider = hitLow.collider;
                    _stepDirection = direction;
                    return; // 하나라도 조건에 맞으면 즉시 반환
                }
            }
        }
    }

    private void StepOver()
    {
        Vector2 leftStickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 direction = GetDirectionFromInput(leftStickInput);

        // 조이스틱 입력이 현재 스텝오버 방향과 맞지 않으면 스텝오버 취소
        if (direction == Vector3.zero || Vector3.Dot(direction, _stepDirection) < 0.5f)
        {
            _isSteppingOver = false;
            _rigidbody.velocity = Vector3.zero;
            Debug.Log("Stepping over canceled due to stick input direction change.");
            return;
        }

        Vector3 originLow = transform.position + Vector3.up * lowRaycastHeight;
        int layerMask = ~excludeLayerMask; // excludeLayerMask에서 제외한 레이어들만 포함

        bool currentLowHit = Physics.Raycast(originLow, _stepDirection, out RaycastHit hitLow, stepCheckDistance, layerMask);

        if (!currentLowHit || hitLow.collider == null || hitLow.collider != _initialStepCollider)
        {
            _isSteppingOver = false;
            _rigidbody.velocity = Vector3.zero; // 이동을 멈춤
            Debug.Log("Stepping over canceled due to target collider change or null. Initial collider: " + (_initialStepCollider != null ? _initialStepCollider.name : "null") + ", Current collider: " + (currentLowHit ? (hitLow.collider != null ? hitLow.collider.name : "null") : "no hit"));
            return;
        }

        Vector3 newPosition = Vector3.MoveTowards(transform.position, _stepTargetPosition, stepOverSpeed * Time.deltaTime);
        Vector3 moveDirection = (newPosition - transform.position).normalized;
        _rigidbody.velocity = moveDirection * stepOverSpeed;
    }

    private Vector3[] Get8Directions()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 left = -right;
        Vector3 back = -forward;

        return new Vector3[]
        {
            forward,
            back,
            right,
            left,
            (forward + right).normalized,
            (forward + left).normalized,
            (back + right).normalized,
            (back + left).normalized
        };
    }

    private Vector3 GetDirectionFromInput(Vector2 input)
    {
        Vector3 direction = Vector3.zero;

        if (input.y > 0) direction += transform.forward;
        if (input.y < 0) direction += -transform.forward;
        if (input.x < 0) direction += -transform.right;
        if (input.x > 0) direction += transform.right;

        return direction.normalized;
    }
}
