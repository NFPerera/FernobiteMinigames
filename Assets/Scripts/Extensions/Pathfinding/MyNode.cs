using UnityEngine;

namespace Extensions.Pathfinding
{
    public class MyNode
    {
        public int XId;
        public int YId;
        public int ZId;
        public bool Walkable;
        public Vector3 WorldPos;

        public void Initialize(bool p_walkable, Vector3 p_worldPos, float p_radius, Vector3 gridId)
        {
            Walkable = p_walkable;
            WorldPos = p_worldPos;

            XId = (int)gridId.x;
            YId = (int)gridId.y;
            ZId = (int)gridId.z;
        }
    }
}