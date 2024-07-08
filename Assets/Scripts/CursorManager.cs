using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // 게임 시작 시 커서가 보이도록 설정
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}