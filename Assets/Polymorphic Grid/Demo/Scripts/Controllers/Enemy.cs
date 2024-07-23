using UnityEngine;
using System;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    [RequireComponent(typeof(PlayerController))]
    public class Enemy : MonoBehaviour
    {
        public GameObject dieEffect;
        public float dieEffectLifetime = 5f;
        public PathHolder holder;
        public int damageAmount = 10;

        public HealthHandler AttatchedHealthHandler { get; private set; }
        public PlayerController AttachedController { get; private set; }

        public static event Action<Enemy> OnEnemyCreatedCallback;

        private void Awake()
        {
            AttachedController = GetComponent<PlayerController>();
            AttatchedHealthHandler = GetComponent<HealthHandler>();
            AttatchedHealthHandler.OnDieCallback += () =>
            {
                if (dieEffect != null)
                {
                    Destroy(Instantiate(dieEffect, transform.position, Quaternion.identity), dieEffectLifetime);
                    Destroy(gameObject);
                }
            };

            OnEnemyCreatedCallback?.Invoke(this);
        }

        private void Start()
        {
            if (holder == null)
                holder = PathHolder.Instance;
        }

        private void Update()
        {
            Vector3 dir = holder.GetMoveDirection(transform.position);
            AttachedController.MoveDirection = new Vector2(dir.x, dir.z);
        }

        private void OnCollisionEnter(Collision collision)
        {
            HealthHandler handler = collision.gameObject.GetComponent<HealthHandler>();
            if (handler != null && collision.gameObject.GetComponent<Enemy>() == null)
                handler.TakeDamage(damageAmount);
        }
    }
}