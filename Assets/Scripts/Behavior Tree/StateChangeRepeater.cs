using UnityEngine;
using System.Collections; // Necessary for IEnumerator and Coroutine
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Repeats the execution of its child task when the global variable 'currentState' changes from the previous value.")]
[TaskIcon("{SkinColor}RepeaterIcon.png")]
public class StateChangeRepeater : Decorator
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The number of times to repeat the execution of its child task")]
    public SharedInt count = 1;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Allows the repeater to repeat forever")]
    public SharedBool repeatForever;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Should the task return if the child task returns a failure")]
    public SharedBool endOnFailure;

    private SharedInt globalCurrentState;
    private int previousState;
    private int executionCount;
    private TaskStatus executionStatus = TaskStatus.Inactive;
    private bool stateChanged = false;

    public override void OnAwake()
    {
        // Get the global variable
        globalCurrentState = (SharedInt)GlobalVariables.Instance.GetVariable("currentState");
        // Initialize previousState with the current global state
        previousState = globalCurrentState.Value;
    }

    public override void OnStart()
    {
        // Start the coroutine to check for state changes
        StartCoroutine(CheckStateChange());
    }

    private IEnumerator CheckStateChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
            if (globalCurrentState.Value != previousState)
            {
                stateChanged = true;
                previousState = globalCurrentState.Value;
            }
        }
    }

    public override bool CanExecute()
    {
        if (stateChanged)
        {
            // Reset stateChanged to false to wait for the next state change
            stateChanged = false;
            executionCount = 0; // Reset the execution count for the new state
        }

        // Continue executing until we've reached the count or the child task returned failure and we should stop on a failure.
        return (repeatForever.Value || executionCount < count.Value) && (!endOnFailure.Value || (endOnFailure.Value && executionStatus != TaskStatus.Failure));
    }

    public override void OnChildExecuted(TaskStatus childStatus)
    {
        // The child task has finished execution. Increase the execution count and update the execution status.
        executionCount++;
        executionStatus = childStatus;
    }

    public override void OnEnd()
    {
        // Reset the variables back to their starting values.
        executionCount = 0;
        executionStatus = TaskStatus.Inactive;
    }

    public override void OnReset()
    {
        // Reset the public properties back to their original values.
        count = 1;
        endOnFailure = false;
        repeatForever = false;
        globalCurrentState = null;
        previousState = 0;
        stateChanged = false;
    }
}
