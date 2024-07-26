using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairMenu : CustomizationMenu
{
    public Transform randomInHairMenu;
    public Transform emptyInHairMenu;
    public Transform generalInHairMenu;
    
    private Image[] hairObjectImages;
    private Image[] hairUIImages;

    private Dictionary<Image, Image> imageToModelMap = new Dictionary<Image, Image>();

    public override void Initialize(CharacterCustomization character)
    {
        InjectCustomCharacter(character);
        InjectHair(character);
        SyncImageWithModel(hairUIImages, hairObjectImages);
        AddListenerToButtons();
    }

    private void InjectHair(CharacterCustomization character)
    {
        hairObjectImages = character.hairMeshTransform.GetComponentsInChildren<Image>(true);
        hairUIImages = generalInHairMenu.GetComponentsInChildren<Image>(true);
    }

    private void SyncImageWithModel(Image[] hairUIImages, Image[] hairObjectImages)
    {
        // GetComponentsInChildren includes parent itself ... 
        for (int i = 0; i < Mathf.Min(hairObjectImages.Length, hairUIImages.Length); i++) {
            Image hairObjectImage = hairObjectImages[i];
            Image hairUIImage = hairUIImages[i];
            hairUIImage.sprite = hairObjectImage.sprite;

            imageToModelMap.Add(hairUIImage, hairObjectImage);
        }
    }

    private void ApplyRandomHairStyle()
    {
        int choice = Random.Range(0, hairObjectImages.Length);
        Image applyImage = hairObjectImages[choice];
        
        SkinnedMeshRenderer renderer = applyImage.GetComponent<SkinnedMeshRenderer>();
        Mesh newMesh = renderer.sharedMesh;
        this.customCharacter.SetMesh(this.customCharacter.hairMeshTransform, newMesh);
    }

    private void ApplyEmptyHairStyle()
    {
        Mesh newMesh = null;
        this.customCharacter.SetMesh(this.customCharacter.hairMeshTransform, newMesh);
    }

    private void ApplyHairStyle(Button button)
    {
        Image image = button.GetComponent<Image>();
        if (!imageToModelMap.ContainsKey(image)) return;
        Image applyImage = imageToModelMap[image];
        
        SkinnedMeshRenderer renderer = applyImage.GetComponent<SkinnedMeshRenderer>();
        Mesh newMesh = renderer.sharedMesh;
        this.customCharacter.SetMesh(this.customCharacter.hairMeshTransform, newMesh);
    }

    private void AddListenerToButtons()
    {
        // Random Button
        Button randomButton = randomInHairMenu.GetComponentsInChildren<Button>()[0];
        randomButton.onClick.AddListener(() => ApplyRandomHairStyle());
        
        // Empty Button
        Button emptyButton = emptyInHairMenu.GetComponentsInChildren<Button>()[0];
        emptyButton.onClick.AddListener(() => ApplyEmptyHairStyle());
        
        // General Button
        Button[] buttons = generalInHairMenu.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ApplyHairStyle(button));
        }
    }
}
