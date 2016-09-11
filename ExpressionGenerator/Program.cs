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

            //var expr = generator.FromFormat("((#+#-#/#)*#-(#*#+#/#-#)/#)/#", 130);
            for(int i = 0; i < 10; i++)
            {
                var expr = generator.BuildExpression(999, 10);
                Console.WriteLine(expr);
            }
            Console.ReadKey();
        }
    }
}
