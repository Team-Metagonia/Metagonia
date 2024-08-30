using UnityEngine;

namespace Oculus.Movement.Locomotion
{
    public class SmoothRotation : MonoBehaviour
    {
        [Tooltip("The speed at which the player rotates smoothly.")]
        [SerializeField]
        private float smoothRotationSpeed = 90f; // degrees per second

        private void Update()
        {
            // 오른쪽 스틱 입력을 받습니다.
            Vector2 rotationInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            if (rotationInput.x != 0)
            {
                // 입력에 따라 회전 각도를 계산합니다.
                float rotationAmount = rotationInput.x * smoothRotationSpeed * Time.deltaTime;
                // 캐릭터를 회전시킵니다.
                transform.Rotate(0, rotationAmount, 0);
            }
        }
    }
}