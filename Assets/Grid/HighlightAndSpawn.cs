using UnityEngine;

public class HighlightAndSpawn : MonoBehaviour
{
    public GameObject cubePrefab; // 생성할 큐브 프리팹
    public LayerMask cellLayer; // 그리드 셀 레이어
    private GameObject highlightedCell;

    void Update()
    {
        // 레이저 포인터를 통해 그리드 셀과 상호작용
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cellLayer))
        {
            // 레이저가 그리드 셀 위에 있을 때 셀의 색상을 파란색으로 변경
            HighlightCell(hit.collider.gameObject);

            // 우측 컨트롤러의 IndexTrigger 입력 감지
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                TrySpawnCube(hit.collider.gameObject); // 큐브 생성 함수 호출
            }
        }
        else
        {
            ClearHighlight(); // 레이저가 그리드 밖으로 나갔을 때 강조 표시 제거
        }
    }

    void HighlightCell(GameObject cell)
    {
        if (highlightedCell != null && highlightedCell != cell)
        {
            // 이전에 강조된 셀의 색상을 원래대로 되돌림
            highlightedCell.GetComponent<Renderer>().material.color = Color.white;
        }

        // 새로운 셀을 강조
        highlightedCell = cell;
        highlightedCell.GetComponent<Renderer>().material.color = Color.blue;
    }

    void ClearHighlight()
    {
        if (highlightedCell != null)
        {
            highlightedCell.GetComponent<Renderer>().material.color = Color.white;
            highlightedCell = null;
        }
    }

    void TrySpawnCube(GameObject cell)
    {
        Vector3 spawnPosition = cell.transform.position + Vector3.up * 0.5f; // 큐브가 셀 위에 생성되도록 Y 축 위치 조정
        Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
    }
}