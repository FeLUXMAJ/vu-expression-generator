using System;

namespace ExpressionGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var generator = ExpressionGenerator.Instance;
            var expr      = generator.BuildExpression(100, 20);
            Console.WriteLine(expr);*/
            new ExpressionFormat("#*(#+#*(#-#+#))");
            Console.ReadKey();
        }
    }
}
