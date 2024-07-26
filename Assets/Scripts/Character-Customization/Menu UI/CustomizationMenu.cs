using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationMenu : Menu
{
    public CustomizationPanelController panelController;
    protected CharacterCustomization customCharacter;

    protected override void Awake()
    {
        panelController.OnInitialize += Initialize;
        panelController.OnMenuOpen += MenuOpen;

        base.Awake();
    }

    public void InjectCustomCharacter(CharacterCustomization character)
    {
        this.customCharacter = character;
    }

    public virtual void Initialize(CharacterCustomization character)
    {
        
    }


    public override void Open()
    {
        panelController.MenuOpen(this);
    }

    public override void Close()
    {
        base.Close();
        panelController.MenuClose(this);
    }
    
    public void MenuOpen(Menu menu) 
    {
        if (menu != this) this.Close();
        else base.Open();
    }
}
