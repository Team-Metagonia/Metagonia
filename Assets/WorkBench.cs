using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorkBench : MonoBehaviour
{
    [Tooltip("The IActiveState to debug.")]
    [SerializeField, Interface(typeof(IActiveState))]
    private Object _activeState;
    private IActiveState ActiveState { get; set; }

    private bool _lastActiveValue = false;

    [Tooltip("Boolean to check if player is in range")]
    public bool isWorkable = false;

    public static UnityAction<bool> OnWorkStateChange;

    public static UnityAction<item,item> OnAttach;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isWorkable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isWorkable = false;
        }
    }

    void Awake()
    {
        ActiveState = _activeState as IActiveState;
        isWorkable = ActiveState.Active;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // Check whether workbench is workable - If you are inside the area and holding items in both hands 
    void Update()
    {
        if (!isWorkable) return;

        bool isActive = ActiveState.Active;

        if (_lastActiveValue != isActive)
        {
            //isWorkable = isActive;
            _lastActiveValue = isActive;
            OnWorkStateChange?.Invoke(isActive);
        }
    }
}
