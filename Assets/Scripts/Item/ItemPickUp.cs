using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 오브젝트에 부착해야하는 스크립트
// 아이템 정보 저장
// 아이템 획득 시 InventoryVR의 리스트에 해당 아이템 정보를 추가 후 아이템 파괴
public class ItemPickUp : MonoBehaviour
{
    public Item item;
    public bool isInSlot;
    [SerializeField] bool instantPickUp;
    [SerializeField] float duration;
    [SerializeField] RayInteractable[] rays;
    public void PickUp()
    {
        InventoryVR.instance.AddToStackableList(item);
        Destroy(gameObject);
    }

    IEnumerator AutomaticPickUp()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime/duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = endPos;

        yield return null;

        PickUp();
        
    }

    public void SelectDebug()
    {
        Debug.Log("Select : " + item.itemName);
    }

    public void SlotPickUp()
    {
        if (!isInSlot) return;
        Debug.Log("Picked Up from Slot");
        isInSlot = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.parent = null;

        // If Picked up from Slot, disable Inventory ray UI in order to stop double input
        // runtime?
        foreach(var r in rays)
        {
            r.enabled = false;
        }
        
    }

    public void ItemUnSelect()
    {
        foreach (var r in rays)
        {
            r.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && instantPickUp)
        {
            StartCoroutine(AutomaticPickUp());
        }
    }

    private void OnEnable()
    {
        

        //if (instantPickUp)
        //{
        //    StartCoroutine(AutomaticPickUp());
        //}
    }

    
}
