using UnityEngine;
using UnityEngine.AI;

public class DisableObstacleAvoidance : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance; // 회피 비활성화
    }
}
