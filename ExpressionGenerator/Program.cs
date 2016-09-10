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
            var generator = new ExpressionGenerator();
            var expr      = generator.BuildExpression(16, 10);
            Console.WriteLine(expr);
            Console.ReadKey();
        }
    }
}
