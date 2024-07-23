using UnityEngine;

public class GridToggle : MonoBehaviour
{
    public GameObject grid; // 그리드 오브젝트
    public OVRInput.RawButton toggleButton = OVRInput.RawButton.LIndexTrigger; // 좌측 트리거 버튼

    private bool isGridVisible = false;

    void Update()
    {
        if (OVRInput.GetDown(toggleButton))
        {
            isGridVisible = !isGridVisible;
            grid.SetActive(isGridVisible);
        }
    }
}
