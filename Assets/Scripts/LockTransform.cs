using UnityEngine;

public class LockLocalTransform : MonoBehaviour
{
    void Update()
    {
        // 로컬 위치를 (0, 0, 0)으로 고정
        transform.localPosition = Vector3.zero;

        // 로컬 회전을 (0, 0, 0)으로 고정
        transform.localRotation = Quaternion.identity;

        // 로컬 스케일을 (1, 1, 1)로 고정
        transform.localScale = Vector3.one;
    }
}