using UnityEngine;
using System.IO;

public class CameraCaptureWithEmpty : MonoBehaviour
{
    public GameObject rootObject; // 캡처할 루트 오브젝트
    public Camera targetCamera; // 캡처할 카메라
    public string saveFolder = "Captures"; // 저장할 폴더 이름
    public int captureWidth = 1920; // 캡처할 이미지의 너비
    public int captureHeight = 1080; // 캡처할 이미지의 높이

    void Start()
    {
        if (rootObject == null || targetCamera == null)
        {
            Debug.LogError("Root object or target camera is not assigned.");
            return;
        }

        // 캡처 디렉토리 생성
        string directoryPath = Path.Combine(Application.dataPath, saveFolder);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // 모든 자식 오브젝트 비활성화
        foreach (Transform child in rootObject.transform)
        {
            child.gameObject.SetActive(false);
        }

        // 아무것도 선택되지 않은 상태에서 캡처
        CaptureCamera("Empty", directoryPath);

        // 자식 오브젝트 하나씩 활성화하면서 캡처
        foreach (Transform child in rootObject.transform)
        {
            child.gameObject.SetActive(true);
            CaptureCamera(child.name, directoryPath);
            child.gameObject.SetActive(false);
        }

        Debug.Log("Capture completed.");
    }

    void CaptureCamera(string objectName, string directoryPath)
    {
        // 기존 해상도 저장
        int originalWidth = targetCamera.pixelWidth;
        int originalHeight = targetCamera.pixelHeight;

        // 새로운 해상도로 설정
        RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
        targetCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        
        targetCamera.Render();

        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
        screenShot.Apply();

        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // 파일 저장 경로 설정
        string filePath = Path.Combine(directoryPath, objectName + ".png");
        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
        Debug.Log($"Captured {objectName} and saved to: {filePath}");

        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif

        // 해상도 복원
        targetCamera.pixelRect = new Rect(0, 0, originalWidth, originalHeight);
    }
}
