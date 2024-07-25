using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectionMode
{
    Sex, Style
}

[RequireComponent(typeof(Animator))]
public class CharacterCustomization : MonoBehaviour
{
    private Animator animator;
    private SelectionMode selectionMode;

    private Object selectionLock = new Object();
    private bool isSexSelectionFinished = false;
    private bool quit = false;

    public CharacterCustomizationManager customizationManager;
    public Transform bodyMeshTransform;
    public Transform hairMeshTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        EnterSexSelection();
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        customizationManager.OnSexSelectionFinished += SexSelectionFinished;
    }

    private void OnDisable()
    {
        if (quit) return;

        customizationManager.OnSexSelectionFinished -= SexSelectionFinished;
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
}
