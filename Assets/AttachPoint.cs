using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPoint : MonoBehaviour
{

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("AttachPoint Trigger");
    //}
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("AttachPoint collision");
        Item baseItem = transform.GetComponentInParent<ItemPickUp>().item;
        Item attachedItem = col.gameObject.GetComponent<ItemPickUp>().item;
        WorkBench.OnAttach?.Invoke(baseItem, attachedItem);
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
