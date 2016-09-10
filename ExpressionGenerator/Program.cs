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
            var tree = new ExpressionTree(16, 50);
            Console.WriteLine(tree.Expression);
            Console.ReadKey();
        }
    }
}
