using UnityEngine;
using System;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    [Serializable]
    public class GridBasedSpawner
    {
        public GameObject prefab;
        public Vector3 initPosOffset = Vector3.up;
        public float minTimeBetweenSpawns = 1f;
        public float maxTimeBetweenSpawns = 2f;
        public GridMaster targetGrid;
        public Transform root;

        public bool Enabled { get; set; }
        public float TimeToGenerateNext { get; private set; }

        public event Action<int> OnNodeChoosedCallback;
        public event Action<GameObject> OnSpawnCallback;

        private int nextIndex = -1;

        private int PickRandomNode()
        {
            int temp;

            do
            {
                temp = UnityEngine.Random.Range(0, targetGrid.MaxSize);
            } while (!targetGrid.GetNode(temp).walkable);
            return temp;
        }

        private void PickNext() => nextIndex = PickRandomNode();

        public void Update(float deltaTime)
        {
            if (!Enabled)
                return;

            if (TimeToGenerateNext <= 0f)
            {
                if (nextIndex != -1)
                {
                    GameObject temp = Spawn(nextIndex);
                    OnSpawnCallback?.Invoke(temp);
                }

                TimeToGenerateNext = UnityEngine.Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
                
                PickNext();
                OnNodeChoosedCallback?.Invoke(nextIndex);
            }

            TimeToGenerateNext -= deltaTime;
        }

        public GameObject Spawn(int nodeIndex)
        {
            GameObject generated = UnityEngine.Object.Instantiate(prefab, targetGrid.GetNode(nodeIndex).WorldPosition + initPosOffset, Quaternion.identity);
            if (root != null)
                generated.transform.parent = root;
            return generated;
        }

        public GameObject Spawn() => Spawn(PickRandomNode());
    }
}