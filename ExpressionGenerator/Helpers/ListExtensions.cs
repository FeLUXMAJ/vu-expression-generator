using System.Collections.Generic;

namespace ExpressionGenerator.Helpers
{
    static class ListExtensions
    {
        public static T GetRandomElement<T>(this List<T> list) //[[6]]
        {
            return list[ExpressionGenerator.Random.Next(list.Count)];
        }
    }
}
