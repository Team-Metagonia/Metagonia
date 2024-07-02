using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 오브젝트에 부착해야하는 스크립트
// 아이템 정보 저장
// 아이템 획득 시 InventoryVR의 리스트에 해당 아이템 정보를 추가 후 아이템 파괴
public class ItemPickUp : MonoBehaviour
{
    public Item item;
    [SerializeField] bool instantPickUp;
    [SerializeField] float duration;
    public void PickUp()
    {
        InventoryVR.instance.AddtoInvenList(item);
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

    private void OnEnable()
    {
        

        if (instantPickUp)
        {
            StartCoroutine(AutomaticPickUp());
        }
    }
}
