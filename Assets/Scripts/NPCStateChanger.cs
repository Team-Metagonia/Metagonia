using UnityEngine;
using BehaviorDesigner.Runtime;

public class NPCStateChanger : MonoBehaviour
{
    [Tooltip("The state to set when the event is triggered.")]
    public int stateToSet;

    public void SetState()
    {
        // Set the global variable "currentState" to the specified state
        GlobalVariables.Instance.SetVariableValue("currentState", stateToSet);
        Debug.Log($"[NPCStateChanger] SetState: currentState set to {stateToSet}");
    }
}