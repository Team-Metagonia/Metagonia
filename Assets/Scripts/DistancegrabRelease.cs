using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class DistanceGrabRelease : MonoBehaviour
{
    [Tooltip("Reference to the DistanceGrabInteractor component.")]

    [SerializeField] private DistanceGrabInteractor leftDistanceGrabInteractor;
    [SerializeField] private DistanceGrabInteractor rightDistanceGrabInteractor;
    

    [SerializeField] private HandGrabInteractor leftHandGrabInteractor;
    [SerializeField] private HandGrabInteractor rightHandGrabInteractor;

    [Tooltip("The target transform where the object will be released.")]
    
    [SerializeField] Transform leftHandAnchor;
    [SerializeField] Transform rightHandAnchor;

    [Tooltip("Distance threshold to automatically release the grab.")]
    [SerializeField]
    private float releaseDistanceThreshold = 0.1f;
    
    private void Update()
    {
        if (leftDistanceGrabInteractor.HasSelectedInteractable)
        {
            var selectedInteractable = leftDistanceGrabInteractor.SelectedInteractable;
            float distanceToTarget = Vector3.Distance(selectedInteractable.transform.position, leftHandAnchor.position);
            if (distanceToTarget <= releaseDistanceThreshold)
            {
                Debug.Log("Within release distance threshold. Releasing grab.");
                leftDistanceGrabInteractor.Unselect();
                Debug.Log("Distance Grab released.");
                
                var newInteractable = selectedInteractable.gameObject.GetComponent<HandGrabInteractable>();
                leftHandGrabInteractor.ForceSelect(newInteractable, true);
                Debug.Log("Hand Grab Forced");
            }
        }
        
        if (rightDistanceGrabInteractor.HasSelectedInteractable)
        {
            var selectedInteractable = rightDistanceGrabInteractor.SelectedInteractable;
            float distanceToTarget = Vector3.Distance(selectedInteractable.transform.position, rightHandAnchor.position);
            if (distanceToTarget <= releaseDistanceThreshold)
            {
                Debug.Log("Within release distance threshold. Releasing grab.");
                rightDistanceGrabInteractor.Unselect();
                Debug.Log("Distance Grab released.");
                
                var newInteractable = selectedInteractable.gameObject.GetComponent<HandGrabInteractable>();
                rightHandGrabInteractor.ForceSelect(newInteractable, true);
                Debug.Log("Hand Grab Forced");
            }
        }
        
        
    }
    
}
