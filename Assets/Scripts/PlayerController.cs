using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();

        // 초기 속도와 회전 설정
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 초기 위치 설정
        transform.position = new Vector3(0f, -0.85f, 0f); // 예시로 (0, 1, 0) 위치로 설정
    }
}