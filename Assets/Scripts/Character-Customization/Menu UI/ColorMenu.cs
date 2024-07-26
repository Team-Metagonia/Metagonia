using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMenu : CustomizationMenu
{
    public Transform randomColorInMenu;
    public Transform generalColorsInMenu;

    protected Dictionary<Image, Color> imageToColorMap = new Dictionary<Image, Color>();
    protected List<Color> allColors = new List<Color>();
    
    protected Transform[] meshTransforms;

    protected override void Awake()
    {
        panelController.OnInitialize += Initialize;
        
        isOpen = true;
        this.gameObject.SetActive(true);
    }

    public override void Initialize(CharacterCustomization character)
    {
        InjectCustomCharacter(character);
        DetermineActivation(character);
        DetermineMeshTransforms(character);
        BuildImageToColorMap();
        AddListenerToButtons();
    }

    private void DetermineActivation(CharacterCustomization character)
    {
        Vector2 origin = new Vector2(panelController.transform.position.x, panelController.transform.position.z);
        Vector2 colorMenuPosition = new Vector2(transform.position.x, transform.position.z);
        Vector2 characterPosition = new Vector3(character.transform.position.x, character.transform.position.z);

        float value = Vector2.Dot(colorMenuPosition - origin, characterPosition - origin);

        isOpen = (value < 0);
        this.gameObject.SetActive(isOpen);
    }

    protected virtual void DetermineMeshTransforms(CharacterCustomization character)
    {
        meshTransforms = new Transform[] { character.beardMeshTransform, character.moustacheMeshTransform };
    }

    private void BuildImageToColorMap()
    {
        Image[] images = generalColorsInMenu.GetComponentsInChildren<Image>();

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

    public virtual void ApplyRandomColor()
    {
        int choice = Random.Range(0, allColors.Count);
        
        Color newColor = allColors[choice];
        foreach (Transform meshTransform in meshTransforms)
        {
            this.customCharacter.SetColor(meshTransform, newColor);
        }
    }

    public virtual void ApplyColor(Button button)
    {
        Image image = button.GetComponent<Image>();
        if (!imageToColorMap.ContainsKey(image)) return;
        
        Color newColor = imageToColorMap[image];
        foreach (Transform meshTransform in meshTransforms)
        {
            this.customCharacter.SetColor(meshTransform, newColor);
        }
    }

    protected virtual void AddListenerToButtons()
    {
        // Random Button
        Button randomButton = randomColorInMenu.GetComponentsInChildren<Button>()[0];
        randomButton.onClick.AddListener(() => ApplyRandomColor());
        
        Button[] buttons = generalColorsInMenu.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ApplyColor(button));
        }
    }
}
