using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationIconStateVisualizer : MonoBehaviour
{
    public CustomizationIcon icon;
    
    private CustomizationMenu menu;
    private CustomizationPanelController panelController;

    private Image image;
    private Color activeColor;
    private Color inactiveColor;

    private void Awake()
    {
        menu = icon.menu;
        panelController = menu.panelController;

        image = GetComponentsInChildren<Image>()[0];
        activeColor = panelController.activeColor;
        inactiveColor = panelController.inactiveColor;

        panelController.OnMenuOpen += MenuOpen;
        panelController.OnMenuClose += MenuClose;
    }

    private void SetActive(bool flag)
    {
        image.color = flag ? activeColor : inactiveColor;
    }

    private void MenuOpen(Menu _menu)
    {
        if (this.menu != _menu) return;
        this.SetActive(true);
    }

    private void MenuClose(Menu _menu)
    {
        if (this.menu != _menu) return;
        this.SetActive(false);
    }
}
