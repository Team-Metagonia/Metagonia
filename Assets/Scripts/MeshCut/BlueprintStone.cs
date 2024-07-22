using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전제 조건
// guide stone 은 convex 이어야 한다
// guide stone 의 origin 은 mesh 내부에 존재해야 한다

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class BlueprintStone : MonoBehaviour
{
    // public Transform targetTransform = null;
    
    private MeshFilter meshFilter;
    private Mesh mesh;

    private HashSet<int> remainingTriangles;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter component is missing.");
            return;
        }

        mesh = meshFilter.sharedMesh;

        remainingTriangles = new HashSet<int>();
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            remainingTriangles.Add(i);
        }
    }

    private void Update()
    {
        // UpdateTransform();
    }

    // private void UpdateTransform()
    // {
    //     FollowTransform(targetTransform);
    // }

    // private void FollowTransform(Transform targetTransform)
    // {
    //     if (targetTransform == null) return;
        
    //     this.transform.position = targetTransform.position;
    //     this.transform.rotation = targetTransform.rotation;
    // }

    // public void InjectTargetTransform(Transform transform)
    // {
    //     targetTransform = transform;
    // }
    
    public int GetFaceCount()
    {
        int faceCount = mesh.triangles.Length / 3;
        return faceCount;
    }

    public bool IsCutCompleted()
    {
        return remainingTriangles.Count <= 0;
    }

    public HitInfo GetRemainingTriangleHitInfo()
    {
        if (IsCutCompleted()) 
        {
            return null;
        }
        
        List<int> remainingTrianglesList = new List<int>(remainingTriangles);
        int choice = Random.Range(0, remainingTrianglesList.Count);

        int i = remainingTrianglesList[choice];
        remainingTriangles.Remove(i);

        // 삼각형 배열과 정점 배열 가져오기
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        int vertexIndexA = triangles[i];
        int vertexIndexB = triangles[i + 1];
        int vertexIndexC = triangles[i + 2];

        // 정점 위치 가져오기
        Vector3 vertexA = meshFilter.transform.TransformPoint(vertices[vertexIndexA]);
        Vector3 vertexB = meshFilter.transform.TransformPoint(vertices[vertexIndexB]);
        Vector3 vertexC = meshFilter.transform.TransformPoint(vertices[vertexIndexC]);

        // 무게중심 계산
        Vector3 centroid = (vertexA + vertexB + vertexC) / 3f;

        // 법선 벡터 계산 (정점 순서에 따른 방향)
        Vector3 edgeX = vertexB - vertexA;
        Vector3 edgeY = vertexC - vertexA;
        Vector3 normal = Vector3.Cross(edgeX, edgeY).normalized;

        // 결과 출력
        Debug.Log($"Face {i / 3}: Centroid {centroid}, Normal {normal}");

        return new HitInfo(centroid, normal, normal);
    }
}
