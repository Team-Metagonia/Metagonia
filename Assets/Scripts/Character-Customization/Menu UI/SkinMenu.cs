using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinMenu : CustomizationMenu
{
    public Transform randomColorInSkinMenu;
    public Transform generalColorsInSkinMenu;

    private Dictionary<Image, Color> imageToColorMap = new Dictionary<Image, Color>();
    private List<Color> allColors = new List<Color>();

    public override void Initialize(CharacterCustomization character)
    {
        InjectCustomCharacter(character);
        BuildImageToColorMap();
        AddListenerToButtons();
    }

    private void BuildImageToColorMap()
    {
        Image[] images = generalColorsInSkinMenu.GetComponentsInChildren<Image>();

        foreach (Image image in images)
        {
            Color color = image.color;
            if (image.sprite != null) 
            {
                Texture2D texture = image.sprite.texture;
                int x = texture.width / 2;
                int y = texture.height / 2;
                color = texture.GetPixel(x, y);
            }
            
            imageToColorMap.Add(image, color);
            allColors.Add(color);
        }
    }

    public void ApplyRandomSkinColor()
    {
        int choice = Random.Range(0, allColors.Count);
        
        Color newColor = allColors[choice];
        this.customCharacter.SetColor(this.customCharacter.bodyMeshTransform, newColor);
    }

    public void ApplySkinColor(Button button)
    {
        Image image = button.GetComponent<Image>();
        if (!imageToColorMap.ContainsKey(image)) return;
        
        Color newColor = imageToColorMap[image];
        this.customCharacter.SetColor(this.customCharacter.bodyMeshTransform, newColor);
    }

    private void AddListenerToButtons()
    {
        // Random Button
        Button randomButton = randomColorInSkinMenu.GetComponentsInChildren<Button>()[0];
        randomButton.onClick.AddListener(() => ApplyRandomSkinColor());
        
        Button[] buttons = generalColorsInSkinMenu.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ApplySkinColor(button));
        }
    }
}
