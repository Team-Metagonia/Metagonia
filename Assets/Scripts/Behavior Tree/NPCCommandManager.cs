using UnityEngine;
using BehaviorDesigner.Runtime;

public class NPCCommandManager : MonoBehaviour
{
    public BehaviorTree behaviorTree;
    private int previousState;

    void Start()
    {
        // 초기 상태 설정
        previousState = (int)behaviorTree.GetVariable("currentState").GetValue();
    }

    void Update()
    {
        int currentState = previousState;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentState = 1;
            Debug.Log("[NPCCommandManager] Update: currentState set to 1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentState = 2;
            Debug.Log("[NPCCommandManager] Update: currentState set to 2");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentState = 3;
            Debug.Log("[NPCCommandManager] Update: currentState set to 3");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentState = 4;
            Debug.Log("[NPCCommandManager] Update: currentState set to 4");
        }

        // 상태가 변경되었을 때만 Behavior Tree 업데이트
        if (currentState != previousState)
        {
            behaviorTree.SetVariableValue("currentState", currentState);
            behaviorTree.SendEvent("UpdateState"); // 이벤트 보내기
            previousState = currentState;
        }
    }
}