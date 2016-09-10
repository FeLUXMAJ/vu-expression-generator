using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator.Entities.ExpressionTree
{
    class ExpressionTree
    {
        private string _expression;

        public INode Root { get; private set; }
        public string Expression
        {
            get
            {
                if (_expression == null)
                    return _expression = Root.Evaluate();
                return _expression;
            }
        }

        public ExpressionTree(int desiredValue, int numberOfOperands = 2)
        {
            Root = new OperandNode(desiredValue);
            if (numberOfOperands > 1)
                Root = Root.Expand(numberOfOperands);
        }
    }
}
