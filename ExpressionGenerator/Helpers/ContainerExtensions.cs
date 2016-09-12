using System.Collections.Generic;
using System.Linq;

namespace ExpressionGenerator.Helpers
{
    /// <summary>
    /// Extension methods for some container classes
    /// </summary>
    internal static class ContainerExtensions
    {
        /// <summary>
        /// Gets a random element from a List
        /// </summary>
        public static T GetRandomElement<T>(this List<T> list) //[[6]]
        {
            return list[ExpressionGenerator.Random.Next(list.Count)];
        }

        /// <summary>
        /// Gets a random element from a HashSet
        /// </summary>
        public static T GetRandomElement<T>(this HashSet<T> set) //[[6]]
        {
            return set.ToList().GetRandomElement();
        }
    }
}
