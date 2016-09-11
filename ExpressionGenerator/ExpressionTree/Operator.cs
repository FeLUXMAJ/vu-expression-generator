using System;

namespace ExpressionGenerator.ExpressionTree
{
    internal struct Operator //[[2]]
    {
        private char _operator;
        private Func<int, int, int> calculate;
        public int  PrecedenceLevel { get; private set; } //[[4]]
        public bool IsStrongProcedence { get; private set; }

        public Operator(char op)
        {
            _operator = op;
            if (op == '+')
            {
                PrecedenceLevel = 1;
                IsStrongProcedence = false;
                calculate = (x, y) => (x + y);
            }
            else if (op == '-')
            {
                PrecedenceLevel = 1;
                IsStrongProcedence = true;
                calculate = (x, y) => (x - y);
            }
            else if (op == '*')
            {
                PrecedenceLevel = 2;
                IsStrongProcedence = false;
                calculate = (x, y) => (x * y);
            }
            else if (op == '/')
            {
                PrecedenceLevel = 2;
                IsStrongProcedence = true;
                calculate = (x, y) => (x / y);
            }
            else
                throw new ArgumentException("Invalid operator");
        }

        public int? Evaluate(int? left, int? right)
        {
            if (left == null || right == null)
                return null;
            return calculate(left.Value, right.Value);
        }

        public static bool operator ==(Operator lhs, Operator rhs)
        {
            return lhs.ToString() == rhs.ToString();
        }

        public static bool operator !=(Operator lhs, Operator rhs)
        {
            return !(lhs.ToString() == rhs.ToString());
        }

        public override string ToString()
        {
            return _operator.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return this == (Operator)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}