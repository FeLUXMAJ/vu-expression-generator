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
            } while (!SplitHelper.SplitValue(goal, op, out left, out right));

            Operator = new Operator(op);
            Left  = new OperandNode(left);
            Right = new OperandNode(right);

            int toLeft, toRight;
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
                shouldEnclose |= right.Operator.PrecedenceLevel == Operator.PrecedenceLevel && Operator.IsStrongProcedence;
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
            Left.SetValue(left);
            Right.SetValue(right);
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
