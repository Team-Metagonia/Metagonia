using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ������Ʈ�� �����ؾ��ϴ� ��ũ��Ʈ
// ������ ���� ����
// ������ ȹ�� �� InventoryVR�� ����Ʈ�� �ش� ������ ������ �߰� �� ������ �ı�
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
