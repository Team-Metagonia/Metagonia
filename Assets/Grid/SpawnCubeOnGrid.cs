using UnityEngine;

public class SpawnCubeOnGrid : MonoBehaviour
{
    public GameObject cubePrefab; // 생성할 큐브 프리팹
    public GameObject gridObject; // 그리드 오브젝트
    public LayerMask gridLayer; // 그리드 레이어

    private GameObject highlightedCell;

    void Update()
    {
        // 레이저 포인터를 통해 그리드와 상호작용
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, gridLayer))
        {
            // 레이저가 그리드 칸 위에 있을 때 칸의 색상을 파란색으로 변경
            HighlightCell(hit.point);

            // 우측 컨트롤러의 IndexTrigger 입력 감지
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                TrySpawnCube(hit.point); // 큐브 생성 함수 호출
            }
        }
        else
        {
            ClearHighlight(); // 레이저가 그리드 밖으로 나갔을 때 강조 표시 제거
        }
    }

    void HighlightCell(Vector3 hitPoint)
    {
        // 그리드의 각 칸을 계산하여 칸의 색상을 파란색으로 변경
        Vector3 cellPosition = GetNearestCellCenter(hitPoint);

        if (highlightedCell != null)
        {
            // 이전에 강조된 셀의 색상을 원래대로 되돌림
            highlightedCell.GetComponent<Renderer>().material.color = Color.white;
        }

        // 새로운 셀을 강조
        highlightedCell = GetCellAtPosition(cellPosition);
        if (highlightedCell != null)
        {
            highlightedCell.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    void ClearHighlight()
    {
        if (highlightedCell != null)
        {
            highlightedCell.GetComponent<Renderer>().material.color = Color.white;
            highlightedCell = null;
        }
    }

    void TrySpawnCube(Vector3 hitPoint)
    {
        // 그리드의 각 칸을 계산하여 큐브를 생성
        Vector3 spawnPosition = GetNearestCellCenter(hitPoint);
        spawnPosition.y += 0.5f; // 큐브가 플레인 위에 생성되도록 Y 축 위치 조정
        Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
    }


    Vector3 GetNearestCellCenter(Vector3 hitPoint)
    {
        // hitPoint를 기준으로 가장 가까운 그리드 칸의 중앙을 계산
        float gridSize = 1.0f; // 그리드 칸의 크기 (필요에 따라 조정)
        float halfGridSize = gridSize / 2.0f;
        float x = Mathf.Floor(hitPoint.x / gridSize) * gridSize + halfGridSize;
        float z = Mathf.Floor(hitPoint.z / gridSize) * gridSize + halfGridSize;
        return new Vector3(x, hitPoint.y, z);
    }

    GameObject GetCellAtPosition(Vector3 position)
    {
        // 위치를 기준으로 해당 칸의 게임 오브젝트를 가져옴
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 10, Vector3.down, out hit, Mathf.Infinity, gridLayer))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}
