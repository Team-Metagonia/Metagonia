using UnityEngine;
using Oculus.Interaction;

public class ToggleSelectionOnRayInteractableSelect : MonoBehaviour
{
    public RayInteractor rayInteractor; // Reference to the Ray Interactor
    public OVRInput.Button toggleButton = OVRInput.Button.One; // A button
    private RayInteractable lastInteractable;
    private bool buttonPressed = false;

    void Update()
    {
        if (rayInteractor == null)
        {
            Debug.LogError("RayInteractor is not assigned.");
            return;
        }

        // Check the currently selected Interactable
        RayInteractable selectedInteractable = rayInteractor.Candidate as RayInteractable;

        // 인터랙터블이 null이 아니고 이전에 선택된 인터랙터블과 다를 때만 처리
        if (selectedInteractable != null && selectedInteractable != lastInteractable)
        {
            Debug.Log("Selected Interactable: " + selectedInteractable.gameObject.name);
            lastInteractable = selectedInteractable;
        }
        else if (selectedInteractable == null)
        {
            lastInteractable = null;
            Debug.Log("No interactable selected.");
        }

        // OVR의 A 버튼을 누를 때만 isSelected 토글
        if (selectedInteractable != null && OVRInput.Get(toggleButton) && !buttonPressed)
        {
            buttonPressed = true; // 버튼이 눌렸음을 기록
            Debug.Log("A button pressed.");
            ToggleSelection(selectedInteractable.gameObject);
        }

        // 버튼이 눌렸다가 떼어졌을 때 상태를 초기화
        if (OVRInput.GetUp(toggleButton))
        {
            buttonPressed = false; // 버튼이 떼어졌음을 기록
        }
    }

    private void ToggleSelection(GameObject hitObject)
    {
        // hitObject의 부모 오브젝트를 찾음
        GameObject parentObject = hitObject.transform.parent ? hitObject.transform.parent.gameObject : hitObject;

        Debug.Log("Parent Object: " + parentObject.name);

        SpearNPC spearNPC = parentObject.GetComponent<SpearNPC>();
        if (spearNPC != null)
        {
            spearNPC.ToggleSelection();
            Debug.Log("isSelected toggled for: " + parentObject.name + " to " + spearNPC.isSelected);
        }
        else
        {
            Debug.LogWarning("The hit object or its parent does not have a SpearNPC component: " + parentObject.name);
        }
    }
}
