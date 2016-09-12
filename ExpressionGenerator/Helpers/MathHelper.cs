namespace ExpressionGenerator.Helpers
{
    /// <summary>
    /// Provides some helper mathematical methods
    /// </summary>
    internal static class MathHelper
    {
        /// <summary>
        /// Swaps the values of two elements
        /// </summary>
        public static void Swap<T>(ref T a, ref T b) //[[8]]
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

        /// <summary>
        /// Determines whether the given number is prime or not
        /// </summary>
        /// <param name="number">The number to be tested for primality</param>
        /// <returns>True if the number is prime, false otherwise</returns>
        public static bool IsPrime(int number)
        {
            if (number < 2)
                return false;
            if (number == 2)
                return true;
            if (number % 2 == 0)
                return false;
            for (int i = 3; i * i <= number; i += 2)
                if (number % i == 0)
                    return false;
            return true;
        }
    }
}
