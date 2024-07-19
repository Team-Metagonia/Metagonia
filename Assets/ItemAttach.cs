using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Unity.VisualScripting;

public class ItemAttach : MonoBehaviour, IAttachable
{
    [SerializeField]
    Transform[] _attachPoints;

    [SerializeField]
    GameObject _visualPoint;

    [SerializeField]
    GameObject _finishedUnit;

    [SerializeField]
    HandGrabInteractor _currentHandInteractor;

    private void Awake()
    {
        WorkBench.OnWorkStateChange += ShowAttachableArea;
        WorkBench.OnAttach += Attach;
    }

    public void Attach(item baseitem, item attacheditem)
    {
        Destroy(attacheditem.gameObject);
        Destroy(baseitem.gameObject);

        GameObject obj = Instantiate(_finishedUnit);
        HandGrabInteractable interactable = obj.GetComponentInChildren<HandGrabInteractable>();

        _currentHandInteractor.ForceSelect(interactable, true);
    }

    public void ShowAttachableArea(bool isActive)
    {
        foreach (Transform t in _attachPoints) 
        {
            t.gameObject.SetActive(isActive);
        }
        Debug.Log("Show Attachable Area!");
    }

    public void ShowAttachableLink(bool isActive)
    {
        Debug.Log("Show Attachable Link!");
    }

    public void CheckCurrentHand(PointerEvent pointerEvent)
    {
        _currentHandInteractor = pointerEvent.Data as HandGrabInteractor;
    }

    private void OnDestroy()
    {
        WorkBench.OnWorkStateChange -= ShowAttachableArea;
    }
}
