using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // 카메라의 Transform을 참조합니다.
    public Vector3 offset = new Vector3(0, -0.5f, 1); // 패널이 카메라의 앞에 위치할 오프셋을 설정합니다.

    void Update()
    {
        // 카메라의 전방에 패널을 위치시킵니다.
        transform.position = cameraTransform.position + cameraTransform.forward * offset.z + cameraTransform.up * offset.y;
        transform.LookAt(cameraTransform); // 패널이 항상 카메라를 바라보게 합니다.
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // 패널이 수평으로 고정되도록 합니다.
    }
}