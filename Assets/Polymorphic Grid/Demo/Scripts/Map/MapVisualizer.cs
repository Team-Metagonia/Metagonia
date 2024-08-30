using UnityEngine;
using System;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class MapVisualizer : MonoBehaviour
    {
        public GridMaster master;
        public float groundThickness = .1f;
        public float minObstaclesHeight = .9f;
        public float maxObstaclesHeight = 1.2f;
        public float visualizerRadiusMultiplier = 1f;
        public Material obstaclesMaterial;
        public Material groundMaterial;

        public GridVisualizer GroundVisualizer { get; private set; }
        public GridVisualizer ObstaclesVisualizer { get; private set; }

        private void Start()
        {
            UpdateVisualizer();
        }

        private void SetupVisualizers()
        {
            if (GroundVisualizer != null)
                DestroyImmediate(GroundVisualizer.gameObject);
            if (ObstaclesVisualizer != null)
                DestroyImmediate(ObstaclesVisualizer.gameObject);

            GroundVisualizer = new GameObject("Ground").AddComponent<GridVisualizer>();
            GroundVisualizer.transform.parent = transform;
            GroundVisualizer.materials.Add(groundMaterial);
            GroundVisualizer.visualizedNodeHeight = groundThickness;
            GroundVisualizer.visualizedNodeOffset = (1f - visualizerRadiusMultiplier) * master.nodeRadius;
            GroundVisualizer.autoGenerate = false;

            ObstaclesVisualizer = new GameObject("Obstacles").AddComponent<GridVisualizer>();
            ObstaclesVisualizer.transform.parent = transform;
            ObstaclesVisualizer.materials.Add(obstaclesMaterial);
            ObstaclesVisualizer.materials.Add(null);
            ObstaclesVisualizer.visualizedNodeHeight = 1f;
            ObstaclesVisualizer.visualizedNodeOffset = (1f - visualizerRadiusMultiplier) * master.nodeRadius + .01f;
            ObstaclesVisualizer.autoGenerate = false;

            GroundVisualizer.Grid = master;
            ObstaclesVisualizer.Grid = master;
        }

        private void AddColliders(GridVisualizer visualizer)
        {
            for (int i = 0; i < master.MaxSize; i++)
                visualizer.GetRenderer(i).gameObject.AddComponent<MeshCollider>().convex = true;
        }

        public void UpdateVisualizer()
        {
            if (master == null)
                throw new ArgumentNullException();

            SetupVisualizers();
            GroundVisualizer.UpdateMeshes();
            GroundVisualizer.UpdateAllMaterials();
            ObstaclesVisualizer.UpdateMeshes();

            foreach (Node n in master.Nodes)
                ObstaclesVisualizer.SetMaterial(n, n.walkable ? 1 : 0);

            for (int i = 0; i < master.MaxSize; i++)
                ObstaclesVisualizer.GetRenderer(i).transform.localScale = new Vector3(1f, UnityEngine.Random.Range(minObstaclesHeight, maxObstaclesHeight), 1f);
            AddColliders(GroundVisualizer);
            AddColliders(ObstaclesVisualizer);
        }
    }
}