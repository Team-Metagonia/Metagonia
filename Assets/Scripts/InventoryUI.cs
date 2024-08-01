using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    //UI에 아이템 아이콘과 수량을 표시하는 메서드가 있어야함
    [SerializeField] GameObject materialPanel;
    [SerializeField] GameObject holder;
    [SerializeField] bool isInteractive;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InstantiateHolder(ItemSO item)
    {
        GameObject go = Instantiate(holder, materialPanel.transform);
        go.GetComponent<Image>().sprite = item.icon;
        go.GetComponentInChildren<TMP_Text>().text = InventoryVR.instance.itemQuantityPairs[item].ToString();
        go.GetComponent<Toggle>().interactable = isInteractive;
        if (isInteractive)
        {
            go.GetComponent<Holder>().HoldingObject = item.completePrefab;
            go.GetComponent<Holder>().itemQuantity = item.quantity;
            //GameObject simpleObj = Instantiate(item.simplePrefab, go.transform);
            //simpleObj.transform.localScale = new Vector3(15, 15, 15);
            //simpleObj.transform.localPosition = new Vector3(0,-10,-10);
        }

        
    }

    public void UpdateHolder(ItemSO item)
    {
        foreach (Transform t in materialPanel.transform)
        {
            GameObject go = t.gameObject;
            if (item.icon == go.GetComponent<Image>().sprite)
            {
                go.GetComponentInChildren<TMP_Text>().text = InventoryVR.instance.itemQuantityPairs[item].ToString();

                return;
            }
        }
    }
}
