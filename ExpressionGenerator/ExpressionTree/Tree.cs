using System.Collections.Generic;

namespace ExpressionGenerator.ExpressionTree
{
    /// <summary>
    /// The entire expression tree
    /// </summary>
    internal class Tree
    {
        private string _expression;
        public static List<char> operators = null;

        public INode Root { get; private set; }

        /// <summary>
        /// Returns the expression which describes this tree
        /// </summary>
        public string Expression //[[4]]
        {
            get
            {
                if (_expression == null)
                    return _expression = Root.Evaluate();
                return _expression;
            }
        }

        /// <summary>
        /// Instantiate an empty tree (used for creating trees after a format)
        /// </summary>
        public Tree()
        {
            Root = new OperandNode();
        }

        /// <summary>
        /// Build a tree which represents an expression, which has the specified
        /// number of operandsand evaluates to the specified value.
        /// </summary>
        /// <param name="desiredValue">The value of the final expression when evaluated</param>
        /// <param name="numberOfOperands">The number of operands in the final expression</param>
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

        /// <summary>
        /// Clear the contents of this tree (that is, turn it into a format tree)
        /// </summary>
        public void Clear()
        {
            Root.Clear();
            Root.Left.Clear();
            Root.Right.Clear();
            _expression = null;
        }

        /// <summary>
        /// Join two trees with a specified operator
        /// </summary>
        /// <param name="op">Character representation of the operator to join the two subtrees</param>
        /// <param name="left">Left subtree</param>
        /// <param name="right">Right subtree</param>
        /// <returns></returns>
        public static Tree Join(char op, Tree left, Tree right)
        {
            return new Tree(op, left, right);
        }

        /// <summary>
        /// Replace all # with numbers so that the final expression has the specified value
        /// </summary>
        /// <param name="goal">The value of the final expression when evaluated</param>
        public void PopulateWithNumbers(int goal)
        {
            if (Root.GetValue() != null)
                return;
            Root.SetValue(goal, true);
        }
    }
}
