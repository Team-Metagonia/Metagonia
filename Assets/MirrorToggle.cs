using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorToggle : MonoBehaviour
{
    // 비활성화할 오브젝트를 드래그하여 할당
    public GameObject targetObject;
    
    void Update()
    {
        // OVRInput을 사용하여 Y 버튼 입력 감지
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            // 타겟 오브젝트 활성/비활성 토글
            if (targetObject != null)
            {
                targetObject.SetActive(!targetObject.activeSelf);
            }
        }
    }
}