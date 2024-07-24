using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CustomizationPanelController : MonoBehaviour
{
    private CharacterCustomization customCharacter;

    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private bool quit = false;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        CharacterCustomizationManager.Instance.OnSexSelectionFinished += InjectCustomCharacter;
    }

    private void OnDisable()
    {
        if (quit) return;
        CharacterCustomizationManager.Instance.OnSexSelectionFinished -= InjectCustomCharacter;
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnApplicationQuit()
    {
        quit = true;
    }

    public void ApplySkinColor(Button button)
    {
        if (customCharacter == null || skinnedMeshRenderers == null)
        {
            InjectCustomCharacter();
        }

        Debug.Assert(customCharacter != null, "Custom Character is not injected!");
        Color newColor = button.GetComponent<Image>().color;

        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            if (renderer.material == null) continue;
            renderer.material.color = newColor;
        }
    }
    
    private void InjectCustomCharacter()
    {
        this.customCharacter = CharacterCustomizationManager.Instance.selectedCharacter;
        
        Transform bodyMeshTransform = this.customCharacter.bodyMeshTransform;
        skinnedMeshRenderers = bodyMeshTransform.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void InjectCustomCharacter(CharacterCustomization customCharacter)
    {
        this.customCharacter = customCharacter;

        Transform bodyMeshTransform = this.customCharacter.bodyMeshTransform;
        skinnedMeshRenderers = bodyMeshTransform.GetComponentsInChildren<SkinnedMeshRenderer>();
    }
}
