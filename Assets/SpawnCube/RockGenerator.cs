using UnityEngine;

public class RockGenerator : MonoBehaviour
{
    public GameObject[] rockPrefabs; // 다양한 모듈형 돌 프리팹 배열
    public LayerMask terrainLayer; // 지형 레이어
    public float minHeightAboveTerrain = 0.5f; // 최소 높이
    public float maxHeightAboveTerrain = 1.5f; // 최대 높이

    public void GenerateRocksUnderBuilding(GameObject building)
    {
        Collider buildingCollider = building.GetComponent<Collider>();
        if (buildingCollider != null)
        {
            Vector3[] corners = GetColliderCorners(buildingCollider);
            foreach (Vector3 corner in corners)
            {
                RaycastHit hit;
                if (Physics.Raycast(corner + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, terrainLayer))
                {
                    float height = Random.Range(minHeightAboveTerrain, maxHeightAboveTerrain);
                    Vector3 spawnPosition = new Vector3(corner.x, hit.point.y + height, corner.z);
                    GameObject rockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
                    Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    Vector3[] GetColliderCorners(Collider collider)
    {
        Bounds bounds = collider.bounds;
        return new Vector3[]
        {
            new Vector3(bounds.min.x, bounds.min.y, bounds.min.z),
            new Vector3(bounds.max.x, bounds.min.y, bounds.min.z),
            new Vector3(bounds.min.x, bounds.min.y, bounds.max.z),
            new Vector3(bounds.max.x, bounds.min.y, bounds.max.z)
        };
    }
}