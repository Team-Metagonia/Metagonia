using UnityEngine;
using Oculus.Interaction;

public class DistanceGrabRelease : MonoBehaviour
{
    [Tooltip("Reference to the DistanceGrabInteractor component.")]
    [SerializeField]
    private DistanceGrabInteractor distanceGrabInteractor;

    [Tooltip("The target transform where the object will be released.")]
    [SerializeField]
    private Transform targetTransform;

    [Tooltip("Distance threshold to automatically release the grab.")]
    [SerializeField]
    private float releaseDistanceThreshold = 0.1f;

    private void Update()
    {
        if (distanceGrabInteractor == null || targetTransform == null)
        {
            Debug.LogError("DistanceGrabInteractor or TargetTransform reference is not set.");
            return; // 필요한 레퍼런스가 설정되지 않은 경우 업데이트 중단
        }

        var currentInteractable = distanceGrabInteractor.SelectedInteractable;

        if (currentInteractable != null)
        {
            float distanceToTarget = Vector3.Distance(currentInteractable.transform.position, targetTransform.position);
            Debug.Log($"Distance to target: {distanceToTarget}");

            if (distanceToTarget <= releaseDistanceThreshold)
            {
                Debug.Log("Within release distance threshold. Releasing grab.");
                ReleaseGrab();
            }
        }
    }

    private void ReleaseGrab()
    {
        var selectedInteractable = distanceGrabInteractor.SelectedInteractable;
        if (selectedInteractable != null)
        {
            distanceGrabInteractor.Unselect(); // 선택 해제
            Debug.Log("Grab released.");
        }
    }
}