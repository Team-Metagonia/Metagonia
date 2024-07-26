using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{

    private static CraftManager instance = null;
    public static CraftManager Instance
    {
        get
        {
            if (instance == null) return null;
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Transform itemSpawnPoint;

    public List<RecipeSO> recipeList = new List<RecipeSO>();

    public GameObject CheckRecipeValidness(Item item1, Item item2)
    {
        foreach (RecipeSO recipe in recipeList)
        {
            List<ItemSO> r = recipe.ingredients;
            if (r.Contains(item1.itemInfo) && r.Contains(item2.itemInfo))
            {
                return recipe.results[0];
            }
        }
        return null;
    }

    public bool CheckRecipe(Item item1, Item item2)
    {
        foreach (RecipeSO recipe in recipeList)
        {
            List<ItemSO> r = recipe.ingredients;
            if (r.Contains(item1.itemInfo) && r.Contains(item2.itemInfo))
            {
                return true;
            }
        }
        return false;
    }
}