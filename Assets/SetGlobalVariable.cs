using UnityEngine;
using BehaviorDesigner.Runtime;

public class SetGlobalVariable : MonoBehaviour
{
    public GameObject targetObject; // The object to set as the global variable
    public string globalVariableName; // The name of the global variable

    void Start()
    {
        // Check if the global variable exists before setting it
        SharedGameObject sharedGameObject = GlobalVariables.Instance.GetVariable(globalVariableName) as SharedGameObject;
        if (sharedGameObject != null)
        {
            // Set the global variable's value to the target object
            sharedGameObject.Value = targetObject;
            Debug.Log($"Global variable '{globalVariableName}' set to {targetObject.name}");
        }
        else
        {
            Debug.LogError($"Global variable '{globalVariableName}' not found.");
        }
    }
}