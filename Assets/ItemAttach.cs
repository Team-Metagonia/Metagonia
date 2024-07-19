using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

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
    }

    public void Attach(Item baseitem, Item attacheditem)
    {
        Destroy(attacheditem);
        Destroy(baseitem);
        Instantiate(_finishedUnit);
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

    }

    private void OnDestroy()
    {
        WorkBench.OnWorkStateChange -= ShowAttachableArea;
    }
}
