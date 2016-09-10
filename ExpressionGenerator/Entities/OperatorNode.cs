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
        private Operator _operator;
        private char[] operatorValues = { '-', '+', '*', '/' };
        public INode Left { get; private set; }

        public INode Right { get; private set; }

        public OperatorNode(int goal, int numberOfNewNodes = 2)
        {
            int left, right;
            char op;
            do
            {
                op = operatorValues[_random.Next(operatorValues.Length)];
            } while (!IsValidOperator(op, goal, out left, out right));

            _operator = new Operator(op);
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
            if(op == '-')
            {
                left = goal + 1;
                right = left - goal;
                return true;
            }

            if(op == '+')
            {
                left = goal - 1;
                right = goal - left;
                return true;
            }

            if (op == '*')
            {
                left = 1;
                right = goal;
                return true;
            }

            if(op == '/')
            {
                left = 2 * goal;
                right = 2;
                return true;
            }
            left = right = 0;
            return false;
        }

        public INode Expand(int numberOfNewOperands = 2)
        {
            return this;
        }

        public string Evaluate()
        {
            return "(" + Left.Evaluate() + " " + _operator.ToString() + " " + Right.Evaluate() + ")"; 
        }
    }
}
