using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator.Entities.ExpressionTree
{
    class OperatorNode : INode
    {
        private Operator _operator;
        public INode Left { get; private set; }

        public INode Right { get; private set; }

        public OperatorNode()
        {
            Left  = new NumberNode();
            Right = new NumberNode();
        }   

        public INode Expand(int numberOfNewOperands = 2)
        {
            return this;
        }

        public string Evaluate()
        {
            return "(" + Left.Evaluate() + " " + _operator.ToString() + " " + Right.Evaluate() + ")"; 
        }
    }
}
