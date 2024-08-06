using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificColliderDebugger : MonoBehaviour
{
    // 특정 콜라이더를 지정하기 위한 변수
    public Collider targetCollider;

    // 충돌 중인 오브젝트 목록을 저장하는 리스트
    private List<Collider> collidingObjects = new List<Collider>();

    void Start()
    {
        if (targetCollider == null)
        {
            Debug.LogError("Target Collider is not assigned.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // targetCollider와 충돌하는 경우에만 리스트에 추가
        if (other == targetCollider && !collidingObjects.Contains(other))
        {
            collidingObjects.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // targetCollider와의 충돌이 끝난 경우에만 리스트에서 제거
        if (other == targetCollider && collidingObjects.Contains(other))
        {
            collidingObjects.Remove(other);
        }
    }

    void Update()
    {
        // 주기적으로 충돌 중인 오브젝트를 디버깅 출력
        Debug.Log("Colliding objects count: " + collidingObjects.Count);
        foreach (Collider collidingObject in collidingObjects)
        {
            Debug.Log("Colliding with: " + collidingObject.name);
        }
    }
}