using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    //UI�� ������ �����ܰ� ������ ǥ���ϴ� �޼��尡 �־����
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
    public void InstantiateHolder(Item item)
    {
        GameObject go = Instantiate(holder, materialPanel.transform);
        go.GetComponent<Image>().sprite = item.icon;
        go.GetComponentInChildren<TMP_Text>().text = InventoryVR.instance.itemQuantityPairs[item].ToString();
    }

    public void UpdateHolder(Item item)
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
