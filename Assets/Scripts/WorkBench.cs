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

    public static UnityAction<Item,Item> OnAttach;

    [SerializeField] GameObject currentLeftObject => OVRBrain.Instance.LeftHandObject;
    [SerializeField] GameObject currentRightObject => OVRBrain.Instance.RightHandObject;

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
            OnWorkStateChange?.Invoke(isWorkable);
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

    bool CheckValidness(GameObject g, GameObject gg)
    {
        if (g == null || gg == null) return false;
        else
        {
            return CraftManager.Instance.CheckRecipe(g.GetComponent<Item>(), gg.GetComponent<Item>());
        }
    }

    // Update is called once per frame
    // Check whether workbench is workable - If you are inside the area and holding valid items in both hands 
    void Update()
    {
        bool isBothHandGrab = ActiveState.Active;

        bool isActive = isBothHandGrab && CheckValidness(currentLeftObject,currentRightObject) && isWorkable;

        Debug.Log(isActive);

        if (_lastActiveValue != isActive)
        {
            Debug.Log(isActive);
            _lastActiveValue = isActive;
            OnWorkStateChange?.Invoke(isActive);
        }
    }
}
