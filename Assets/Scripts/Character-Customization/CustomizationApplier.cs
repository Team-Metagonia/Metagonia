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
        
    }

    private void Start()
    {
        customizationManager = FindObjectOfType<CharacterCustomizationManager>();
        bool customizationInfoExist = (customizationManager != null);
        
        if (!customizationInfoExist)
        {
            FinishCustomizationEarly();
            return;
        }

        Initialize();
        ApplyCustomizationInfo(info);
        FinishCustomization();
    }

    private void Initialize()
    {
        Debug.Assert(customizationManager != null, "Manager Disappears!");

        info = customizationManager.customizationInfo;
        Debug.Assert(info != null, "Info Diappears!");
    }

    private void ApplyCustomizationInfo(CustomizationInfo info) 
    {
        DetermineSex(info);
        SetCustomizationInfo(info);
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

    private void FinishCustomizationEarly()
    {
        humanFemaleCharacter.gameObject.SetActive(false);
        
        Destroy(humanMaleCharacter);
        Destroy(humanFemaleCharacter);
        Destroy(this.gameObject);
    }

    private void FinishCustomization()
    {
        // humanMaleCharacter.enabled = false;
        // humanFemaleCharacter.enabled = false;
        // customizationManager.gameObject.SetActive(false);
        // this.gameObject.SetActive(false);

        Destroy(humanMaleCharacter);
        Destroy(humanFemaleCharacter);
        Destroy(customizationManager.gameObject); 
        Destroy(this.gameObject);
    }
}
