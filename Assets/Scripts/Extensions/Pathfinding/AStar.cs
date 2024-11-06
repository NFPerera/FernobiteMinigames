using System;
using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities;
using Extensions.IENumerableExtensions;

namespace Extensions.Pathfinding
{
    public class AStar<T>
    {
        public List<T> Run(T p_start, T p_target,
            Func<T, T, bool> p_satiesfies,
            Func<T, List<T>> p_conections,
            Func<T, T, float> p_getCost,
            Func<T,T, float> p_heuristic)
        {
            PriorityQueue<T> l_pending = new PriorityQueue<T>();
            HashSet<T> l_visited = new HashSet<T>();
            Dictionary<T, T> l_parent = new Dictionary<T, T>();
            Dictionary<T, float> l_cost = new Dictionary<T, float>();

            l_pending.Enqueue(p_start, 0);
            l_cost[p_start] = 0;

            while (!l_pending.IsEmpty)
            {
                var l_curr = l_pending.Dequeue();
                
                if (p_satiesfies(l_curr, p_target))
                {
                    var l_path = new List<T>();
                    l_path.Add(l_curr);
                    while (l_parent.ContainsKey(l_path[l_path.Count - 1]))
                    {
                        var l_father = l_parent[l_path[l_path.Count - 1]];
                        l_path.Add(l_father);
                    }
                    l_path.Reverse();
                    return l_path;
                }
                l_visited.Add(l_curr);
                var l_neighbours = p_conections(l_curr);
                for (int l_i = 0; l_i < l_neighbours.Count; l_i++)
                {
                    var l_neigh = l_neighbours[l_i];
                    if (l_visited.Contains(l_neigh)) continue;
                    float l_tentativeCost = l_cost[l_curr] + p_getCost(l_curr, l_neigh);
                    if (l_cost.ContainsKey(l_neigh) && l_cost[l_neigh] < l_tentativeCost) continue;
                    l_pending.Enqueue(l_neigh, l_tentativeCost + p_heuristic(l_neigh, p_target));
                    l_parent[l_neigh] = l_curr;
                    l_cost[l_neigh] = l_tentativeCost;
                }
            }
            return new List<T>();
        }
        public List<T> CleanPath(List<T> p_path, Func<T, T, bool> p_inView)
        {
            if (p_path == null) return p_path;
            if (p_path.Count <= 2) return p_path;
            var l_list = new List<T>();
            l_list.Add(p_path[0]);
            for (int l_i = 2; l_i < p_path.Count - 1; l_i++)
            {
                var l_gp = l_list[l_list.Count - 1];
                if (!p_inView(l_gp, p_path[l_i]))
                {
                    l_list.Add(p_path[l_i - 1]);
                }
            }
            l_list.Add(p_path[p_path.Count - 1]);
            return l_list;
        }
    }
}

