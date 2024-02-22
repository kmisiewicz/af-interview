using System.Collections.Generic;
using UnityEngine;

namespace AFSInterview.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Perform a Fisher-Yates shuffle on a list
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = Random.Range(0, n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
