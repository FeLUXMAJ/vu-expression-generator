using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator.Entities.ExpressionTree
{
    class OperatorNode : INode
    {
        private static Random _random = new Random();
        private char[] operatorValues = { '-', '+', '*', '/' };

        public INode Left { get; private set; }
        public INode Right { get; private set; }
        public Operator Operator { get; private set; }

        public OperatorNode(int goal, int numberOfNewNodes = 2)
        {
            int left, right;
            char op;
            do
            {
                op = operatorValues[_random.Next(operatorValues.Length)];
            } while (!IsValidOperator(op, goal, out left, out right));

            Operator = new Operator(op);
            Left  = new OperandNode(left);
            Right = new OperandNode(right);
            
            int toLeft = 0;
            if(numberOfNewNodes > 1)
                toLeft = _random.Next(1, numberOfNewNodes);
            Left = Left.Expand(toLeft);
            Right = Right.Expand(numberOfNewNodes - toLeft);
        }   

        private bool IsValidOperator(char op, int goal, out int left, out int right)
        {
            left = right = 0;
            if(op == '-')
            {
                if (!Configuration.AllowedOperators.HasFlag(Configuration.Operators.SUB))
                    return false;

                int naturalModification = Configuration.AllowZero ? 1 : 0;

                int lowerBound = goal + naturalModification;
                int upperBound = Configuration.MaxOperandValue;

                if (upperBound < lowerBound)
                    return false;

                left = _random.Next(lowerBound, upperBound + 1);
                right = left - goal;
                
                if (left  < naturalModification || left  > Configuration.MaxOperandValue ||
                    right < naturalModification || right > Configuration.MaxOperandValue)
                    return false;
                return true;
            }

            if(op == '+')
            {
                if (!Configuration.AllowedOperators.HasFlag(Configuration.Operators.ADD))
                    return false;
                int naturalModification = Configuration.AllowZero ? 1 : 0;
                int lowerBound = Math.Max(naturalModification, goal - Configuration.MaxOperandValue);
                int upperBound = Math.Min(Configuration.MaxOperandValue, goal - naturalModification);
                if (upperBound < lowerBound)
                    return false;
                left = _random.Next(lowerBound, upperBound + 1);
                right = goal - left;
                if (left  < naturalModification || left  > Configuration.MaxOperandValue ||
                    right < naturalModification || right > Configuration.MaxOperandValue)
                    return false;
                return true;
            }

            if (op == '*')
            {
                if (!Configuration.AllowedOperators.HasFlag(Configuration.Operators.MUL))
                    return false;

                var divisors = new List<int>();
                for(int i = 1; i <= goal; i++)
                    if(goal % i == 0)
                        divisors.Add(i);

                left = divisors[_random.Next(divisors.Count)];
                right = goal / left;
                
                return true;
            }

            if(op == '/')
            {
                if (!Configuration.AllowedOperators.HasFlag(Configuration.Operators.DIV))
                    return false;

                int lowerBound = 1;
                int upperBound = Configuration.MaxOperandValue / goal;

                right = _random.Next(lowerBound, upperBound + 1);
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
            /*if (Left is OperandNode || ((OperatorNode)Left).Operator.PrecedenceLevel == Operator.PrecedenceLevel)
                ret += Left.Evaluate();
            else
                ret += "(" + Left.Evaluate() + ")";

            ret += " " + Operator.ToString() + " ";

            if (Right is OperandNode || ((OperatorNode)Right).Operator.PrecedenceLevel == Operator.PrecedenceLevel)
                ret += Right.Evaluate();
            else
                ret += "(" + Right.Evaluate() + ")";

            return ret;*/

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
            
            /*if (Left is OperandNode)
                ret += Left.Evaluate();
            else if (((OperatorNode)Left).Operator.PrecedenceLevel < Operator.PrecedenceLevel)
                ret += "(" + Left.Evaluate() + ")";
            else if (((OperatorNode)Left).Operator.PrecedenceLevel > Operator.PrecedenceLevel)
                ret += Left.Evaluate();
            else if (((OperatorNode)Left).Operator.PrecedenceLeft)
                ret += "(" + Left.Evaluate() + ")";
            else
                ret += Left.Evaluate();*/

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

            /*if (Right is OperandNode)
                ret += Right.Evaluate();
            else if (((OperatorNode)Right).Operator.PrecedenceLevel < Operator.PrecedenceLevel)
                ret += "(" + Right.Evaluate() + ")";
            else if (((OperatorNode)Right).Operator.PrecedenceLevel > Operator.PrecedenceLevel)
                ret += Right.Evaluate();
            else if (((OperatorNode)Right).Operator.PrecedenceRight)
                ret += "(" + Right.Evaluate() + ")";
            else
                ret += Right.Evaluate();*/

            return ret;
            //return "(" + Left.Evaluate() + " " + Operator.ToString() + " " + Right.Evaluate() + ")"; 
        }
    }
}
