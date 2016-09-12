using System;

namespace ExpressionGenerator.ExpressionTree
{
    /// <summary>
    /// A structure which represents a single operator
    /// </summary>
    internal struct Operator //[[2]]
    {
        private char _operator;
        private Func<int, int, int> calculate;
        public int  PrecedenceLevel { get; private set; } //[[4]]
        public bool IsStrongPrecedence { get; private set; }

        /// <summary>
        /// Instantiate a new operator
        /// </summary>
        /// <param name="op">Character representation of the operator</param>
        public Operator(char op)
        {
            _operator = op;
            if (op == '+')
            {
                PrecedenceLevel = 1;
                IsStrongPrecedence = false;
                calculate = (x, y) => (x + y);
            }
            else if (op == '-')
            {
                PrecedenceLevel = 1;
                IsStrongPrecedence = true;
                calculate = (x, y) => (x - y);
            }
            else if (op == '*')
            {
                PrecedenceLevel = 2;
                IsStrongPrecedence = false;
                calculate = (x, y) => (x * y);
            }
            else if (op == '/')
            {
                PrecedenceLevel = 2;
                IsStrongPrecedence = true;
                calculate = (x, y) => (x / y);
            }
            else
                throw new ArgumentException("Invalid operator");
        }

        /// <summary>
        /// Calculate the value of performing an operation on two operands
        /// </summary>
        /// <param name="left">Left-hand operand</param>
        /// <param name="right">Right-hand operand</param>
        /// <returns></returns>
        public int? Evaluate(int? left, int? right)
        {
            if (left == null || right == null)
                return null;
            return calculate(left.Value, right.Value);
        }

        public override string ToString()
        {
            return _operator.ToString();
        }
    }
}