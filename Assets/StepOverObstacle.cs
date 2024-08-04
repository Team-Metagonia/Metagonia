using UnityEngine;

public class StepOverObstacle : MonoBehaviour
{
    /// <summary>
    /// Maximum height of steps that can be stepped over.
    /// </summary>
    [Tooltip("최대 넘어갈 수 있는 계단 높이")]
    [SerializeField]
    private float stepHeight = 0.3f;

    /// <summary>
    /// Distance to check in front of the character for steps.
    /// </summary>
    [Tooltip("계단을 확인할 앞쪽 거리")]
    [SerializeField]
    private float stepCheckDistance = 0.5f;

    /// <summary>
    /// Speed to step over the obstacle.
    /// </summary>
    [Tooltip("장애물을 넘는 속도")]
    [SerializeField]
    private float stepOverSpeed = 2.0f;

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
    private Vector3 _initialPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (_isSteppingOver)
        {
            Debug.Log("Stepping");
            StepOver();
        }
        else
        {
            CheckForStep();
        }
    }

    private void CheckForStep()
    {
        Vector3 originLow = transform.position + Vector3.up * 0.1f;
        Vector3 originHigh = transform.position + Vector3.up * (0.1f + stepHeight);
        Vector3 direction = transform.forward;

        if (Physics.Raycast(originLow, direction, out RaycastHit hitLow, stepCheckDistance))
        {
            Debug.Log("stepping low");
            Physics.Raycast(originHigh, direction, out RaycastHit hitHigh, stepCheckDistance);

            // 첫 번째 레이캐스트는 충돌하고, 두 번째 레이캐스트가 충돌하지 않았거나 두 레이캐스트가 다른 콜라이더와 충돌한 경우
            if (hitLow.collider != null && (hitHigh.collider == null || hitLow.collider != hitHigh.collider))
            {
                Debug.Log("stepping diff");
                Vector2 leftStickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                if (leftStickInput.y > 0) // OVR 왼쪽 스틱 입력이 앞으로 이동할 때만 계단을 넘음
                {
                    Debug.Log("stepping dirc");
                    _isSteppingOver = true;
                    _initialPosition = transform.position;
                    _stepTargetPosition = new Vector3(transform.position.x, hitLow.point.y + _collider.bounds.extents.y, transform.position.z) + direction * stepCheckDistance;
                }
            }
        }
    }

    private void StepOver()
    {
        Vector2 leftStickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 moveDirection = (_stepTargetPosition - transform.position).normalized;

        // 스틱 입력 방향과 이동 방향을 비교
        Vector3 stickDirection = new Vector3(leftStickInput.x, 0, leftStickInput.y).normalized;
        if (Vector3.Dot(stickDirection, moveDirection) < 0.5f) // 방향이 크게 다른 경우 스텝오버 해제
        {
            _isSteppingOver = false;
            _rigidbody.velocity = Vector3.zero; // 이동을 멈춤
            return;
        }

        Vector3 newPosition = Vector3.MoveTowards(transform.position, _stepTargetPosition, stepOverSpeed * Time.deltaTime);
        moveDirection = (newPosition - transform.position).normalized;
        _rigidbody.velocity = moveDirection * stepOverSpeed;

        // 목표 위치보다 트랜스폼이 0.01 위에 있으면 스텝오버 종료
        if (Vector3.Distance(transform.position, _stepTargetPosition) < 0.01f)
        {
            _isSteppingOver = false;
            _rigidbody.velocity = Vector3.zero; // 이동 완료 후 속도를 0으로 설정
        }
    }
}
