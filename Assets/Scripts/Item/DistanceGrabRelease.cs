using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using Oculus.Interaction.HandGrab;

public class DistanceGrabRelease : MonoBehaviour
{
    [Tooltip("Distance threshold to automatically release the grab.")]
    [SerializeField]
    private float releaseDistanceThreshold = 0.1f;

    [Tooltip("Select which hand to use for Force Select.")]
    [SerializeField]
    private Handedness handSelection;

    [SerializeField]
    private Transform OVRControllerDrivenHands;

    [SerializeField]
    private HandGrabInteractable interactable;

    public void ReleaseDistanceGrab(PointerEvent pointerEvent)
    {
        DistanceGrabInteractor interactor = pointerEvent.Data as DistanceGrabInteractor;
        if (interactor == null) return;

        Handedness handedness = interactor.GetComponent<ControllerRef>().Handedness;

        interactor.Unselect();
        ForceHandGrab(handedness);
    }

    public void ForceHandGrab(Handedness handedness)
    {
        if (interactable == null) return;

        HandGrabInteractor[] interactors = OVRControllerDrivenHands.GetComponentsInChildren<HandGrabInteractor>(true);
        HandGrabInteractor interactor = interactors[(int)handedness];

        Debug.Log($"[ForceHandGrab] Selected Hand: {handedness}, Interactor: {interactor.name}");
        
        if (handSelection == handedness)
        {
            Debug.Log("[ForceHandGrab] Hand match found, executing ForceSelect.");
            interactor.ForceSelect(interactable, true);
        }
        else
        {
            Debug.Log("[ForceHandGrab] Hand match not found, skipping ForceSelect.");
        }
    }
}