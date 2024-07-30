using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCustomizationManager : MonoBehaviour
{
    public Action<CharacterCustomization> OnSexSelectionFinished;

    public OVRCameraRig cameraRig;
    public Canvas customizationCanvas;
    public CustomizationPanelController panelController;

    [HideInInspector]
    public CharacterCustomization selectedCharacter;

    private float durationOffset = 2f;
    private float cameraMovingDuration = 3f;

    public CustomizationInfo customizationInfo;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        DisableCustomizationCanvas();
    }

    public void SexSelectionFinished(CharacterCustomization customCharacter)
    {
        selectedCharacter = customCharacter;
        OnSexSelectionFinished?.Invoke(customCharacter);

        Vector3 cameraPositionAfterSelected = GenerateCameraPositionAfterSelected();
        StartCoroutine(MoveCamera(cameraPositionAfterSelected, cameraMovingDuration, durationOffset));

        EnableCutomizationCanvas();
        InitializePanelController();
    }

    private Vector3 GenerateCameraPositionAfterSelected()
    {
        if (selectedCharacter == null)
        {
            Debug.LogError("There is no selected character");
            return Vector3.zero;
        }

        float dist = 1.5f;
        Transform characterTransform = selectedCharacter.gameObject.transform;
        Vector3 origin = characterTransform.position;
        Vector3 adj = characterTransform.forward * dist;
        Vector3 targetPos = origin + adj;
        targetPos.y = cameraRig.transform.position.y;

        return targetPos;
    }

    private IEnumerator MoveCamera(Vector3 targetPos, float duration, float durationOffset = 0f)
    {
        yield return new WaitForSeconds(durationOffset);
        
        Vector3 startPos = cameraRig.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            cameraRig.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    
        // Set Exact Position
        cameraRig.transform.position = targetPos;
    }

    private void EnableCutomizationCanvas()
    {
        customizationCanvas.gameObject.SetActive(true);
    }
    
    private void DisableCustomizationCanvas()
    {
        customizationCanvas.gameObject.SetActive(false);
    }

    private void InitializePanelController()
    {
        panelController.InjectCustomCharacter(selectedCharacter);
        panelController.Initialize(selectedCharacter);
    }

    public void CompleteCustomization()
    {
        customizationInfo = selectedCharacter.GetCustomizationInfo();
        SceneManager.LoadScene("Final");
    }
}
