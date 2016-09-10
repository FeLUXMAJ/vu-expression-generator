using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionGenerator.Entities.ExpressionTree;

namespace ExpressionGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = ExpressionGenerator.Instance;
            var expr      = generator.BuildExpression(50, 4);
            Console.WriteLine(expr);
            Console.ReadKey();
        }
    }
}
