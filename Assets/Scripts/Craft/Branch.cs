using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Branch : Item, IAttachable, IDamagable 
{
    [SerializeField]
    Transform[] _attachPoints;

    [SerializeField]
    GameObject _visualPoint;

    [SerializeField]
    GameObject _finishedUnit;

    public float Health => throw new System.NotImplementedException();

    [SerializeField]
    //HandGrabInteractor _currentHandInteractor;

    public void Attach(Item baseitem, Item attacheditem)
    {
        //Check if Recipe is Valid and return result Object & catch exception
        GameObject obj = CraftManager.Instance.CheckRecipeValidness(baseitem, attacheditem);
        if (obj == null)
        {
            Debug.Log("Invalid Recipe. Recipe must be declared in RecipeSO in order to succesfully attach items.");
            return;
        }

        //Instantiate Object into World and Destroy ingredient Objects
        GameObject resultItem = Instantiate(obj, baseitem.transform.position, Quaternion.identity);
        Destroy(attacheditem.gameObject);
        Destroy(baseitem.gameObject);

        //Branch based result Items should be instantiated in hands
        IAttachable attachable = this;
        HandGrabInteractable[] interactable = resultItem.GetComponentsInChildren<HandGrabInteractable>();
        attachable.AttachToHand(_currentHandInteractor, interactable);
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void ShowAttachableArea(bool isActive)
    {
        if (_currentHandInteractor == null) return;

        Debug.Log("Show Attachable Area : " + isActive);

        foreach (Transform t in _attachPoints)
        {
            t.gameObject.SetActive(isActive);
        }
       
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        WorkBench.OnWorkStateChange += ShowAttachableArea;
    }
    private void OnDestroy()
    {
        WorkBench.OnWorkStateChange -= ShowAttachableArea;
    }
}
