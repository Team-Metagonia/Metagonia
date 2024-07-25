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

        GameObject simpleObj = Instantiate(item.simplePrefab, go.transform);
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
