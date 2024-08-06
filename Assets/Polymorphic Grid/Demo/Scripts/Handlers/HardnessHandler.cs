using UnityEngine;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class HardnessHandler : MonoBehaviour
    {
        public GameManagement management;
        public float hardnessRatio = .01f;
        public float targetMinTime = .1f;
        public float targetMaxTime = .2f;

        private void Update()
        {
            if (management.enemySpawner.minTimeBetweenSpawns > targetMinTime)
                management.enemySpawner.minTimeBetweenSpawns -= hardnessRatio * Time.deltaTime;
            if (management.enemySpawner.maxTimeBetweenSpawns > targetMaxTime)
                management.enemySpawner.maxTimeBetweenSpawns -= hardnessRatio * Time.deltaTime;
        }
    }
}