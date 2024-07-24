using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO itemInfo;

    [SerializeField]
    protected HandGrabInteractor _currentHandInteractor;

    protected virtual void Awake()
    {
        
    }
    
    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public void SendSelectedItemInfo(PointerEvent pointerEvent)
    {
        _currentHandInteractor = pointerEvent.Data as HandGrabInteractor;
        if (_currentHandInteractor.gameObject.GetComponent<HandRef>().Handedness == Handedness.Left)
        {
            OVRBrain.Instance.LeftHandObject = this.gameObject;
        }
        else OVRBrain.Instance.RightHandObject = this.gameObject;
    }

    public void SendUnselectedItemInfo(PointerEvent pointerEvent)
    {
        _currentHandInteractor = pointerEvent.Data as HandGrabInteractor;
        if (_currentHandInteractor.gameObject.GetComponent<HandRef>().Handedness == Handedness.Left)
        {
            OVRBrain.Instance.LeftHandObject = null;
        }
        else OVRBrain.Instance.RightHandObject = null;
        _currentHandInteractor = null;
    }
}
