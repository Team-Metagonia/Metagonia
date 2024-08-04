using UnityEngine;
using BehaviorDesigner.Runtime;

public class NewNPCManager : MonoBehaviour
{
    private int previousState;

    void Start()
    {
        // 초기 상태 설정
        previousState = (int)GlobalVariables.Instance.GetVariable("currentState").GetValue();
    }

    void Update()
    {
        int currentState = previousState;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentState = 1; // 따라와
            Debug.Log("[NPCCommandManager] Update: currentState set to Follow");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentState = 2; // 앉아
            Debug.Log("[NPCCommandManager] Update: currentState set to Crouch");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentState = 3; // 일어서
            Debug.Log("[NPCCommandManager] Update: currentState set to StandUp");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentState = 4; // 멈춰
            Debug.Log("[NPCCommandManager] Update: currentState set to Stop");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentState = 5; // 호위
            Debug.Log("[NPCCommandManager] Update: currentState set to Cover");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentState = 6; // 공격
            Debug.Log("[NPCCommandManager] Update: currentState set to Attack");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentState = 7; // 해산
            Debug.Log("[NPCCommandManager] Update: currentState set to Dismiss");
        }

        // 상태가 변경되었을 때만 글로벌 변수 업데이트
        if (currentState != previousState)
        {
            GlobalVariables.Instance.SetVariableValue("currentState", currentState);
            previousState = currentState;
        }
    }
}