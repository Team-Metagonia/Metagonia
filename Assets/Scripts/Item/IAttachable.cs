using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttachable
{
    public void Attach(Item item1, Item item2);

    public void AttachToHand(HandGrabInteractor interactor, HandGrabInteractable[] interactable)
    {
        Debug.Log("Default Interface member");

        if (interactor.gameObject.GetComponent<HandRef>().Handedness == Handedness.Left)
        {
            interactor.ForceSelect(interactable[0], true);
        }
        else interactor.ForceSelect(interactable[1], true);

    }

    public void ShowAttachableArea(bool isActive);
    
    
}
