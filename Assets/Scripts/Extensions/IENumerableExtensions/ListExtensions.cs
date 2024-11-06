using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions.IENumerableExtensions
{
    public static class ListExtensions
    {
        public static T GetRandomElement<T>(this List<T> p_baseList)
        {
            return p_baseList[Random.Range(0, p_baseList.Count)];
        }

        public static List<T> GetUnmatchedElements<T>(ICollection<T> p_list1, List<T> p_list2)
        {
            List<T> l_unmatchedList = p_list1.Except(p_list2).ToList();
            l_unmatchedList.AddRange(p_list2.Except(p_list1));

            return l_unmatchedList;
        }

        public static void Shuffle<T>(this IList<T> p_baseList)
        {

            var l_count = p_baseList.Count;
            var l_last = l_count - 1;
            for (var l_i = 0; l_i < l_last; ++l_i)
            {
                var l_r = UnityEngine.Random.Range(l_i, l_count);
                (p_baseList[l_i], p_baseList[l_r]) = (p_baseList[l_r], p_baseList[l_i]);
            }
        }

    }
}