using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3.0f; // 이동 속도
    public Transform playerCamera; // 플레이어의 카메라
    private Vector2 inputAxis;

    void Update()
    {
        // 조이스틱 입력 받기
        inputAxis = new Vector2(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x,
            OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).y);

        // 이동 처리
        MovePlayer();
    }

    void MovePlayer()
    {
        if (inputAxis.sqrMagnitude > 0.01f)
        {
            // 입력 방향 벡터 생성
            Vector3 moveDirection = new Vector3(inputAxis.x, 0, inputAxis.y);
            moveDirection = playerCamera.TransformDirection(moveDirection);
            moveDirection.y = 0; // Y 방향 이동 없음

            // 이동 처리
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}