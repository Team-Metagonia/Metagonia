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

    private HandGrabInteractor[] newInteractors;

    [SerializeField]
    private HandGrabInteractable interactable;

    private void Start()
    {
        OVRControllerDrivenHands = GameObject.Find("OVRControllerDrivenHands").transform;
        newInteractors = OVRControllerDrivenHands.GetComponentsInChildren<HandGrabInteractor>(true);    
    }
    
    public void ReleaseDistanceGrab(PointerEvent pointerEvent)
    {
        DistanceGrabInteractor interactor = pointerEvent.Data as DistanceGrabInteractor;
        if (interactor == null || interactable == null) return;
        
        Handedness handedness = interactor.GetComponent<ControllerRef>().Handedness;
        HandGrabInteractor newInteractor = newInteractors[(int)handedness];
        if (newInteractor == null) return;

        Debug.Log($"[ForceHandGrab] Selected Hand: {handedness}, Interactor: {interactor.name}");
        
        if (handSelection == handedness)
        {
            Debug.Log("[ForceHandGrab] Hand match found, executing ForceSelect.");
            interactor.Unselect();
            newInteractor.ForceSelect(interactable, true);
        }
        else
        {
            Debug.Log("[ForceHandGrab] Hand match not found, skipping ForceSelect.");
        }
    }
}
