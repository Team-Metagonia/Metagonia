using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationManager : MonoBehaviour
{
    private static CharacterCustomizationManager instance = null;
    public Action<CharacterCustomization> OnSexSelectionFinished;
    public Action OnCharacterSelected;

    public OVRCameraRig cameraRig;
    public CharacterCustomization selectedCharacter;

    private float durationOffset = 2f;
    private float cameraMovingDuration = 3f;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static CharacterCustomizationManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void SexSelectionFinished(CharacterCustomization customCharacter)
    {
        selectedCharacter = customCharacter;
        OnSexSelectionFinished?.Invoke(customCharacter);

        Vector3 cameraPositionAfterSelected = GenerateCameraPositionAfterSelected();
        StartCoroutine(MoveCamera(cameraPositionAfterSelected, cameraMovingDuration, durationOffset));
        
        // StartCoroutine(
        //     MoveCamera(selectedCharacter.cameraTransformWhenSelected.position, cameraMovingDuration, durationOffset)
        // );
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
}
