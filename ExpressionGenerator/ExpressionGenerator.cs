using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionGenerator.Entities.ExpressionTree;

namespace ExpressionGenerator
{
    class ExpressionGenerator
    {
        public ExpressionGenerator()
        {
            Configuration.MaxOperandValue = 100;
            Configuration.AllowZero = true;
            Configuration.AllowedOperators = Configuration.Operators.ALL;
        }

        public string BuildExpression(int desiredValue, int numberOfOperands = 2)
        {
            var expressionTree = new ExpressionTree(desiredValue, numberOfOperands);
            return expressionTree.Expression;
        }
    }
}
