using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridCellPrefab; // 그리드 셀 프리팹
    public int rows = 10; // 그리드의 행 수
    public int columns = 10; // 그리드의 열 수
    public float cellSize = 1.0f; // 그리드 셀의 크기

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 cellPosition = new Vector3(i * cellSize, 0, j * cellSize);
                Instantiate(gridCellPrefab, cellPosition, Quaternion.identity, transform);
            }
        }
    }
}