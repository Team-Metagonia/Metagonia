using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurMenu : CustomizationMenu
{
    public Transform randomInFurMenu;
    public Transform emptyInFurMenu;
    public Transform generalBeardsInFurMenu;
    public Transform generalMoustachesInFurMenu;

    private Sprite emptyFurSprite;
    private Image[] beardObjectImages;
    private Image[] beardUIImages;
    private Image[] moustacheObjectImages;
    private Image[] moustacheUIImages;

    private Dictionary<Image, Image> imageToModelMap = new Dictionary<Image, Image>();

    public override void Initialize(CharacterCustomization character)
    {
        if (character.sex == Sex.Female) return;
        
        InjectCustomCharacter(character);
        InjectFur(character);
        SyncImageWithModel(beardUIImages, beardObjectImages);
        SyncImageWithModel(moustacheUIImages, moustacheObjectImages);
        AddListenerToButtons();
    }

    private void InjectFur(CharacterCustomization character)
    {
        emptyFurSprite = character.emptyFurSprite;

        beardObjectImages = character.beardMeshTransform.GetComponentsInChildren<Image>(true);
        beardUIImages = generalBeardsInFurMenu.GetComponentsInChildren<Image>(true);

        moustacheObjectImages = character.moustacheMeshTransform.GetComponentsInChildren<Image>(true);
        moustacheUIImages = generalMoustachesInFurMenu.GetComponentsInChildren<Image>(true);
    }

    private void SyncImageWithModel(Image[] uiImages, Image[] objectImages)
    {
        Image randomFurUIImage = randomInFurMenu.GetComponentsInChildren<Image>(true)[0];
        Image emptyFurUIImage = emptyInFurMenu.GetComponentsInChildren<Image>(true)[0];

        randomFurUIImage.sprite = emptyFurSprite;
        emptyFurUIImage.sprite = emptyFurSprite;

        for (int i = 0; i < Mathf.Min(objectImages.Length, uiImages.Length); i++) {
            Image objectImage = objectImages[i];
            Image uiImage = uiImages[i];
            uiImage.sprite = objectImage.sprite;

            imageToModelMap.Add(uiImage, objectImage);
        }
    }

    private void ApplyRandomFurStyle()
    {
        int beardChoice = Random.Range(0, beardObjectImages.Length);
        int moustacheChoice = Random.Range(0, moustacheObjectImages.Length);
        
        Image applyBeardImage = beardObjectImages[beardChoice];
        Image applyMoustacheImage = moustacheObjectImages[moustacheChoice];

        SkinnedMeshRenderer beardRenderer = applyBeardImage.GetComponent<SkinnedMeshRenderer>();
        Mesh newBeardMesh = beardRenderer.sharedMesh;
        this.customCharacter.SetMesh(this.customCharacter.beardMeshTransform, newBeardMesh.name);

        SkinnedMeshRenderer moustacheRenderer = applyMoustacheImage.GetComponent<SkinnedMeshRenderer>();
        Mesh newMoustacheMesh = moustacheRenderer.sharedMesh;
        this.customCharacter.SetMesh(this.customCharacter.moustacheMeshTransform, newMoustacheMesh.name);
    }

    private void ApplyEmptyFurStyle()
    {
        this.customCharacter.SetMesh(this.customCharacter.beardMeshTransform, null);
        this.customCharacter.SetMesh(this.customCharacter.moustacheMeshTransform, null);
    }

    private void ApplyFurStyle(Button button, Image[] objectImages)
    {
        Image image = button.GetComponent<Image>();
        if (!imageToModelMap.ContainsKey(image)) return;
        Image applyImage = imageToModelMap[image];

        SkinnedMeshRenderer renderer = applyImage.GetComponent<SkinnedMeshRenderer>();
        Mesh newMesh = renderer.sharedMesh;
        this.customCharacter.SetMesh(renderer.transform.parent, newMesh.name);
    }

    private void AddListenerToButtons()
    {
        // Random Button
        Button randomButton = randomInFurMenu.GetComponentsInChildren<Button>()[0];
        randomButton.onClick.AddListener(() => ApplyRandomFurStyle());

        // Empty Button
        Button emptyButton = emptyInFurMenu.GetComponentsInChildren<Button>()[0];
        emptyButton.onClick.AddListener(() => ApplyEmptyFurStyle());
        
        // General Button
        // A. General Beard Button
        Button[] beardButtons = generalBeardsInFurMenu.GetComponentsInChildren<Button>();
        foreach (Button beardButton in beardButtons)
        {
            beardButton.onClick.AddListener(() => ApplyFurStyle(beardButton, beardObjectImages));
        }

        // B. General Moustache Button
        Button[] moustacheButtons = generalMoustachesInFurMenu.GetComponentsInChildren<Button>();
        foreach (Button moustacheButton in moustacheButtons)
        {
            moustacheButton.onClick.AddListener(() => ApplyFurStyle(moustacheButton, moustacheObjectImages));
        }
    }
}
