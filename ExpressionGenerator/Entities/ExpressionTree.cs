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
        public static List<char> operators = null;

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
            if(operators == null)
            {
                operators = new List<char>();
                var allowed = Configuration.AllowedOperators;
                if (allowed.HasFlag(Configuration.Operators.ADD))
                    operators.Add('+');
                if (allowed.HasFlag(Configuration.Operators.SUB))
                    operators.Add('-');
                if (allowed.HasFlag(Configuration.Operators.MUL))
                    operators.Add('*');
                if (allowed.HasFlag(Configuration.Operators.DIV))
                    operators.Add('/');
            }
            Root = new OperandNode(desiredValue);
            if (numberOfOperands > 1)
                Root = Root.Expand(numberOfOperands);
        }
    }
}
