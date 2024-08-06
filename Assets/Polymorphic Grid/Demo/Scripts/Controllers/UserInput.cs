using UnityEngine;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    [RequireComponent(typeof(PlayerController))]
    public class UserInput : MonoBehaviour
    {
        public Camera cam;

        public PlayerController Controller { get; private set; }
        public ShootingHandler AttatchedShootingHandler { get; private set; }
        public Vector2 InputVector { get; private set; }

        private Vector3 GetPointOnPlane(Ray ray, float planeY) => ray.GetPoint((planeY - ray.origin.y) / ray.direction.y);

        private void UpdateInputVector() => InputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        private Vector2 GetLookDirection()
        {
            Vector3 look = (GetPointOnPlane(cam.ScreenPointToRay(Input.mousePosition), transform.position.y) - transform.position).normalized;
            return new Vector2(look.x, look.z);
        }

        private Vector2 GetMoveDirection()
        {
            Vector3 right = cam.transform.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up).normalized;
            Vector3 dir = right * InputVector.x + forward * InputVector.y;
            return new Vector2(dir.x, dir.z);
        }

        private void Awake()
        {
            Controller = GetComponent<PlayerController>();
            AttatchedShootingHandler = GetComponent<ShootingHandler>();

            if (cam == null)
                cam = Camera.main;
        }

        private void Update()
        {
            UpdateInputVector();
            Controller.LookDirection = GetLookDirection();
            Controller.MoveDirection = GetMoveDirection();

            if (Input.GetMouseButtonDown(0))
                AttatchedShootingHandler.Shoot();
        }
    }
}