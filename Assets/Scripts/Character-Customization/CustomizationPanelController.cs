using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomizationPanelController : MonoBehaviour
{
    [HideInInspector]
    public CharacterCustomization customCharacter;

    [Header("Icons")]
    public CustomizationIcon furIcon;

    [Header("Icon State Visualizer")]
    public Color activeColor;
    public Color inactiveColor;

    public Action<CharacterCustomization> OnInitialize;
    public Action<Menu> OnMenuOpen;
    public Action<Menu> OnMenuClose;

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
        if (character.sex == Sex.Female)
        {
            DisableFurIcon();
        }

        OnInitialize?.Invoke(character);
    }

    public void MenuOpen(Menu menu)
    {
        OnMenuOpen?.Invoke(menu);
    }

    public void MenuClose(Menu menu)
    {
        OnMenuClose?.Invoke(menu);
    }

    private void DisableFurIcon()
    {
        if (furIcon == null)
        {
            Debug.LogWarning("Moustache Icon is not assigned");
            return;
        }

        furIcon.gameObject.SetActive(false);
    }
}
