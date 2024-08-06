using UnityEngine;

namespace TheoryTeam.PolymorphicGrid.Demo
{
    public class PathHolder : MonoBehaviour
    {
        public GridMaster grid;
        public PathFindingManager finder;
        public Transform target;

        public static PathHolder Instance { get; private set; }

        public Traversal<Node> AllPathes { get; private set; }

        private PathRequestBase request;

        private void RequestPath()
        {
            if (request == null || finder == null)
                return;

            request.target = target.position;
            finder.RequestPath(request);
        }

        private void OnPathFound(PathResponseBase response)
        {
            AllPathes = (response as AllPathesResponse).traversal;
            RequestPath();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (finder == null)
                finder = PathFindingManager.Instance;

            request = new PathRequestBase(target.position, grid, OnPathFound);
            RequestPath();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (AllPathes == null)
                return;
            
            Gizmos.color = Color.red;
            foreach (Node n in grid.Nodes)
            {
                if (AllPathes.HasParent(n))
                {
                    Gizmos.DrawLine(n.WorldPosition + Vector3.up, AllPathes.GetParent(n).WorldPosition + Vector3.up);
                    Gizmos.DrawWireSphere(AllPathes.GetParent(n).WorldPosition, .1f);
                }
            }
        }
#endif

        public Vector3 GetMoveDirection(Vector3 pos)
        {
            if (grid == null || AllPathes == null)
                return Vector3.zero;

            Node n = grid.GetNode(pos);
            if (n != null && AllPathes.HasParent(n))
                return (AllPathes.GetParent(n).WorldPosition - n.WorldPosition).normalized;
            return Vector3.zero;
        }
    }
}