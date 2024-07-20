using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전제 조건
// guide stone 은 convex 이어야 한다
// guide stone 의 origin 은 mesh 내부에 존재해야 한다

[RequireComponent(typeof(MeshCollider))]
public class BlueprintStone : MonoBehaviour
{
    public Transform targetTransform = null;

    private void Awake()
    {

    }

    private void Update()
    {
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        FollowTransform(targetTransform);
    }

    private void FollowTransform(Transform targetTransform)
    {
        if (targetTransform == null) return;
        
        this.transform.position = targetTransform.position;
        this.transform.rotation = targetTransform.rotation;
    }

    public void InjectTargetTransform(Transform transform)
    {
        targetTransform = transform;
    }
}
