using UnityEngine;
using TMPro; // TMP 네임스페이스 추가

public class ConstructionModeManager : MonoBehaviour
{
    public TextMeshProUGUI modeText; // TMP 텍스트 (건축 모드 표시)

    private bool isConstructionMode = false;

    void Update()
    {
        // 좌측 컨트롤러의 IndexTrigger 입력 감지
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            isConstructionMode = !isConstructionMode;
            UpdateModeText(); // 모드 텍스트 업데이트
        }
    }

    void UpdateModeText()
    {
        if (modeText != null)
        {
            modeText.text = isConstructionMode ? "Construction Mode" : "Normal Mode";
            modeText.gameObject.SetActive(isConstructionMode); // 건축 모드일 때만 텍스트 활성화
        }
    }

    public bool IsConstructionMode()
    {
        return isConstructionMode;
    }
}