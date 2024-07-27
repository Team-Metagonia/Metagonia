using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    protected bool isOpen;
    public bool IsOpen { get { return isOpen; } }

    // Enter the scene with the menus on!!!
    
    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        isOpen = false;
        this.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {

    }

    public virtual void Open()
    {
        isOpen = true;
        this.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        isOpen = false;
        this.gameObject.SetActive(false);
    }

    public virtual void Toggle()
    {
        if (isOpen) Close();
        else Open();
    }
}
