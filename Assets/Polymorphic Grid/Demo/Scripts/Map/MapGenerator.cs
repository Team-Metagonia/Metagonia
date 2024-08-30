using TheoryTeam.PolymorphicGrid;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class MapGenerator : MonoBehaviour
    {
        public GridMaster master;
        [Range(0f, 1f)]
        public float fillPrecentage = .1f;
        public int seed = 0;
        [HideInInspector]
        public bool autoGenerate = false;

        public void GenerateMap()
        {
            if (master.MaxSize == 0)
                throw new ArgumentException("Grid not initialized!");

            int size = master.MaxSize;
            var rand = new System.Random(seed);
            float unwalkableCount = master.MaxSize * fillPrecentage;
            Node firstWalkable = null;
            Node current;

            for (int i = size - 1; i > -1; i--)
            {
                current = master.GetNode(i);
                current.walkable = (Mathf.Abs(rand.Next() % 100) / 100f) > (unwalkableCount / size);
                if (firstWalkable == null)
                {
                    if (current.walkable)
                        firstWalkable = current;
                    continue;
                }

                if (!current.walkable)
                {
                    unwalkableCount--;
                    Traversal<Node> pathsTraversal = PathFindingManager.FindAllPathes(firstWalkable, master);
                    foreach (Node n in master.Nodes)
                    {
                        if (n != firstWalkable && n.walkable && !pathsTraversal.HasParent(n))
                        {
                            current.walkable = true;
                            unwalkableCount++;
                            break;
                        }
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        private MapGenerator generator;

        private void OnEnable()
        {
            generator = target as MapGenerator;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button(new GUIContent("Random Seed")))
            {
                Undo.RecordObject(generator, "Modify Seed");
                generator.seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            generator.autoGenerate = GUILayout.Toggle(generator.autoGenerate, new GUIContent("Auto Generate"));
            GUI.enabled = !generator.autoGenerate;
            if (GUILayout.Button(new GUIContent("Generate Map")))
            {
                Undo.RegisterCompleteObjectUndo(generator.master, "Generate Map");
                generator.GenerateMap();
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            if (generator.autoGenerate && GUI.changed)
                generator.GenerateMap();
        }
    }
#endif
}