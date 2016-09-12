using ExpressionGenerator.Helpers;

namespace ExpressionGenerator.ExpressionTree
{
    /// <summary>
    /// Operator node of an expression tree.
    /// Holds the operand and its two children nodes.
    /// </summary>
    internal class OperatorNode : INode
    {
        public INode Left { get; private set; }
        public INode Right { get; private set; }
        public Operator Operator { get; private set; }

        /// <summary>
        /// Instantiate a new operator node and attach two subtrees to it
        /// </summary>
        /// <param name="op">Character representation of the operator</param>
        /// <param name="left">Left subtree</param>
        /// <param name="right">Right subtree</param>
        internal OperatorNode(char op, Tree left, Tree right)
        {
            Operator = new Operator(op);
            Left = left.Root;
            Right = right.Root;
        }

        /// <summary>
        /// Instantiate a new operator node and make sure that its value is as specified
        /// and the tree rooted at this node has the specified number of operand nodes.
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="numberOfNewNodes"></param>
        public OperatorNode(int goal, int numberOfNewNodes = 2)
        {
            int left, right;
            char op;
            // Choose a random valid operator for this node and split the goal
            do
            {
                op = Tree.operators.GetRandomElement();
            } while (!SplitHelper.SplitValue(goal, op, out left, out right));

            Operator = new Operator(op);
            Left  = new OperandNode(left);
            Right = new OperandNode(right);

            int toLeft, toRight;
            // Distribute the number of new operands to be made between the two sides
            SplitHelper.SplitNumberOfOperands(left, right, numberOfNewNodes, out toLeft, out toRight);

            Left = Left.Expand(toLeft);
            Right = Right.Expand(toRight);
        }   
        
        public INode Expand(int numberOfNewOperands = 2)
        {
            return this;
        }

        public string Evaluate()
        {
            string ret = "";

            bool shouldEnclose;
            if (Left is OperandNode)
                shouldEnclose = false;
            else
            {
                var left = (OperatorNode)Left;
                shouldEnclose  = left.Operator.PrecedenceLevel < Operator.PrecedenceLevel;
            }

            if(shouldEnclose)
                ret += "(" + Left.Evaluate() + ")";
            else
                ret += Left.Evaluate();

            ret += " " + Operator.ToString() + " ";

            if (Right is OperandNode)
                shouldEnclose = false;
            else
            {
                var right = (OperatorNode)Right;
                shouldEnclose  = right.Operator.PrecedenceLevel < Operator.PrecedenceLevel;
                shouldEnclose |= right.Operator.PrecedenceLevel == Operator.PrecedenceLevel && Operator.IsStrongPrecedence;
            }

            if (shouldEnclose)
                ret += "(" + Right.Evaluate() + ")";
            else
                ret += Right.Evaluate();

            return ret;
        }

        public void SetValue(int goal, bool presetFormat = false)
        {
            int left, right;
            while(!SplitHelper.SplitValue(goal, Operator.ToString().ToCharArray()[0], out left, out right, presetFormat));
            Left.SetValue(left, presetFormat);
            Right.SetValue(right, presetFormat);
        }

        public int? GetValue()
        {
            return Operator.Evaluate(Left.GetValue(), Right.GetValue());
        }

        public void Clear()
        {
        }
    }
}
