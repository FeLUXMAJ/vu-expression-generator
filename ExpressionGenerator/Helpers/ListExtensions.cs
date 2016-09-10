using System.Collections.Generic;

namespace ExpressionGenerator.Helpers
{
    static class ListExtensions
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[ExpressionGenerator.Random.Next(list.Count)];
        }
    }
}
