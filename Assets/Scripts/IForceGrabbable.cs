using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.HandGrab;
public interface IForceGrabbable
{
    public void ForceHandGrab(HandGrabInteractable interactable, bool allowRelease);
    
}
