using UnityEngine;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource hitSource;
        public AudioSource shootSource;
        public ShootingHandler playerShootHandler;

        private void OnEnemyCreated(Enemy e)
        {
            e.GetComponent<HealthHandler>().OnDieCallback += () => hitSource.Play();
        }

        private void Awake()
        {
            Enemy.OnEnemyCreatedCallback += OnEnemyCreated;
            playerShootHandler.OnShootCallback += shootSource.Play;
        }

        private void OnDestroy()
        {
            playerShootHandler.OnShootCallback -= shootSource.Play;
            Enemy.OnEnemyCreatedCallback -= OnEnemyCreated;
        }
    }
}