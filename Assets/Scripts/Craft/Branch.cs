using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Branch : Item, IAttachable, IDamagable, IDroppable
{
    [SerializeField]
    Transform[] _attachPoints;

    [SerializeField] private float initHealth;

    private bool quit = false;

    public float Health { get; private set; }

    public void Attach(Item baseitem, Item attacheditem, int attachPointIndex)
    {
        Debug.Log("Attach Start");
        Debug.Log($"Base Item : {baseitem}, attached Item : {attacheditem}, attachPointIndex : {attachPointIndex}");
        //Check if Recipe is Valid and return result Object & catch exception
        GameObject obj = CraftManager.Instance.CheckRecipeValidness(baseitem, attacheditem, attachPointIndex);
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
        DropItems();
        Destroy(this.gameObject);
    }

    public void DropItems()
    {
        if (quit) return;
        if (itemInfo == null) return;
        if (itemInfo.dropItems == null || itemInfo.dropItems.Length == 0) return;

        for (int i = 0; i < itemInfo.dropItems.Length; i++)
        {
            GameObject itemToDrop = itemInfo.dropItems[i];
            GameObject instantiateditem = Instantiate(itemToDrop,gameObject.transform.position,Quaternion.identity);
            instantiateditem.GetComponent<Rigidbody>().AddExplosionForce(1, transform.position, 1);
        }
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
        float amount = damageInfo.damage;
        if (amount <= 0) return;

        Health -= amount;
        Health = Mathf.Max(Health, 0);

        if (Health <= 0)
        {
            Die();
            return;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Health = Mathf.Max(initHealth, Mathf.Epsilon);
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

    private void OnDisable()
    {
        if (isInBag) return;
       
    }

     
}
