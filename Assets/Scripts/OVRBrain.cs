using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRBrain : MonoBehaviour
{
    private static OVRBrain instance = null;
    public static OVRBrain Instance
    {
        get
        {
            if (instance == null) return null;
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public GameObject LeftHandObject;
    public GameObject RightHandObject;

}
