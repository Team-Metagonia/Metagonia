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
        item baseItem = transform.GetComponentInParent<item>();
        if(col.gameObject.TryGetComponent<item>(out item i))
        {
            item attachedItem = i;
            WorkBench.OnAttach?.Invoke(baseItem, attachedItem);
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
