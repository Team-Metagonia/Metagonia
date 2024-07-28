using UnityEngine;

public class RunSpeedAnimator : MonoBehaviour
{
    public Animator animator;
    public float maxSpeed = 10.0f; // 최대 속도
    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        // 현재 위치와 이전 위치의 차이로 이동 속도 계산
        float distance = Vector3.Distance(transform.position, previousPosition);
        
        // 회전으로 인한 속도 계산
        float rotationSpeed = Mathf.Abs(Vector3.SignedAngle(transform.forward, (transform.position - previousPosition).normalized, Vector3.up) / Time.deltaTime);

        // 이동 속도와 회전 속도를 합산
        float currentSpeed = distance / Time.deltaTime + rotationSpeed;

        // 속도를 0에서 1 사이로 조절
        float normalizedSpeed = Mathf.Clamp(currentSpeed / maxSpeed, 0, 1);

        // 애니메이터의 Speed 파라미터 업데이트
        animator.SetFloat("Speed", normalizedSpeed);

        // 현재 위치를 이전 위치로 저장
        previousPosition = transform.position;
    }
}