using System.Collections.Generic;
using System.Linq;

namespace ExpressionGenerator.Helpers
{
    static class ContainerExtensions
    {
        public static T GetRandomElement<T>(this List<T> list) //[[6]]
        {
            return list[ExpressionGenerator.Random.Next(list.Count)];
        }

        public static T GetRandomElement<T>(this HashSet<T> set) //[[6]]
        {
            return set.ToList().GetRandomElement();
        }
    }
}
