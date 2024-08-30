using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformsDieWithMe : MonoBehaviour
{
    public Transform[] transformsDyingWithMe;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        foreach (Transform who in transformsDyingWithMe)
        {
            GameObject go = who.gameObject;
            Destroy(go);
        }
    }
}
