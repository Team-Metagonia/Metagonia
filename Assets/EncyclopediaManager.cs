using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EncyclopediaManager : MonoBehaviour
{
    private static EncyclopediaManager _instance = null;
    public static EncyclopediaManager Instance { get { return _instance; } }

    public Dictionary<ItemSO, bool> ItemDic = new Dictionary<ItemSO, bool>();

    public Dictionary<string, bool> ActionDic = new Dictionary<string, bool>();

    public Dictionary<RecipeSO, bool> RecipeDic = new Dictionary<RecipeSO, bool>();

    public string[] ActionKey;

    public ItemSO[] ItemKey;

    public UnityEvent<ItemSO> OnItemUpdate;

    public UnityEvent<string> OnActionUpdate;
    void SetItemDic()
    {
        //Object[] items = Resources.LoadAll("ItemSO/");
        foreach (ItemSO key in ItemKey) 
        {
            ItemDic[key] = false;
        }
    }

    [ContextMenu("printItemDic")]
    public void printdic()
    {
        foreach(var pair in ItemDic) 
        {
            Debug.Log(pair.Key);
            Debug.Log(pair.Value);
        }
    }

    void SetActionDic()
    {
        foreach(string key in ActionKey)
        {
            ActionDic[key] = false;
        }
    }

    public void UpdateItemDic(ItemSO iteminfo)
    {
        if (ItemDic[iteminfo]) return;

        ItemDic[iteminfo] = true;
        OnItemUpdate?.Invoke(iteminfo);

    }

    public void UpdateActionDic(string actionInfo)
    {
        if (ActionDic[actionInfo]) return;

        ActionDic[actionInfo] = true;
        OnActionUpdate?.Invoke(actionInfo);
    }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        SetItemDic();
        SetActionDic();
    }
}
