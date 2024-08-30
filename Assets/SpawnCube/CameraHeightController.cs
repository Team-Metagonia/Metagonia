using UnityEngine;

public class CameraHeightController : MonoBehaviour
{
    public Terrain terrain; // 지형 오브젝트
    public float heightOffset = 3.0f; // 지형 위에 유지할 높이 오프셋

    void Update()
    {
        if (terrain != null)
        {
            // 카메라의 현재 위치
            Vector3 cameraPosition = transform.position;
            
            // 카메라의 x와 z 좌표를 유지하면서 y 좌표를 조정
            float terrainHeight = terrain.SampleHeight(cameraPosition);
            transform.position = new Vector3(cameraPosition.x, terrainHeight + heightOffset, cameraPosition.z);
        }
    }
}