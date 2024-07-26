using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationIcon : MonoBehaviour
{
    public CustomizationMenu menu;
    
    public void OnClick()
    {
        menu.Toggle();
    }
}
