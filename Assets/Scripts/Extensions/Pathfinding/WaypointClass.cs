using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Extensions.Pathfinding
{
    [System.Serializable]
    public class WaypointClass : MonoBehaviour
    {
        public Vector3[] points;
        public Vector3 this[int p_index]
        {
            get { return points[p_index]; }
            set { points[p_index] = value; }
        }

        public int Lenght
        {
            get
            {
                if (points == null)
                    return 0;
                else return points.Length;
            }
        }


        public int GetClosestWaypoint(Vector3 p_entityPosition)
        {
            float l_closestDistanceAux = -1;
            int l_closestIndex = -1;


            for (int l_i = 0; l_i < points.Length; l_i++)
            {
                float l_distance = Vector3.Distance(p_entityPosition, points[l_i]);

                if (l_distance < l_closestDistanceAux)
                {
                    l_closestDistanceAux = l_distance;
                    l_closestIndex = l_i;
                }
            }

            return l_closestIndex;
        }
        
        
        public Vector3 GetWaypointFromIndex(int p_index)
        {
            if (p_index < points.Length)
            {
                return points[p_index];
            }
            else
            {
                Debug.LogError("InsertedIndex is out of range");
                return Vector3.zero;
            }
        }
        
    }
}