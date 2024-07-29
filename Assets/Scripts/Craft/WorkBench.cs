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
    bool isEntered = false;

    [Tooltip("Boolean to check if player is in range")]
    public bool isWorkable = false;

    [Tooltip("WorkBench Canvas GameObject")]
    public GameObject workBenchUI;

    [Tooltip("Position of Instantiated Material Items")]
    public Transform itemSpawnPoint;

    [Tooltip("Respawn trigger object")]
    public GameObject respawnObj;

    

    public static UnityAction<bool> OnWorkStateChange;

    public static UnityAction<Item,Item> OnAttach;

    public static UnityAction<bool> OnWorkBenchEnter;

    [SerializeField] GameObject currentLeftObject => OVRBrain.Instance.LeftHandObject;
    [SerializeField] GameObject currentRightObject => OVRBrain.Instance.RightHandObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !isEntered)
        {
            Debug.Log("Is Workable");
            isEntered = true;
            isWorkable = true;
            //workBenchUI.SetActive(true);
            OnWorkBenchEnter?.Invoke(isEntered);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Is Not Workable");
            isEntered = false;
            isWorkable = false;
            //workBenchUI.SetActive(false);
            OnWorkBenchEnter?.Invoke(isEntered);
            OnWorkStateChange?.Invoke(isWorkable);
        }
    }

    void Awake()
    {
        ActiveState = _activeState as IActiveState;
        isWorkable = ActiveState.Active;
        workBenchUI.gameObject.SetActive(false);
        OnWorkBenchEnter += test;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void test(bool isActive)
    {
        Debug.Log($"Entered WorkBench Area : {isActive}");
        workBenchUI.SetActive(isActive);
        respawnObj.SetActive(isActive);
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
