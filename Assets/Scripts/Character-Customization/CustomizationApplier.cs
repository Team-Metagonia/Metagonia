using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomizationApplier : MonoBehaviour
{
    public CharacterCustomization humanMaleCharacter;
    public CharacterCustomization humanFemaleCharacter;
    
    private CharacterCustomization customCharacter;
    private CharacterCustomizationManager customizationManager;
    private CustomizationInfo info;
    
    private void Awake()
    {   
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
        DetermineSex(info);
        SetCustomizationInfo(info);
    }
    private void Initialize()
    {
        customizationManager = FindObjectOfType<CharacterCustomizationManager>();
        Debug.Assert(customizationManager != null, "Manager Disappears!");

        info = customizationManager.customizationInfo;
        Debug.Assert(info != null, "Info Diappears!");
    }
    
    private void DetermineSex(CustomizationInfo info)
    {
        switch (info.sex)
        {
            case Sex.Male:
                customCharacter = humanMaleCharacter;
                break;
            case Sex.Female:
                customCharacter = humanFemaleCharacter;
                break;
            default:
                Debug.LogError("No assigned character for sex: " + info.sex);
                break;
        }

        humanMaleCharacter.gameObject.SetActive(false);
        humanFemaleCharacter.gameObject.SetActive(false);
        customCharacter.gameObject.SetActive(true);
    }

    private void SetCustomizationInfo(CustomizationInfo info)
    {
        customCharacter.SetCustomizationInfo(info);
    }
}
