using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 2.0f; // 카메라 회전 속도
    public Transform cameraTransform; // 카메라 트랜스폼

    void Update()
    {
        // 왼쪽 조이스틱 입력 받기
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        // 조이스틱 입력에 따라 카메라 회전 처리
        RotateCamera(joystickInput);
    }

    void RotateCamera(Vector2 input)
    {
        // 조이스틱 입력을 기반으로 회전 계산
        float yaw = input.x * rotationSpeed;
        float pitch = input.y * rotationSpeed;

        // 현재 카메라의 회전 값을 얻어와서 적용
        cameraTransform.eulerAngles += new Vector3(-pitch, yaw, 0);
    }
}