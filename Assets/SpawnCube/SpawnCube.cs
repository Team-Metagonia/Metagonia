using UnityEngine;

public class SpawnCube : MonoBehaviour
{
    public GameObject cubePrefab; // 생성할 큐브 프리팹
    public GameObject previewCubePrefab; // 투명한 프리뷰 큐브 프리팹
    public LayerMask planeLayer; // 플레인 레이어
    public LayerMask cubeLayer; // 큐브 레이어

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
        // 레이저 포인터를 통해 플레인과 상호작용
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer))
        {
            // 히트 지점의 위치 계산
            Vector3 hitPoint = hit.point;

            // 큐브가 이미 있는지 확인
            Collider[] colliders = Physics.OverlapBox(new Vector3(hitPoint.x, 0.5f, hitPoint.z), new Vector3(cubeSize / 2, cubeSize / 2, cubeSize / 2), Quaternion.identity, cubeLayer);
            
            if (colliders.Length == 0)
            {
                // 큐브가 없는 경우 프리뷰 큐브 활성화
                previewCube.SetActive(true);
                previewCube.transform.position = new Vector3(hitPoint.x, 0.5f, hitPoint.z); // 플레인 위에 위치하도록 Y 축 조정
            }
            else
            {
                // 큐브가 있는 경우 프리뷰 큐브 비활성화
                previewCube.SetActive(false);
            }

            // 우측 컨트롤러의 IndexTrigger 입력 감지
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && previewCube.activeSelf)
            {
                TrySpawnCube(hitPoint); // 큐브 생성 함수 호출
            }
        }
        else
        {
            // 레이저가 플레인에 히트하지 않으면 프리뷰 큐브를 비활성화
            previewCube.SetActive(false);
        }
    }

    void TrySpawnCube(Vector3 hitPoint)
    {
        // 큐브가 이미 있는지 확인
        Collider[] colliders = Physics.OverlapBox(new Vector3(hitPoint.x, 0.5f, hitPoint.z), new Vector3(cubeSize / 2, cubeSize / 2, cubeSize / 2), Quaternion.identity, cubeLayer);
        if (colliders.Length == 0)
        {
            // 큐브가 없는 경우 생성할 위치를 계산
            Vector3 spawnPosition = new Vector3(hitPoint.x, 0.5f, hitPoint.z); // 큐브가 플레인 위에 생성되도록 Y 축 위치 조정
            Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Cannot spawn cube here. Another cube is already present.");
        }
    }
}
