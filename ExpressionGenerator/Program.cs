using System;

namespace ExpressionGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = ExpressionGenerator.Instance;
            var expr      = generator.BuildExpression(100, 20);
            var format    = generator.FromFormat("#-#+#/#*#+#/#-#+#+#*#", 3);
            Console.WriteLine(expr);
            Console.WriteLine(format);
            Console.ReadKey();
        }
    }
}
