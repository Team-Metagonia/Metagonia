using UnityEngine;
using BehaviorDesigner.Runtime;

public class BehaviorTreeManager : MonoBehaviour
{
    private BehaviorTree[] behaviorTrees;
    private int currentIndex = -1;
    private SharedInt globalCurrentState;
    private int previousState;

    void Start()
    {
        // Get all BehaviorTree components attached to this GameObject
        behaviorTrees = GetComponents<BehaviorTree>();

        // Ensure all behavior trees are disabled initially
        for (int i = 0; i < behaviorTrees.Length; i++)
        {
            if (behaviorTrees[i] != null)
            {
                behaviorTrees[i].DisableBehavior();
                Debug.Log($"Behavior Tree at index {i} initialized and disabled.");
            }
            else
            {
                Debug.LogError($"BehaviorTree component is null at index: {i}");
            }
        }

        // Optionally set the first BT as active
        if (behaviorTrees.Length > 0)
        {
            SetBehaviorTree(0);
        }

        // Get the global variable
        globalCurrentState = (SharedInt)GlobalVariables.Instance.GetVariable("currentState");
        previousState = globalCurrentState.Value;
    }

    void Update()
    {
        if (globalCurrentState.Value != previousState)
        {
            // Reset to 0th index behavior tree when currentState changes
            SetBehaviorTree(0);
            previousState = globalCurrentState.Value;
        }
    }

    public void SetBehaviorTree(int index)
    {
        if (index < 0 || index >= behaviorTrees.Length)
        {
            Debug.LogError("Index out of range.");
            return;
        }

        // Disable the current BT if any
        if (currentIndex >= 0 && currentIndex < behaviorTrees.Length)
        {
            Debug.Log($"Disabling Behavior Tree at index: {currentIndex}");
            behaviorTrees[currentIndex].DisableBehavior();
        }

        // Enable the new BT
        Debug.Log($"Enabling Behavior Tree at index: {index}");
        behaviorTrees[index].EnableBehavior();
        currentIndex = index;
    }

    // Method to disable all behavior trees
    public void DisableAllBehaviorTrees()
    {
        for (int i = 0; i < behaviorTrees.Length; i++)
        {
            Debug.Log($"Disabling all Behavior Trees, currently disabling index: {i}");
            behaviorTrees[i].DisableBehavior();
        }
        currentIndex = -1;
    }
}
