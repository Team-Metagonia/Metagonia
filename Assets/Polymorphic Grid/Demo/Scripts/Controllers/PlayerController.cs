using UnityEngine;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float turnSpeed = 20f;

        public Vector2 LookDirection { get; set; }
        public Vector2 MoveDirection { get; set; }
        public Rigidbody AttatchedRigidbody { get; private set; }

        private void HandleRotation()
        {
            float mag = LookDirection.magnitude;
            if (mag > 0f)
            {
                Vector3 look = new Vector3(LookDirection.x / mag, 0f, LookDirection.y / mag);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.forward, look) * transform.rotation, Time.fixedDeltaTime * turnSpeed);
            }
        }

        private void Awake()
        {
            AttatchedRigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HandleRotation();
        }

        private void FixedUpdate()
        {
            AttatchedRigidbody.MovePosition(transform.position + moveSpeed * Time.fixedDeltaTime * new Vector3(MoveDirection.x, 0f, MoveDirection.y));
        }
    }
}