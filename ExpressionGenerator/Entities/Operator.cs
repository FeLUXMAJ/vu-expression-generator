using System;

namespace ExpressionGenerator.Entities.ExpressionTree
{
    internal class Operator
    {
        private char _operator;
        public int  PrecedenceLevel { get; private set; }
        public bool PrecedenceLeft  { get; private set; } // Parenthesise if expression is on the left
        public bool PrecedenceRight { get; private set; } // Parenthesise if expression is on the right

        public Operator(char op)
        {
            _operator = op;
            if (op == '+')
            {
                PrecedenceLevel = 1;
                PrecedenceLeft = PrecedenceRight = false;
            }
            if (op == '-')
            {
                PrecedenceLevel = 1;
                PrecedenceLeft = false;
                PrecedenceRight = true;
            }
            if (op == '*')
            {
                PrecedenceLevel = 2;
                PrecedenceLeft = PrecedenceRight = false;
            }
            if (op == '/')
            {
                PrecedenceLevel = 2;
                PrecedenceLeft = PrecedenceRight = true;
            }
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