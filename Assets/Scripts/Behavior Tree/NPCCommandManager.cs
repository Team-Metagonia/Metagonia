using UnityEngine;
using BehaviorDesigner.Runtime;

public class NPCCommandManager : MonoBehaviour
{
    private int previousState;

    void Start()
    {
        // 초기 상태 설정
        previousState = (int)GlobalVariables.Instance.GetVariable("currentState").GetValue();
        GlobalVariables.Instance.SetVariableValue("IsStateChanged", 0);
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
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentState = 5;
            Debug.Log("[NPCCommandManager] Update: currentState set to 5");
        }

        // 상태가 변경되었을 때만 글로벌 변수 업데이트
        if (currentState != previousState)
        {
            GlobalVariables.Instance.SetVariableValue("currentState", currentState);
            GlobalVariables.Instance.SetVariableValue("IsStateChanged", 1); // 상태 변경 표시
            previousState = currentState;
        }
    }
}