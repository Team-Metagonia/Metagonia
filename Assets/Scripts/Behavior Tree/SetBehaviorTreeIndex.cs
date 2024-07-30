using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Sets the index of the Behavior Tree to enable in the BehaviorTreeManager.")]
[TaskCategory("Custom")]
public class SetBehaviorTreeIndex : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject with the BehaviorTreeManager component")]
    public SharedGameObject behaviorTreeManagerGameObject;

    [BehaviorDesigner.Runtime.Tasks.Tooltip("The index of the Behavior Tree to enable")]
    public SharedInt treeIndex;

    private BehaviorTreeManager behaviorTreeManager;

    public override void OnStart()
    {
        // Get the BehaviorTreeManager component
        behaviorTreeManager = behaviorTreeManagerGameObject.Value.GetComponent<BehaviorTreeManager>();
        if (behaviorTreeManager == null)
        {
            Debug.LogError("BehaviorTreeManager component not found on " + behaviorTreeManagerGameObject.Value.name);
        }
        else
        {
            Debug.Log("BehaviorTreeManager component found on " + behaviorTreeManagerGameObject.Value.name);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (behaviorTreeManager == null)
        {
            Debug.LogError("BehaviorTreeManager is null.");
            return TaskStatus.Failure;
        }

        Debug.Log("Setting Behavior Tree index to " + treeIndex.Value);
        // Disable all behavior trees and enable the selected behavior tree
        behaviorTreeManager.DisableAllBehaviorTrees();
        behaviorTreeManager.SetBehaviorTree(treeIndex.Value);
        return TaskStatus.Success;
    }

    
}