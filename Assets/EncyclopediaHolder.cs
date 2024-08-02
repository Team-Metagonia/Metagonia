using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncyclopediaHolder : MonoBehaviour
{
    public GameObject EncyclopediaObj;
    EncyclopediaBook book;
    //Vector3 instantiatePos => book._interactionTarget.transform.position - Vector3.forward;
    GameObject _instantiatedObj;
    private void Start()
    {
        book = GetComponentInParent<EncyclopediaBook>();
        book.OnCloseInteractionMode.AddListener(ClearObj);
    }

    public void InstantiateObj()
    {
        ClearObj();
        GameObject obj = Instantiate(EncyclopediaObj, book.transform);
        //obj.transform.localPosition = new Vector3(0, 0, -1);
        _instantiatedObj = obj;
    }

    public void ClearObj()
    {
        if (!_instantiatedObj) return;
        Destroy( _instantiatedObj );
    }
}
