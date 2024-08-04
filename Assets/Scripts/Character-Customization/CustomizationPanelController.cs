using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomizationPanelController : MonoBehaviour
{
    [HideInInspector]
    public CharacterCustomization customCharacter;

    [Header("Canvas")]
    public Transform rootCanvas;

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
        SetFurIcon(character);
        DeterminePosition(character);

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

    private void SetFurIcon(CharacterCustomization character)
    {
        if (furIcon == null)
        {
            Debug.LogWarning("Moustache Icon is not assigned");
            return;
        }

        bool shouldActivate = (character.sex == Sex.Male);
        furIcon.gameObject.SetActive(shouldActivate);
    }

    private Vector3 GeneratePanelPositionAfterSelected(CharacterCustomization character)
    {
        Vector3 origin = character.GetComponent<Collider>().bounds.center;
        Vector3 dir = -0.5f * character.transform.forward + (-1f) * character.transform.right;
        return origin + dir;
    }

    private void DeterminePosition(CharacterCustomization character)
    {
        Vector3 positionAfterSelected = GeneratePanelPositionAfterSelected(character);
        rootCanvas.transform.position = positionAfterSelected;
    }
}
