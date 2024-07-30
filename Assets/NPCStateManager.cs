using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateManager : MonoBehaviour
{
    private static NPCStateManager instance = null;
    public static NPCStateManager Instance
    {
        get { return instance; }
    }

    public List<NPCStateChanger> stateChangers = new List<NPCStateChanger>();

    public bool _isActive;
    public bool _lastActiveState;

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

    void SetCommandBoolean(bool isActive)
    {
        foreach (NPCStateChanger c in stateChangers) 
        {
            c.isChangeable = isActive;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Two))
        {
            _isActive = !_isActive;
        }

        // Guaranteed to be called Only Once.
        // Call Event or Action in here
        if(_lastActiveState!=_isActive)
        {
            _lastActiveState = _isActive;
            SetCommandBoolean(_isActive);
            Debug.Log(_isActive);
        }
    }
}
