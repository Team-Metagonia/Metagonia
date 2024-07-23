using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe/Create New Recipe Data")]

public class RecipeSO : ScriptableObject
{
    public string id;
    public List <ItemSO> ingredients;
    public List <GameObject> results;

    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

}
