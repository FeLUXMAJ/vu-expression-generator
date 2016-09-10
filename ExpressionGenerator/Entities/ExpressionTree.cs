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

        public void BuildTree()
        {
            _expression = null;
            Root.Expand();
        }


    }
}
