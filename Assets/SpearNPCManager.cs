using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;

public class SpearNPCManager : MonoBehaviour
{
    public GameObject npcParent; // NPC들이 모여있는 부모 오브젝트
    public string enemyTargetName = "Enemy"; // 인스펙터에서 받은 문자열
    private List<SpearNPC> selectedNPCs = new List<SpearNPC>();
    private SpearNPC leader;

    void Awake()
    {
        // 초기화 시점에 자식 오브젝트들만 찾아 리스트를 업데이트합니다.
        UpdateSelectedNPCs();
        SetLeader();
    }

    void Update()
    {
        // 매 프레임마다 자식 오브젝트들을 다시 검색하여 업데이트합니다.
        UpdateSelectedNPCs();
        SetLeader();
    }

    private void UpdateSelectedNPCs()
    {
        // 자식 오브젝트들만 찾아 리스트를 업데이트합니다.
        selectedNPCs.Clear();
        SpearNPC[] allNPCs = npcParent.GetComponentsInChildren<SpearNPC>();
        foreach (var npc in allNPCs)
        {
            if (npc.isSelected)
            {
                selectedNPCs.Add(npc);
            }
        }
    }

    private void SetLeader()
    {
        if (selectedNPCs.Count > 0)
        {
            leader = selectedNPCs[0];
            Debug.Log("Leader is set to: " + leader.gameObject.name);
        }
        else
        {
            leader = null;
            Debug.Log("No leader selected.");
        }

        UpdateBehaviorTreeVariables();
    }

    private void UpdateBehaviorTreeVariables()
    {
        foreach (var npc in npcParent.GetComponentsInChildren<SpearNPC>())
        {
            var behaviorTrees = npc.GetComponents<BehaviorTree>();
            foreach (var behaviorTree in behaviorTrees)
            {
                if (behaviorTree != null)
                {
                    bool hasLeaderVariable = HasVariable(behaviorTree, "Leader");
                    bool hasEnemyTargetVariable = HasVariable(behaviorTree, "EnemyTarget");

                    if (hasLeaderVariable || hasEnemyTargetVariable)
                    {
                        if (npc == leader)
                        {
                            if (hasLeaderVariable)
                                behaviorTree.SetVariableValue("Leader", (GameObject)null);
                            if (hasEnemyTargetVariable)
                                behaviorTree.SetVariableValue("EnemyTarget", enemyTargetName);
                        }
                        else
                        {
                            if (hasLeaderVariable)
                                behaviorTree.SetVariableValue("Leader", leader != null ? leader.gameObject : null);
                            if (hasEnemyTargetVariable)
                                behaviorTree.SetVariableValue("EnemyTarget", (string)null);
                        }
                    }
                }
            }
        }
    }

    private bool HasVariable(BehaviorTree behaviorTree, string variableName)
    {
        try
        {
            var variable = behaviorTree.GetVariable(variableName);
            return variable != null;
        }
        catch
        {
            return false;
        }
    }
}
