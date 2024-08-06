using UnityEngine;
using TMPro; // TMP 네임스페이스 추가

public class SpawnCubeOnTerrain : MonoBehaviour
{
    public GameObject cubePrefab; // 생성할 큐브 프리팹
    public GameObject previewCubePrefab; // 투명한 프리뷰 큐브 프리팹
    public Terrain terrain; // 지형
    public LayerMask terrainLayer; // 지형 레이어
    public LayerMask cubeLayer; // 큐브 레이어
    public TextMeshProUGUI modeText; // TMP 텍스트 (건축 모드 표시)

    private GameObject previewCube;
    private bool isConstructionMode = false;
    private float cubeSize = 1.0f; // 큐브 크기 (기본값 1.0)

    void Start()
    {
        // 투명한 프리뷰 큐브 생성
        previewCube = Instantiate(previewCubePrefab);
        previewCube.SetActive(false); // 처음에는 비활성화
        UpdateModeText(); // 초기 상태 업데이트
    }

    void Update()
    {
        // 좌측 컨트롤러의 IndexTrigger 입력 감지
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            isConstructionMode = !isConstructionMode;
            UpdateModeText(); // 모드 텍스트 업데이트
        }

        if (isConstructionMode)
        {
            // 건축 모드일 때만 큐브 생성 및 프리뷰 업데이트
            HandleCubePreviewAndCreation();
        }
        else
        {
            // 건축 모드가 아닐 때 프리뷰 큐브 비활성화
            previewCube.SetActive(false);
        }
    }

    void HandleCubePreviewAndCreation()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer))
        {
            Vector3 hitPoint = hit.point;
            
            // 지형의 표면 위에서 프리뷰 큐브의 Y 좌표를 조정
            previewCube.SetActive(true);
            previewCube.transform.position = new Vector3(hitPoint.x, hitPoint.y + (cubeSize / 2), hitPoint.z);
            
            // 큐브가 이미 생성된 위치와 겹치지 않도록 확인
            Collider[] colliders = Physics.OverlapBox(previewCube.transform.position, new Vector3(cubeSize / 2, cubeSize / 2, cubeSize / 2), Quaternion.identity, cubeLayer);
            if (colliders.Length == 0)
            {
                previewCube.SetActive(true);
            }
            else
            {
                previewCube.SetActive(false);
            }

            // 우측 컨트롤러의 IndexTrigger 입력 감지
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && previewCube.activeSelf)
            {
                TrySpawnCube(hitPoint);
            }
        }
        else
        {
            previewCube.SetActive(false);
        }
    }

    void TrySpawnCube(Vector3 hitPoint)
    {
        // 지형의 표면 위에서 큐브의 Y 좌표를 조정
        Vector3 spawnPosition = new Vector3(hitPoint.x, hitPoint.y + (cubeSize / 2), hitPoint.z);
        Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
    }

    void UpdateModeText()
    {
        if (modeText != null)
        {
            modeText.text = isConstructionMode ? "Construction Mode" : "Normal Mode";
            modeText.gameObject.SetActive(isConstructionMode); // 건축 모드일 때만 텍스트 활성화
        }
    }
}