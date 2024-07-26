using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class CharacterCustomization : MonoBehaviour
{
    private Animator animator;
    private SelectionMode selectionMode;

    private Object selectionLock = new Object();
    private bool isSexSelectionFinished = false;
    private bool quit = false;

    public Sex sex;
    public CharacterCustomizationManager customizationManager;

    [Header("Mesh Transforms")]
    public Transform bodyMeshTransform;
    public Transform hairMeshTransform;
    
    [Header("Optional Mesh Transforms")]
    public Transform beardMeshTransform;
    public Transform moustacheMeshTransform;

    [Header("Customization Info")]
    public Color skinColor = default(Color);
    public Color hairColor = default(Color);
    public Color furColor = default(Color);
    public Mesh hairStyle = null;
    public Mesh beardStyle = null;
    public Mesh moustacheStyle = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        EnterSexSelection();
        UpdateCustomizationInfo();
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        if (customizationManager != null)
        {
            customizationManager.OnSexSelectionFinished += SexSelectionFinished;
        }
    }

    private void OnDisable()
    {
        if (quit) return;

        if (customizationManager != null)
        {
            customizationManager.OnSexSelectionFinished -= SexSelectionFinished;
        }
    }

    private void Update()
    {
        
    }

    private void OnApplicationQuit() 
    {
        quit = true;    
    }

    public void WhenHover()
    {   
        if (selectionMode == SelectionMode.Sex) 
        {
            if (isSexSelectionFinished) return;
            animator.SetTrigger("Hover");
        }

        // else if ...
    }

    public void WhenUnhover()
    {
        if (selectionMode == SelectionMode.Sex) 
        {
            if (isSexSelectionFinished) return;
            animator.SetTrigger("Idle");
        }
    }

    public void WhenSelect()
    {
        if (selectionMode == SelectionMode.Sex) 
        {
            lock (selectionLock)
            {
                if (isSexSelectionFinished) return;
                animator.SetTrigger("Select");
                FinishSexSelection();
                EnterStyleSelection();
            }
        }
    }

    public void WhenUnselect()
    {
        // Do Nothing
    }

    private void EnterSexSelection()
    {
        selectionMode = SelectionMode.Sex;
    }

    private void FinishSexSelection()
    {
        customizationManager.SexSelectionFinished(this);
    }

    private void SexSelectionFinished(CharacterCustomization customCharacter)
    {
        isSexSelectionFinished = true;
        if (customCharacter == this) return;
        
        if (selectionMode == SelectionMode.Sex) 
        {
            animator.SetTrigger("Death");
        }
        // Destroy(this.gameObject);
    }

    private void EnterStyleSelection()
    {
        selectionMode = SelectionMode.Style;
    }

    private Color GetColor(Transform meshTransform)
    {
        SkinnedMeshRenderer[] meshRenderers = meshTransform.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (meshRenderers.Length == 0) return default(Color);
            
        SkinnedMeshRenderer renderer = meshRenderers[0];
        Color color = renderer.material.color;
        return color;
    }

    private Mesh GetMesh(Transform meshTransform)
    {
        SkinnedMeshRenderer[] meshRenderers = meshTransform.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (meshRenderers.Length == 0) return null;
            
        SkinnedMeshRenderer renderer = meshRenderers[0];
        Mesh mesh = renderer.sharedMesh;
        return mesh;
    }

    private void UpdateCustomizationInfo()
    {
        // Skin Color
        skinColor = GetColor(bodyMeshTransform);

        // Hair Color and Style
        hairColor = GetColor(hairMeshTransform);
        hairStyle = GetMesh(hairMeshTransform);

        // Fur Color and Style
        if (this.sex == Sex.Female) return;
        
        Color zero = default(Color);
        Color beardColor = GetColor(beardMeshTransform);
        Color moustacheColor = GetColor(moustacheMeshTransform);
        furColor = (beardColor != zero) ? beardColor : (moustacheColor != zero) ? moustacheColor : zero;

        beardStyle = GetMesh(beardMeshTransform);
        moustacheStyle = GetMesh(moustacheMeshTransform);
    }

    public CustomizationInfo GetCustomizationInfo()
    {
        UpdateCustomizationInfo();
        return new CustomizationInfo(
            this.sex,
            this.skinColor, this.hairColor, this.furColor,
            this.hairStyle, this.beardStyle, this.moustacheStyle
        );
    }

    public void SetColor(Transform meshTransform, Color newColor)
    {
        SkinnedMeshRenderer[] skinnedMeshRenderers = meshTransform.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            if (renderer.material == null) continue;
            renderer.material.color = newColor;
        }

        UpdateCustomizationInfo();
    }

    public void SetMesh(Transform meshTransform, Mesh newMesh)
    {   
        SkinnedMeshRenderer[] skinnedMeshRenderers = meshTransform.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            bool shouldActivate = (newMesh != null && CheckMeshEquals(renderer.sharedMesh, newMesh));
            renderer.gameObject.SetActive(shouldActivate);
        }
        
        UpdateCustomizationInfo();
    }

    private bool CheckMeshEquals(Mesh first, Mesh second)
    {
        if (first == null || second == null) return false;
        if (first == second) return true;

        if (first.vertexCount != second.vertexCount) return false;
        if (!(first.vertices).SequenceEqual(second.vertices)) return false;
        if (!(first.triangles).SequenceEqual(second.triangles)) return false;
        
        return true;
    }

    public void SetCustomizationInfo(CustomizationInfo info)
    {
        // Set Mesh first and Color later
        SetColor(bodyMeshTransform, info.skinColor);

        SetMesh(hairMeshTransform, info.hairStyle);
        SetColor(hairMeshTransform, info.hairColor);

        if (this.sex == Sex.Female) return;
        
        SetMesh(beardMeshTransform, info.beardStyle);
        SetColor(beardMeshTransform, info.furColor);
        
        SetMesh(moustacheMeshTransform, info.moustacheStyle);
        SetColor(moustacheMeshTransform, info.furColor);
    }
}
