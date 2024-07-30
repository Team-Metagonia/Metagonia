using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Evade the target specified using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("cecc9277e75f9964e98d167be763695c", "992feefbe2d39f945b808bed5b4f0986")]
    public class Evade_nara : NavMeshMovement
    {
        [Tooltip("The agent has evaded when the magnitude is greater than this value")]
        [UnityEngine.Serialization.FormerlySerializedAs("evadeDistance")]
        public SharedFloat m_EvadeDistance = 10;
        [Tooltip("The distance to look ahead when evading")]
        [UnityEngine.Serialization.FormerlySerializedAs("lookAheadDistance")]
        public SharedFloat m_LookAheadDistance = 5;
        [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
        [UnityEngine.Serialization.FormerlySerializedAs("targetDistPrediction")]
        public SharedFloat m_TargetDistPrediction = 20;
        [Tooltip("Multiplier for predicting the look ahead distance")]
        [UnityEngine.Serialization.FormerlySerializedAs("targetDistPredictionMult")]
        public SharedFloat m_TargetDistPredictionMult = 20;
        [Tooltip("The GameObject that the agent is evading")]
        [UnityEngine.Serialization.FormerlySerializedAs("target")]
        public SharedGameObject m_Target;
        [Tooltip("The maximum number of interations that the position should be set")]
        [UnityEngine.Serialization.FormerlySerializedAs("maxInterations")]
        public int m_MaxInterations = 1;

        // The position of the target at the last frame
        private Vector3 m_TargetPosition;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Vector3 previousPosition;
        private float idleThreshold = 0.1f;
        private float positionUpdateInterval = 0.1f;
        private float nextPositionUpdateTime = 0;
        private float nextRandomMoveTime = 0;
        private float randomMoveInterval = 5.0f;
        
        private float fixedSpeed = 3.0f;

        public override void OnStart()
        {
            base.OnStart();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            navMeshAgent.speed = fixedSpeed;
            navMeshAgent.angularSpeed = 500.0f;
            navMeshAgent.acceleration = 20.0f;

            m_TargetPosition = m_Target.Value.transform.position;
            previousPosition = transform.position;
            if (m_MaxInterations == 0) {
                Debug.LogWarning("Error: Max iterations must be greater than 0");
                m_MaxInterations = 1;
            }
        }

        public override TaskStatus OnUpdate()
        {
            navMeshAgent.speed = fixedSpeed;

            if (Vector3.Magnitude(transform.position - m_Target.Value.transform.position) <= m_EvadeDistance.Value) {
                Evade();
            } else {
                RandomMove();
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

        private void Evade()
        {
            if (navMeshAgent.isStopped) {
                navMeshAgent.isStopped = false;
            }

            if (navMeshAgent.velocity.sqrMagnitude == 0) {
                navMeshAgent.velocity = (navMeshAgent.destination - navMeshAgent.transform.position).normalized * navMeshAgent.speed;
            }

            var interation = 0;
            while (!SetDestination(Target(interation)) && interation < m_MaxInterations - 1) {
                interation++;
            }
        }

        private void RandomMove()
        {
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
            return transform.position + (transform.position - position).normalized * m_LookAheadDistance.Value * ((m_MaxInterations - iteration) / m_MaxInterations);
        }

        private bool SetDestination(Vector3 targetPosition)
        {
            if (navMeshAgent == null || !navMeshAgent.isOnNavMesh) {
                return false;
            }
            navMeshAgent.isStopped = false;
            bool result = navMeshAgent.SetDestination(targetPosition);
            return result;
        }

        public override void OnReset()
        {
            base.OnReset();
            m_EvadeDistance = 10;
            m_LookAheadDistance = 5;
            m_TargetDistPrediction = 20;
            m_TargetDistPredictionMult = 20;
            m_Target = null;
        }
    }
}
