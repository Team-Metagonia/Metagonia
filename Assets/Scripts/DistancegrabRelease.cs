using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using Oculus.Interaction.HandGrab;

public class DistanceGrabRelease : MonoBehaviour
{
    [Tooltip("Find Interactors")]
    [SerializeField]
    private Transform OVRControllerDrivenHands;
    private HandGrabInteractor[] newInteractors;
    
    [Tooltip("Select which hand to use for Force Select.")]
    [SerializeField]
    private HandGrabInteractable handGrabInteractable;
    private Handedness interactableHandedness;
    
    [Tooltip("Distance threshold to automatically release the grab.")]
    [SerializeField]
    private float releaseDistanceThreshold = 0.1f;

    private void Start()
    {
        newInteractors = OVRControllerDrivenHands.GetComponentsInChildren<HandGrabInteractor>(true);
        if (handGrabInteractable == null) handGrabInteractable = GetComponent<HandGrabInteractable>();
        interactableHandedness = handGrabInteractable.HandGrabPoses[0].HandPose.Handedness;
    }

    public void ReleaseDistanceGrab(PointerEvent pointerEvent)
    {
        DistanceGrabInteractor interactor = pointerEvent.Data as DistanceGrabInteractor;
        if (interactor == null || handGrabInteractable == null) return;

        Handedness interactorHandedness = interactor.GetComponent<ControllerRef>().Handedness;

        if (interactorHandedness == interactableHandedness)
        {
            interactor.Unselect();
            HandGrabInteractor newInteractor = newInteractors[(int) interactorHandedness];
            newInteractor.ForceSelect(handGrabInteractable, true);
        }
    }
}
