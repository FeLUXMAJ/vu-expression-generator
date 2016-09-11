using System;
using System.Collections.Generic;
using ExpressionGenerator.Helpers;

namespace ExpressionGenerator.ExpressionTree
{
    class OperatorNode : INode
    {
        public INode Left { get; private set; }
        public INode Right { get; private set; }
        public Operator Operator { get; private set; }

        internal OperatorNode(char op, Tree left, Tree right)
        {
            Operator = new Operator(op);
            Left = left.Root;
            Right = right.Root;
        }

        public OperatorNode(int goal, int numberOfNewNodes = 2)
        {
            int left, right;
            char op;
            do
            {
                op = Tree.operators.GetRandomElement();
            } while (!IsValidOperator(op, goal, out left, out right));

            Operator = new Operator(op);
            Left  = new OperandNode(left);
            Right = new OperandNode(right);
            
            int toLeft = 0;
            if(numberOfNewNodes > 1)
                toLeft = ExpressionGenerator.Random.Next(1, numberOfNewNodes);
            Left = Left.Expand(toLeft);
            Right = Right.Expand(numberOfNewNodes - toLeft);
        }   

        private bool IsValidOperator(char op, int goal, out int left, out int right)
        {
            left = right = 0;
            if(op == '-')
            {
                int naturalModification = Configuration.AllowZero ? 0 : 1;

                int lowerBound = goal + naturalModification;
                int upperBound = Configuration.MaxOperandValue;

                if (upperBound < lowerBound)
                    return false;

                left = ExpressionGenerator.Random.Next(lowerBound, upperBound + 1);
                right = left - goal;
                
                if (left  < naturalModification || left  > Configuration.MaxOperandValue ||
                    right < naturalModification || right > Configuration.MaxOperandValue)
                    return false;
                return true;
            }

            if(op == '+')
            {
                int naturalModification = Configuration.AllowZero ? 0 : 1;
                int lowerBound = Math.Max(naturalModification, goal - Configuration.MaxOperandValue);
                int upperBound = Math.Min(Configuration.MaxOperandValue, goal - naturalModification);
                if (upperBound < lowerBound)
                    return false;
                left = ExpressionGenerator.Random.Next(lowerBound, upperBound + 1);
                right = goal - left;
                if (left  < naturalModification || left  > Configuration.MaxOperandValue ||
                    right < naturalModification || right > Configuration.MaxOperandValue)
                    return false;
                return true;
            }

            if (op == '*')
            {
                var divisors = new List<int>();
                for(int i = 1; i <= goal; i++)
                    if(goal % i == 0)
                        divisors.Add(i);

                left = divisors.GetRandomElement();
                right = goal / left;
                
                return true;
            }

            if(op == '/')
            {
                int lowerBound = 1;
                int upperBound = Configuration.MaxOperandValue / goal;

                right = ExpressionGenerator.Random.Next(lowerBound, upperBound + 1);
                left = right * goal;

                return true;
            }

            return false;
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
                shouldEnclose |= left.Operator == Operator && left.Operator.PrecedenceLeft;
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
                shouldEnclose |= right.Operator == Operator && right.Operator.PrecedenceRight;
            }

            if (shouldEnclose)
                ret += "(" + Right.Evaluate() + ")";
            else
                ret += Right.Evaluate();

            return ret;
        }

        public void SetValue(int goal)
        {
            int left, right;
            IsValidOperator(Operator.ToString().ToCharArray()[0], goal, out left, out right);
            Left.SetValue(left);
            Right.SetValue(right);
        }

        public int? GetValue()
        {
            return Operator.Evaluate(Left.GetValue(), Right.GetValue());
        }
    }
}
