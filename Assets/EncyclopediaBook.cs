using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine.Events;

public class EncyclopediaBook : MonoBehaviour
{
    private Vector3 _initPos;
    private Quaternion _initRot;
    private bool _interactionMode = false;
    private Vector3 _interactionPos;
    private Quaternion _interactionRot;

    public Transform _interactionTarget;
    [SerializeField] GameObject _interactionButton;
    [SerializeField] Transform _buttonParent;
    [SerializeField] HandGrabInteractor _leftinteractor;
    [SerializeField] HandGrabInteractor _rightinteractor;

    [SerializeField] GameObject leftCanvas;
    [SerializeField] GameObject rightCanvas;

    public UnityEvent OnCloseInteractionMode;
    // Start is called before the first frame update
    void Start()
    {
        _initPos = transform.localPosition;
        _initRot = transform.localRotation;
        _interactionPos = _interactionTarget.localPosition;
        _interactionRot = _interactionTarget.localRotation;
        Debug.Log($"initPosition : {_initPos}");
        Debug.Log($"initRotation : {_initRot}");

        EncyclopediaManager.Instance.OnItemUpdate.AddListener(UpdateUI);

        OnCloseInteractionMode.AddListener(CloseBookInteractionMode);
    }

    public void RotateCanvasLeftHand()
    {

        gameObject.GetComponentInChildren<RectTransform>().localEulerAngles = new Vector3(94, 30, -60);

    }

    public void RotateCanvasRightHand()
    {

        gameObject.GetComponentInChildren<RectTransform>().localEulerAngles = new Vector3(86.91f, 210, -60);


    }


    [ContextMenu("Reset Book")]
    public void ResetBook()
    {
        if (_interactionMode) return;
        StartCoroutine(ResetCoroutine());
    }

    IEnumerator ResetCoroutine()
    {
        
        yield return new WaitForEndOfFrame();
        Debug.Log("ResetBook");
        transform.localPosition = _initPos;
        transform.localRotation = _initRot;
    }

    public void OnInteractionModeChange()
    {
        _interactionMode = !_interactionMode;
        if (_interactionMode)
        {
            StartCoroutine(OpenBookInteractionMode());
        }
        else OnCloseInteractionMode?.Invoke();
            
    }

    [ContextMenu("OpenBook")]
    IEnumerator OpenBookInteractionMode()
    {
        _interactionMode = true;
        _leftinteractor.ForceRelease();
        _rightinteractor.ForceRelease();

        yield return new WaitForEndOfFrame();

        transform.localPosition = _interactionPos;
        transform.localRotation = _interactionRot;

    }
    [ContextMenu("CloseBook")]
    void CloseBookInteractionMode()
    {
        _interactionMode = false;
        ResetBook();
    }

    public void UpdateUI(ItemSO iteminfo)
    {
        GameObject obj = Instantiate(_interactionButton, _buttonParent);
        obj.GetComponent<Image>().sprite = iteminfo.icon;
        obj.GetComponent<Button>().onClick.AddListener(OnInteractionModeChange);
        obj.GetComponent<EncyclopediaHolder>().EncyclopediaObj = iteminfo.encyclopediaPrefab;
    }

    
}
