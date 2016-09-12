using ExpressionGenerator.ExpressionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator
{
    class ExpressionFormat : IEquatable<ExpressionFormat>
    {
        private Tree _expressionTree;
        private static int _globalIndex = 1;

        public string Format { get; private set; }
        public int Index { get; private set; }

        public ExpressionFormat(string infixExpression)
        {
            Index = _globalIndex++;
            Format = infixExpression;
            _expressionTree = ParseTree(infixExpression);
        }

        public string GetPopulatedExpression(int value)
        {
            _expressionTree.PopulateWithNumbers(value);
            string expression = _expressionTree.Expression;
            _expressionTree.Clear();
            return expression;
        }

        private Tree ParseTree(string expression)
        {
            var stack         = new Stack<Tree>(); //[[8]]
            var operatorStack = new Stack<char>();

            foreach(char ch in expression)
            {
                if (ch == '#')
                    stack.Push(new Tree());
                else if (ch == '(')
                    operatorStack.Push('(');
                else if (ch == ')')
                {
                    char c;
                    while ((c = operatorStack.Pop()) != '(')
                    {
                        Tree right = stack.Pop();
                        Tree left = stack.Pop();
                        stack.Push(Tree.Join(c, left, right));
                    }
                }
                else
                {
                    var current = new Operator(ch);
                    while(operatorStack.Count > 0 && operatorStack.Peek() != '(' && new Operator(operatorStack.Peek()).PrecedenceLevel >= current.PrecedenceLevel)
                    {
                        var topOperator = operatorStack.Pop();
                        Tree right = stack.Pop();
                        Tree left = stack.Pop();
                        stack.Push(Tree.Join(topOperator, left, right));
                    }
                    operatorStack.Push(ch);
                }
            }

            while(operatorStack.Count > 0)
            {
                char popped = operatorStack.Pop();
                Tree right = stack.Pop();
                Tree left = stack.Pop();
                stack.Push(Tree.Join(popped, left, right));
            }
            return stack.Pop();
        }

        public override int GetHashCode()
        {
            return Format.GetHashCode();
        }

        public bool Equals(ExpressionFormat other)
        {
            return Format == other.Format;
        }
    }
}
