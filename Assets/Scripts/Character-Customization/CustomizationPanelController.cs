using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomizationPanelController : MonoBehaviour
{
    [HideInInspector]
    public CharacterCustomization customCharacter;

    public CustomizationMenu[] customizationMenus;
    public Action<CharacterCustomization> OnInitialize;
    public Action<Menu> OnMenuOpen;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void InjectCustomCharacter(CharacterCustomization character)
    {
        this.customCharacter = character;
    }

    public void Initialize(CharacterCustomization character)
    {
        InjectCustomCharacter(character);
        foreach (CustomizationMenu menu in customizationMenus)
        {
            // menu.InjectCustomCharacter(character);
            // menu.Initialize(character);
        }

        OnInitialize?.Invoke(character);
    }

    public void MenuOpen(Menu menu)
    {
        OnMenuOpen?.Invoke(menu);
    }

}
