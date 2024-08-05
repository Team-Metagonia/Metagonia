using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Approach the target specified using the Unity NavMesh and attack when in range.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("cecc9277e75f9964e98d167be763695c", "992feefbe2d39f945b808bed5b4f0986")]
    public class ApproachAndAttack : NavMeshMovement 
    {
        [Tooltip("The agent has approached when the magnitude is less than this value")]
        public SharedFloat m_ApproachDistance = 10; // 접근 거리 설정
        [Tooltip("The distance to look ahead when approaching")]
        public SharedFloat m_LookAheadDistance = 5;
        [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
        public SharedFloat m_TargetDistPrediction = 20;
        [Tooltip("Multiplier for predicting the look ahead distance")]
        public SharedFloat m_TargetDistPredictionMult = 20;
        [Tooltip("The GameObject that the agent is approaching")]
        public SharedGameObject m_Target;
        [Tooltip("The maximum number of interations that the position should be set")]
        public int m_MaxInterations = 1;
        [Tooltip("The distance within which the agent attacks the target")]
        public SharedFloat m_AttackDistance = 5; // 공격 거리 설정

        private Vector3 m_TargetPosition;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Vector3 previousPosition;
        private float idleThreshold = 0.1f;
        private float positionUpdateInterval = 0.1f;
        private float nextPositionUpdateTime = 0;
        private float nextRandomMoveTime = 0;
        private float randomMoveInterval = 5.0f;
        private float fixedSpeed = 3.0f; // 기본 속도
        private float approachSpeed = 6.0f; // 접근 시 속도
        private bool isAttacking = false;

        public override void OnStart()
        {
            base.OnStart();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            navMeshAgent.speed = fixedSpeed;
            navMeshAgent.angularSpeed = 500.0f;
            navMeshAgent.acceleration = 50.0f; // 가속도 증가
            navMeshAgent.autoBraking = false; // autoBraking 비활성화

            m_TargetPosition = m_Target.Value.transform.position;
            previousPosition = transform.position;
            if (m_MaxInterations == 0) {
                Debug.LogWarning("Error: Max iterations must be greater than 0");
                m_MaxInterations = 1;
            }
        }

        public override TaskStatus OnUpdate()
        {
            float distanceToTarget = Vector3.Magnitude(transform.position - m_Target.Value.transform.position);
            Debug.Log("Distance to target: " + distanceToTarget); // 디버그 메시지 추가

            if (distanceToTarget <= m_AttackDistance.Value) {
                Attack(); // 공격 동작
            } else if (distanceToTarget <= m_ApproachDistance.Value) {
                Approach(); // 접근 동작
            } else {
                RandomMove(); // 타겟이 없을 때 이동
            }

            if (Time.time >= nextPositionUpdateTime) {
                nextPositionUpdateTime = Time.time + positionUpdateInterval;

                if (Vector3.Distance(previousPosition, transform.position) > idleThreshold) {
                    animator.SetBool("isWalking", true);
                } else {
                    animator.SetBool("isWalking", false);
                }

                previousPosition = transform.position;
            }

            return TaskStatus.Running;
        }

        private void Approach()
        {
            if (navMeshAgent.isStopped) {
                navMeshAgent.isStopped = false;
            }

            navMeshAgent.speed = approachSpeed; 
            animator.SetBool("isRunning", true); 
            animator.SetBool("isWalking", false); 
            animator.SetBool("isAttacking", false); 

            Vector3 targetPosition = m_Target.Value.transform.position;

            if (SetDestination(targetPosition)) {
                Debug.Log("Approaching target: " + targetPosition);
                navMeshAgent.SetDestination(targetPosition);
            } else {
                Debug.LogWarning("Failed to set destination for approach.");
            }

            // 강제 업데이트
            navMeshAgent.isStopped = false;
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = true;

            // 추가 디버그 정보
            Debug.Log("NavMeshAgent velocity: " + navMeshAgent.velocity);
            Debug.Log("NavMeshAgent remainingDistance: " + navMeshAgent.remainingDistance);
            Debug.Log("NavMeshAgent pathPending: " + navMeshAgent.pathPending);
            Debug.Log("NavMeshAgent isPathStale: " + navMeshAgent.isPathStale);
            Debug.Log("NavMeshAgent pathStatus: " + navMeshAgent.pathStatus);
        }

        private void Attack()
        {
            navMeshAgent.speed = fixedSpeed; 
            if (!isAttacking) {
                isAttacking = true;
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", true);
                navMeshAgent.isStopped = true;
                StartCoroutine(ResetAttack(1.0f)); 
            }
        }

        private System.Collections.IEnumerator ResetAttack(float delay)
        {
            yield return new WaitForSeconds(delay);
            isAttacking = false;
            animator.SetBool("isAttacking", false); // 공격 애니메이션 종료
            navMeshAgent.isStopped = false; // 공격이 끝나면 다시 움직이기
        }

        private void RandomMove()
        {
            navMeshAgent.speed = fixedSpeed; // 타겟이 없을 때 고정 속도 유지
            animator.SetBool("isRunning", false); // 걷기 애니메이션 사용
            if (Time.time >= nextRandomMoveTime || navMeshAgent.remainingDistance <= 0.5f) {
                nextRandomMoveTime = Time.time + randomMoveInterval;

                Vector3 randomDirection = Random.insideUnitSphere * m_LookAheadDistance.Value * 10;
                randomDirection += transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, m_LookAheadDistance.Value * 10, NavMesh.AllAreas)) {
                    navMeshAgent.SetDestination(hit.position);
                }
            }
        }

        private Vector3 Target(int iteration)
        {
            var distance = (m_Target.Value.transform.position - transform.position).magnitude;
            var speed = Velocity().magnitude;

            float futurePrediction = 0;
            if (speed <= distance / m_TargetDistPrediction.Value) {
                futurePrediction = m_TargetDistPrediction.Value;
            } else {
                futurePrediction = (distance / speed) * m_TargetDistPredictionMult.Value;
            }

            var prevTargetPosition = m_TargetPosition;
            m_TargetPosition = m_Target.Value.transform.position;
            var position = m_TargetPosition + (m_TargetPosition - prevTargetPosition) * futurePrediction;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas)) {
                position = hit.position; 
            }
            return transform.position + (position - transform.position).normalized * m_LookAheadDistance.Value * ((m_MaxInterations - iteration) / m_MaxInterations);
        }

        private bool SetDestination(Vector3 targetPosition)
        {
            if (navMeshAgent == null || !navMeshAgent.isOnNavMesh) {
                return false;
            }
            navMeshAgent.isStopped = false;
            bool result = navMeshAgent.SetDestination(targetPosition);
            Debug.Log("SetDestination result: " + result + " for position: " + targetPosition);
            return result;
        }

        public override void OnReset()
        {
            base.OnReset();
            m_ApproachDistance = 10;
            m_LookAheadDistance = 5;
            m_TargetDistPrediction = 20;
            m_TargetDistPredictionMult = 20;
            m_Target = null;
        }
    }
}
