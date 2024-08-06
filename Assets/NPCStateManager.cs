using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCStateManager : MonoBehaviour
{
    private static NPCStateManager instance = null;
    public static NPCStateManager Instance
    {
        get { return instance; }
    }

    public List<NPCStateChanger> stateChangers = new List<NPCStateChanger>();

    public UnityEvent<bool> OnCommandState;

    public GameObject commandObject;
    public GameObject poseObject;

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
        commandObject.SetActive(isActive);
        poseObject.SetActive(isActive);
    }

    void SetCommandHelper(bool isActive)
    {
        commandObject.SetActive(isActive);
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
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
