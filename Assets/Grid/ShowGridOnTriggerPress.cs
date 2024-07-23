using UnityEngine;

public class ShowGridOnTriggerPress : MonoBehaviour
{
    public GameObject gridObject; // 표시할 그리드 오브젝트

    void Update()
    {
        // 좌측 컨트롤러의 IndexTrigger 입력 감지
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            ToggleGridVisibility(); // 그리드 표시/숨김 토글 함수 호출
        }
    }

    void ToggleGridVisibility()
    {
        // 그리드 오브젝트의 활성화/비활성화를 토글
        gridObject.SetActive(!gridObject.activeSelf);
    }
}