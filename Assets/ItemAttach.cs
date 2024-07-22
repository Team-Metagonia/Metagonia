using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Unity.VisualScripting;
using Oculus.Interaction.Input;

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
        GameObject obj = CraftManager.Instance.CheckRecipeValidness(baseitem, attacheditem);
        if (obj == null)
        {
            Debug.Log("Invalid Recipe. Recipe must be declared in RecipeSO in order to succesfully attach items.");
            return;
        }


        Destroy(attacheditem.gameObject);
        Destroy(baseitem.gameObject);
        GameObject resultItem = Instantiate(obj);
        HandGrabInteractable[] interactable = resultItem.GetComponentsInChildren<HandGrabInteractable>();

        if(_currentHandInteractor.gameObject.GetComponent<HandRef>().Handedness==Handedness.Left)
        {
            _currentHandInteractor.ForceSelect(interactable[0], true);
        }
        else _currentHandInteractor.ForceSelect(interactable[1], true);


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
