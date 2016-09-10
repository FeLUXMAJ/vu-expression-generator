using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator.Entities.ExpressionTree
{
    class OperandNode : INode
    {

        public int Value { get; private set; }
        public INode Left { get { return null; } }
        public INode Right { get { return null; } }

        public OperandNode(int value)
        {
            Value = value;
        }

        public INode Expand(int numberOfNewOperands = 2)
        {
            if (numberOfNewOperands == 1)
                return this;
            return new OperatorNode(Value, numberOfNewOperands);
        }

        public string Evaluate()
        {
            return Value.ToString();
        }
    }
}
