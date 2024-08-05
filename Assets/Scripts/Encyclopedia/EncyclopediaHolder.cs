using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EncyclopediaHolder : MonoBehaviour
{
    public GameObject EncyclopediaObj;
    public string encyclopediaText;
    EncyclopediaBook book;
    TextMeshProUGUI _textMeshPro;
    
    //Vector3 instantiatePos => book._interactionTarget.transform.position - Vector3.forward;
    static GameObject _instantiatedObj;
    private void Start()
    {
        book = GetComponentInParent<EncyclopediaBook>();
        book.OnCloseInteractionMode.AddListener(ClearObj);
        _textMeshPro = GameObject.Find("TextCanvas").GetComponentInChildren<TextMeshProUGUI>();
        InteractableUnityEventWrapper[] wrappers = book.GetComponentsInChildren<InteractableUnityEventWrapper>();

        foreach (InteractableUnityEventWrapper w in wrappers) 
        {
            w.WhenUnselect.AddListener(ClearObj);
        }
    }

    public void InstantiateObj()
    {
        ClearObj();
        GameObject obj = Instantiate(EncyclopediaObj, book._interactionTarget);
        //obj.transform.localPosition = new Vector3(0, 0, -1);
        _instantiatedObj = obj;
    }

    public void UpdateText()
    {
        _textMeshPro.text = encyclopediaText;
    }

    public void ClearObj()
    {
        if (!_instantiatedObj) return;
        Destroy( _instantiatedObj );
    }
}
