using System;

namespace ExpressionGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = ExpressionGenerator.Instance;
            //var expr      = generator.BuildExpression(100, 10);
            //var format    = generator.FromFormat("#-#+#/#*#+#/#-#+#+#*#", 3);
            //Console.WriteLine(expr);
            //Console.WriteLine(format);
            for(int i = 10; i <= 20; i++)
            {
                var expr = generator.BuildExpression(i, i / 2);
                Console.WriteLine(expr);
            }
            Console.ReadKey();
        }
    }
}
