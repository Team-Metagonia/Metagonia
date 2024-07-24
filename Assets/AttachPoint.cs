using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPoint : MonoBehaviour
{

    
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("AttachPoint collision");
        Item baseItem = this.transform.GetComponentInParent<Item>();
        IAttachable attachable = transform.GetComponentInParent<IAttachable>();
        if(col.gameObject.TryGetComponent<Item>(out Item i))
        {
            Item attachedItem = i;

            attachable.Attach(baseItem, attachedItem);
            //If there are more than one branch, Same events are subscribed multiple times and cause problem
            //WorkBench.OnAttach?.Invoke(baseItem, attachedItem);


        }
        
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
