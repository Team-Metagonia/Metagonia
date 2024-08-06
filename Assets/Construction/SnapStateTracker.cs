using UnityEngine;
using Oculus.Interaction;

public class SnapStateTracker : MonoBehaviour
{
    public SnapInteractable snapInteractable; // SnapInteractable 인스턴스 참조

    private bool _isSnapped = false;

    // 메터리얼을 변경할 오브젝트
    public Renderer targetRenderer;
    public Material snapMaterial;

    // 비활성화할 오브젝트
    public GameObject objectToDisable;

    void Update()
    {
        // SnapInteractable의 Selecting 상태를 확인하여 스냅 여부를 판단
        foreach (var interactor in snapInteractable.Interactors)
        {
            if (interactor.State == InteractorState.Select && interactor.Interactable == snapInteractable)
            {
                if (!_isSnapped)
                {
                    _isSnapped = true;
                    OnSnap(interactor);
                }
                return; // 스냅 상태인 인터렉터를 찾았으므로 더 이상 확인하지 않음
            }
        }
    }

    private void OnSnap(SnapInteractor interactor)
    {
        // 스냅 시 수행할 동작
        if (targetRenderer != null && snapMaterial != null)
        {
            targetRenderer.material = snapMaterial;
        }

        // 지정된 오브젝트를 비활성화
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }

        // 스냅된 SnapInteractor 오브젝트의 부모 비활성화
        if (interactor != null)
        {
            var parentObject = interactor.transform.parent.gameObject;
            if (parentObject != null)
            {
                parentObject.SetActive(false);
            }
        }
    }
}