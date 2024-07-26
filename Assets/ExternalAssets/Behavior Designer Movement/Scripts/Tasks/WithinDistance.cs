using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Check to see if the any object specified by the object list or tag is within the distance specified of the current agent.")]
    [TaskCategory("Movement")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("62dc1c328b5c4eb45a90ec7a75cfb747", "0e2ffa7c5e610214eb6d5c71613bbdec")]
    public class WithinDistance : Conditional
    {
        [Tooltip("Should the 2D version be used?")]
        [UnityEngine.Serialization.FormerlySerializedAs("usePhysics2D")]
        public bool m_UsePhysics2D;
        [Tooltip("Specifies the type of detection that should be used.")]
        public SharedDetectionMode m_DetectionMode = DetectionMode.Object | DetectionMode.ObjectList | DetectionMode.Tag | DetectionMode.LayerMask;
        [Tooltip("The object that we are searching for")]
        public SharedGameObject m_TargetObject;
        [Tooltip("The objects that we are searching for")]
        [UnityEngine.Serialization.FormerlySerializedAs("targetObjects")]
        public SharedGameObjectList m_TargetObjects;
        [Tooltip("The tag of the object that we are searching for")]
        public SharedString m_TargetTag;
        [Tooltip("The LayerMask of the objects that we are searching for")]
        public SharedLayerMask m_TargetLayerMask;
        [Tooltip("If using the object layer mask, specifies the maximum number of colliders that the physics cast can collide with")]
        public int m_MaxCollisionCount = 200;
        [Tooltip("The distance that the object needs to be within")]
        public SharedFloat m_Magnitude = 5;
        [Tooltip("If true, the object must be within line of sight to be within distance. For example, if this option is enabled then an object behind a wall will not be within distance even though it may " +
                 "be physically close to the other object")]
        public SharedBool m_LineOfSight;
        [Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
        public LayerMask m_IgnoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        [Tooltip("The raycast offset relative to the pivot position")]
        public SharedVector3 m_Offset;
        [Tooltip("The target raycast offset relative to the pivot position")]
        public SharedVector3 m_TargetOffset;
        [Tooltip("Should a debug look ray be drawn to the scene view?")]
        public SharedBool m_DrawDebugRay;
        [Tooltip("The object variable that will be set when a object is found what the object is")]
        public SharedGameObject m_ReturnedObject;

        private float m_SqrMagnitude; // distance * distance, optimization so we don't have to take the square root
        private Collider[] m_OverlapColliders;
        private Collider2D[] m_Overlap2DColliders;

        public override void OnStart()
        {
            m_SqrMagnitude = m_Magnitude.Value * m_Magnitude.Value;
            Debug.Log($"[WithinDistance] OnStart: m_SqrMagnitude set to {m_SqrMagnitude}");
        }

        public override TaskStatus OnUpdate()
        {
            m_ReturnedObject.Value = null;
            Debug.Log("[WithinDistance] OnUpdate: Started");

            if ((m_DetectionMode.Value & DetectionMode.Object) != 0 && m_TargetObject.Value != null) {
                Debug.Log("[WithinDistance] OnUpdate: Checking single target object");
                if (IsWithinDistance(m_TargetObject.Value)) {
                    m_ReturnedObject.Value = m_TargetObject.Value;
                    Debug.Log("[WithinDistance] OnUpdate: Single target object is within distance");
                }
            }

            if (m_ReturnedObject.Value == null && (m_DetectionMode.Value & DetectionMode.ObjectList) != 0) {
                Debug.Log("[WithinDistance] OnUpdate: Checking target object list");
                for (int i = 0; i < m_TargetObjects.Value.Count; ++i) {
                    if (m_TargetObjects.Value[i] == null || m_TargetObjects.Value[i] == gameObject) {
                        continue;
                    }

                    if (IsWithinDistance(m_TargetObjects.Value[i])) {
                        m_ReturnedObject.Value = m_TargetObjects.Value[i];
                        Debug.Log("[WithinDistance] OnUpdate: An object in the list is within distance");
                        break;
                    }
                }
            }

            if (m_ReturnedObject.Value == null && (m_DetectionMode.Value & DetectionMode.Tag) != 0 && !string.IsNullOrEmpty(m_TargetTag.Value)) {
                Debug.Log("[WithinDistance] OnUpdate: Checking objects with tag");
                var objects = GameObject.FindGameObjectsWithTag(m_TargetTag.Value);
                for (int i = 0; i < objects.Length; ++i) {
                    if (objects[i] == null || objects[i] == gameObject) {
                        continue;
                    }

                    if (IsWithinDistance(objects[i])) {
                        m_ReturnedObject.Value = objects[i];
                        Debug.Log("[WithinDistance] OnUpdate: An object with the specified tag is within distance");
                        break;
                    }
                }
            }

            if (m_ReturnedObject.Value == null && (m_DetectionMode.Value & DetectionMode.LayerMask) != 0) {
                Debug.Log("[WithinDistance] OnUpdate: Checking objects with layer mask");
                if (m_UsePhysics2D) {
                    if (m_Overlap2DColliders == null) {
                        m_Overlap2DColliders = new Collider2D[m_MaxCollisionCount];
                    }
                    var count = Physics2D.OverlapCircleNonAlloc(transform.position, m_Magnitude.Value, m_Overlap2DColliders, m_TargetLayerMask.Value);
                    for (int i = 0; i < count; ++i) {
                        if (IsWithinDistance(m_Overlap2DColliders[i].gameObject)) {
                            m_ReturnedObject.Value = m_Overlap2DColliders[i].gameObject;
                            Debug.Log("[WithinDistance] OnUpdate: A 2D collider is within distance");
                            break;
                        }
                    }
                } else {
                    if (m_OverlapColliders == null) {
                        m_OverlapColliders = new Collider[m_MaxCollisionCount];
                    }
                    var count = Physics.OverlapSphereNonAlloc(transform.position, m_Magnitude.Value, m_OverlapColliders, m_TargetLayerMask.Value);
                    for (int i = 0; i < count; ++i) {
                        if (IsWithinDistance(m_OverlapColliders[i].gameObject)) {
                            m_ReturnedObject.Value = m_OverlapColliders[i].gameObject;
                            Debug.Log("[WithinDistance] OnUpdate: A 3D collider is within distance");
                            break;
                        }
                    }
                }
            }

            if (m_ReturnedObject.Value != null) {
                Debug.Log("[WithinDistance] OnUpdate: Found an object within distance, returning Success");
                return TaskStatus.Success;
            }

            Debug.Log("[WithinDistance] OnUpdate: No objects within distance, returning Failure");
            return TaskStatus.Failure;
        }

        private bool IsWithinDistance(GameObject target)
        {
            var direction = target.transform.position - (transform.position + m_Offset.Value);
            Debug.Log($"[WithinDistance] IsWithinDistance: Checking distance to target {target.name}");

            if (Vector3.SqrMagnitude(direction) < m_SqrMagnitude) {
                if (m_LineOfSight.Value) {
                    var hitTransform = MovementUtility.LineOfSight(transform, m_Offset.Value, target, m_TargetOffset.Value, m_UsePhysics2D, m_IgnoreLayerMask.value, m_DrawDebugRay.Value);
                    if (hitTransform != null && MovementUtility.IsAncestor(hitTransform, target.transform)) {
                        Debug.Log("[WithinDistance] IsWithinDistance: Target is within line of sight and within distance");
                        return true;
                    }
                } else {
                    Debug.Log("[WithinDistance] IsWithinDistance: Target is within distance (line of sight not required)");
                    return true;
                }
            }

            Debug.Log("[WithinDistance] IsWithinDistance: Target is not within distance");
            return false;
        }

        public override void OnReset()
        {
            m_UsePhysics2D = false;
            m_TargetObject = null;
            m_TargetTag = string.Empty;
            m_TargetLayerMask = (LayerMask)0;
            m_Magnitude = 5;
            m_LineOfSight = true;
            m_IgnoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
            m_Offset = Vector3.zero;
            m_TargetOffset = Vector3.zero;
            Debug.Log("[WithinDistance] OnReset: Reset all fields to default values");
        }

        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Owner == null || m_Magnitude == null) {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(Owner.transform.position, m_UsePhysics2D ? Owner.transform.forward : Owner.transform.up, m_Magnitude.Value);
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}
