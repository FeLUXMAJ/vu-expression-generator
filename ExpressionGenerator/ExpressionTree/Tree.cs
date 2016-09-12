using System.Collections.Generic;

namespace ExpressionGenerator.ExpressionTree
{
    class Tree
    {
        private string _expression;
        public static List<char> operators = null;

        public INode Root { get; private set; }
        public string Expression //[[4]]
        {
            get
            {
                if (_expression == null)
                    return _expression = Root.Evaluate();
                return _expression;
            }
        }

        public Tree()
        {
            Root = new OperandNode();
        }

        public Tree(int desiredValue, int numberOfOperands = 2)
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

        private Tree(char op, Tree left, Tree right)
        {
            Root = new OperatorNode(op, left, right);
        }

        public void Clear()
        {
            Root.Clear();
            Root.Left.Clear();
            Root.Right.Clear();
            _expression = null;
        }

        public static Tree Join(char op, Tree left, Tree right)
        {
            return new Tree(op, left, right);
        }

        public void PopulateWithNumbers(int goal)
        {
            if (Root.GetValue() != null)
                return;
            Root.SetValue(goal, true);
        }
    }
}
