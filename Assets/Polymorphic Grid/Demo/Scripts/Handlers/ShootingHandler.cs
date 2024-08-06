using UnityEngine;
using System;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class ShootingHandler : MonoBehaviour
    {
        public GameObject prefab;
        public Transform spawnPoint;
        public float shootForce;
        public float shootDuration;
        public float prefabLifeTime;
        public int startPrefabsCount;
        public ParticleSystem shootEffect;

        public event Action<int> OnPrefabsCountUpdatedCallback;
        public event Action OnShootCallback;

        public bool Shooting { get; private set; }

        public int CurrentPrefabsCount
        {
            get => currentPrefabsCount;
            set
            {
                OnPrefabsCountUpdatedCallback?.Invoke(value);
                currentPrefabsCount = value;
            }
        }

        private int currentPrefabsCount;
        private float timer;

        private void Awake()
        {
            CurrentPrefabsCount = startPrefabsCount;
            timer = 0f;
        }

        private void Update()
        {
            if (Shooting)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                    Shooting = false;
            }
        }

        public GameObject Shoot()
        {
            if (Shooting || CurrentPrefabsCount <= 0)
                return null;

            Shooting = true;
            timer = shootDuration;
            
            GameObject obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddForce(shootForce * spawnPoint.forward);
            if (shootEffect != null)
                shootEffect.Play();

            Destroy(obj, prefabLifeTime);
            CurrentPrefabsCount--;

            OnShootCallback?.Invoke();
            return obj;
        }
    }
}
