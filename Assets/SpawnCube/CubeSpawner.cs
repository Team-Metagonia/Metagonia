using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab; // 생성할 큐브 프리팹
    public GameObject previewCubePrefab; // 투명한 프리뷰 큐브 프리팹
    public GameObject rockPrefab; // 빈 공간을 채울 돌 프리팹
    public LayerMask terrainLayer; // 지형 레이어
    public LayerMask cubeLayer; // 큐브 레이어
    public ConstructionModeManager modeManager; // 건축 모드 관리 스크립트

    private GameObject previewCube;
    private float cubeSize = 1.0f; // 큐브 크기 (기본값 1.0)

    void Start()
    {
        // 투명한 프리뷰 큐브 생성
        previewCube = Instantiate(previewCubePrefab);
        previewCube.SetActive(false); // 처음에는 비활성화
    }

    void Update()
    {
        if (modeManager.IsConstructionMode())
        {
            // 건축 모드일 때만 큐브 생성 및 프리뷰 업데이트
            HandleCubePreviewAndCreation();
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && previewCube.activeSelf)
            {
                // 프리뷰 큐브가 활성화된 상태에서만 큐브 생성
                TrySpawnCube(previewCube.transform.position);
            }
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

            // 프리뷰 큐브의 위치를 조정하여 지형 위에 위치하도록 함
            Vector3 previewPosition = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
            AdjustPositionToTerrain(ref previewPosition, cubeSize);

            // 큐브와 겹치는지 여부 확인
            bool isOverlap = Physics.CheckBox(previewPosition, Vector3.one * (cubeSize / 2), Quaternion.identity, cubeLayer);

            if (isOverlap)
            {
                // 큐브와 겹치는 경우 프리뷰 큐브 비활성화
                previewCube.SetActive(false);
            }
            else
            {
                previewCube.SetActive(true);
                previewCube.transform.position = previewPosition;
            }
        }
        else
        {
            previewCube.SetActive(false);
        }
    }

    void TrySpawnCube(Vector3 hitPoint)
    {
        Vector3 spawnPosition = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);

        // 큐브의 위치를 조정하여 지형 위에 위치하도록 함
        AdjustPositionToTerrain(ref spawnPosition, cubeSize);

        // 큐브와 겹치는지 여부 확인
        bool isOverlap = Physics.CheckBox(spawnPosition, Vector3.one * (cubeSize / 2), Quaternion.identity, cubeLayer);

        if (!isOverlap)
        {
            GameObject newCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
            CreateRockUnderCube(newCube.transform);
        }
    }

    void AdjustPositionToTerrain(ref Vector3 position, float cubeSize)
    {
        RaycastHit hit;
        Vector3[] bottomCorners = new Vector3[4];
        bottomCorners[0] = position + new Vector3(-cubeSize / 2, -cubeSize / 2, -cubeSize / 2);
        bottomCorners[1] = position + new Vector3(cubeSize / 2, -cubeSize / 2, -cubeSize / 2);
        bottomCorners[2] = position + new Vector3(-cubeSize / 2, -cubeSize / 2, cubeSize / 2);
        bottomCorners[3] = position + new Vector3(cubeSize / 2, -cubeSize / 2, cubeSize / 2);

        float maxHeight = float.MinValue;

        foreach (var corner in bottomCorners)
        {
            if (Physics.Raycast(corner + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, terrainLayer))
            {
                if (hit.point.y > maxHeight)
                {
                    maxHeight = hit.point.y;
                }
            }
        }

        position.y = maxHeight + cubeSize / 2;
    }

    void CreateRockUnderCube(Transform cubeTransform)
    {
        Vector3 rockPosition = cubeTransform.position - new Vector3(0, cubeSize, 0);
        GameObject rock = Instantiate(rockPrefab, rockPosition, Quaternion.identity);
        rock.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
    }
}