using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator.Entities.ExpressionTree
{
    class NumberNode : INode
    {
        public int Value { get; private set; }

        public INode Left { get { return null; } }

        public INode Right { get { return null; } }

        public NumberNode()
        {
            Value = new Random().Next(); // TODO: fix
        }

        public INode Expand(int numberOfNewOperands = 2)
        {
            return new OperatorNode();
        }

        public string Evaluate()
        {
            return Value.ToString();
        }
    }
}
