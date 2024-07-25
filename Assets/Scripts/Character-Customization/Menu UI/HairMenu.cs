using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairMenu : CustomizationMenu
{
    private Transform hairMeshTransformInCharacter;

    public Transform randomHairInHairMenu;
    public Transform generalHairsInHairMenu;
    
    private Image[] hairObjectImages;
    private Image[] hairUIImages;

    private Dictionary<Image, Image> imageToModelMap = new Dictionary<Image, Image>();

    public override void Initialize(CharacterCustomization character)
    {
        InjectCustomCharacter(character);
        InjectHair(character);
        SyncImageWithModel(hairObjectImages, hairUIImages);
        AddListenerToButtons();
    }

    private void InjectHair(CharacterCustomization character)
    {
        hairMeshTransformInCharacter = character.hairMeshTransform;
        Debug.Assert(hairMeshTransformInCharacter != null, "Hair Mesh Transform is null");

        hairObjectImages = hairMeshTransformInCharacter.GetComponentsInChildren<Image>(true);
        hairUIImages = generalHairsInHairMenu.GetComponentsInChildren<Image>(true);
    }

    private void SyncImageWithModel(Image[] hairObjectImages, Image[] hairUIImages)
    {
        // GetComponentsInChildren includes parent itself ... 
        for (int i = 0; i < Mathf.Min(hairObjectImages.Length, hairUIImages.Length); i++) {
            Image hairObjectImage = hairObjectImages[i];
            Image hairUIImage = hairUIImages[i];
            hairUIImage.sprite = hairObjectImage.sprite;

            imageToModelMap.Add(hairUIImage, hairObjectImage);
        }
    }

    private void ApplyRandomHairstyle()
    {
        int choice = Random.Range(0, hairObjectImages.Length);
        Image applyImage = hairObjectImages[choice];

        foreach (Image hairObjectImage in hairObjectImages)
        {
            bool flag = (hairObjectImage == applyImage);
            hairObjectImage.gameObject.SetActive(flag);
        }
    }

    private void ApplyHairstyle(Button button)
    {
        Image image = button.GetComponent<Image>();
        if (!imageToModelMap.ContainsKey(image)) return;
        Image applyImage = imageToModelMap[image];
        
        foreach (Image hairObjectImage in hairObjectImages)
        {
            bool shouldActivate = (hairObjectImage == applyImage);
            hairObjectImage.gameObject.SetActive(shouldActivate);
        }
    }

    private void AddListenerToButtons()
    {
        // Random Button
        Button randomButton = randomHairInHairMenu.GetComponentsInChildren<Button>()[0];
        randomButton.onClick.AddListener(() => ApplyRandomHairstyle());
        
        // General Button
        Button[] buttons = generalHairsInHairMenu.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ApplyHairstyle(button));
        }
    }
}
