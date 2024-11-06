using System;
using System.Collections.Generic;
using _Main.Scripts.DevelopmentUtilities;
using Extensions.IENumerableExtensions;

namespace Extensions.Pathfinding
{
    public static class ThetaStar
    {
        public static List<T> Run<T>(T p_start, T p_target,
            Func<T, T, bool> p_satisfies,
            Func<T, List<T>> p_connections,
            Func<T, T, float> p_getCost,
            Func<T,T, float> p_heuristic,
            Func<T, T, bool> p_inView,
            int p_watchdog = 2000)
        {
            PriorityQueue<T> l_pending = new PriorityQueue<T>();
            HashSet<T> l_visited = new HashSet<T>();
            Dictionary<T, T> l_parent = new Dictionary<T, T>();
            Dictionary<T, float> l_cost = new Dictionary<T, float>();

            l_pending.Enqueue(p_start, 0);
            l_cost[p_start] = 0;

            while (p_watchdog > 0 && !l_pending.IsEmpty)
            {
                p_watchdog--;
                var l_curr = l_pending.Dequeue();
                
                if (p_satisfies(l_curr, p_target))
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
                var l_neighbours = p_connections(l_curr);
                for (int l_i = 0; l_i < l_neighbours.Count; l_i++)
                {
                    var l_neigh = l_neighbours[l_i];
                    if (l_visited.Contains(l_neigh)) continue;
                    T l_realParent = l_curr;
                    if (l_parent.ContainsKey(l_curr) && p_inView(l_parent[l_curr], l_neigh))
                    {
                        l_realParent = l_parent[l_curr];
                    }
                    float l_tentativeCost = l_cost[l_realParent] + p_getCost(l_realParent, l_neigh);
                    if (l_cost.ContainsKey(l_neigh) && l_cost[l_neigh] < l_tentativeCost) continue;
                    l_pending.Enqueue(l_neigh, l_tentativeCost + p_heuristic(l_neigh, p_target));
                    l_parent[l_neigh] = l_realParent;
                    l_cost[l_neigh] = l_tentativeCost;
                }
            }
            return new List<T>();
        }
    }
}

