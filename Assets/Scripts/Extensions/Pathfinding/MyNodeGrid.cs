using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions.Pathfinding
{
    public class MyNodeGrid : MonoBehaviour
    {
        [SerializeField] private MyNode myNodePrefab;
        [SerializeField] private LayerMask unWalkableMask;
        [SerializeField] private Vector3 gridworldSize;
        [SerializeField] private float nodeRadius;
        [SerializeField] private float ySpacing;
        
        
        private MyNode [,,] m_grid;
        private float m_nodeDiameter;
        private int m_gridSizeX;
        private int m_gridSizeY;
        private int m_gridSizeZ;


        private void Awake()
        {
            Initialize();
        }


        [ContextMenu("GenerateGrid")]
        private void Initialize()
        {
            
            m_nodeDiameter = nodeRadius*2;
            m_gridSizeX = Mathf.RoundToInt(gridworldSize.x/m_nodeDiameter);
            m_gridSizeY = Mathf.RoundToInt(gridworldSize.y/m_nodeDiameter);
            m_gridSizeZ = Mathf.RoundToInt(gridworldSize.z/m_nodeDiameter);
            CreateGrid();
        }
        
        void CreateGrid()
        {
            m_grid = new MyNode[m_gridSizeX, m_gridSizeY, m_gridSizeZ];
            Vector3 l_worldBottomLeft = transform.position - Vector3.right * gridworldSize.x / 2 -
                                      Vector3.up * gridworldSize.y / 2 - Vector3.forward * gridworldSize.z /2;

            var l_halfExtents = new Vector3(m_nodeDiameter,m_nodeDiameter,m_nodeDiameter);
            for (int l_x = 0; l_x < m_gridSizeX; l_x++)
            {
                for (int l_y = 0; l_y < m_gridSizeY; l_y++)
                {
                    for (int l_z = 0; l_z < m_gridSizeZ; l_z++)
                    {
                        //Fijamos la verdadera posicion en el mundo del nodo
                        Vector3 l_worldPoint = l_worldBottomLeft + Vector3.right * (l_x * m_nodeDiameter + nodeRadius) + Vector3.up * ySpacing * ((l_y) * m_nodeDiameter + nodeRadius) + Vector3.forward *(l_z*m_nodeDiameter+ nodeRadius);
                        bool l_walkable =! Physics.CheckBox(l_worldPoint, l_halfExtents, Quaternion.identity, unWalkableMask);
                        
                        var l_node = new MyNode();
                        l_node.Initialize(l_walkable, l_worldPoint, m_nodeDiameter/2 , new Vector3(l_x, l_y, l_z));

                        
                        m_grid[l_x, l_y, l_z] = l_node;
                    }
                }
            }
            
            Debug.Log(m_grid.Length);
        }

       
        public MyNode NodeFromWorldPoint(Vector3 p_worldPosition)
        {
            var l_position = transform.position;
            var l_percentX = ((p_worldPosition.x - l_position.x) + gridworldSize.x / 2) / gridworldSize.x;
            var l_percentY = ((p_worldPosition.y - l_position.y) + gridworldSize.y / 2) / gridworldSize.y;
            var l_percentZ = ((p_worldPosition.z - l_position.z) + gridworldSize.z / 2) / gridworldSize.z;
            
            
            l_percentX = Mathf.Clamp01(l_percentX);
            l_percentY = Mathf.Clamp01(l_percentY);
            l_percentZ = Mathf.Clamp01(l_percentZ);

            var l_x = Mathf.RoundToInt((m_gridSizeX - 1) * l_percentX);
            var l_y = Mathf.RoundToInt((m_gridSizeY - 1) * l_percentY);
            var l_z = Mathf.RoundToInt((m_gridSizeZ - 1) * l_percentZ);
            return m_grid[l_x, l_y, l_z];
        }
        
        public IEnumerable<MyNode> GetNeighbours(MyNode p_node)
        {
            var l_neighbours = new List<MyNode>();

            if (p_node.XId-1 >-1)
                l_neighbours.Add(m_grid[(p_node.XId - 1), p_node.YId, p_node.ZId]);

            if (p_node.XId + 1 <= m_gridSizeX-1)
                l_neighbours.Add(m_grid[(p_node.XId + 1), p_node.YId, p_node.ZId]);
            
            if (p_node.YId - 1 > -1)
                l_neighbours.Add(m_grid[p_node.XId, (p_node.YId-1), p_node.ZId]);

            if (p_node.YId + 1 <= m_gridSizeY-1)
                l_neighbours.Add(m_grid[p_node.XId, (p_node.YId + 1), p_node.ZId]);
            
            if (p_node.ZId - 1 > -1)
                l_neighbours.Add(m_grid[p_node.XId, p_node.YId, p_node.ZId -1]);
            
            if (p_node.ZId + 1 <= m_gridSizeZ-1)
                l_neighbours.Add(m_grid[p_node.XId, p_node.YId, p_node.ZId +1]);

            return l_neighbours.Where(p_x=> p_x.Walkable);
        }
        
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridworldSize.x, gridworldSize.y, gridworldSize.z));

            if (m_grid != null)
            {
                foreach (var l_node in m_grid)
                {
                    Gizmos.color = (l_node.Walkable) ? Color.green : Color.red;
                    Gizmos.DrawWireCube(l_node.WorldPos, Vector3.one * (m_nodeDiameter- 0.1f));
                }
            }
        }
#endif
        
    }
}