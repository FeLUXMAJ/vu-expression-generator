using ExpressionGenerator.ExpressionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator
{
    class ExpressionFormat
    {
        private Tree _expressionTree;

        public ExpressionFormat(string infixExpression)
        {
            _expressionTree = ParseTree(infixExpression);
            Console.WriteLine(_expressionTree.Expression);
        }

        private Tree ParseTree(string expression)
        {
            var stack         = new Stack<Tree>();
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
                    if(operatorStack.Count > 0)
                    {
                        char topOperator = operatorStack.Peek();
                        var inStack = new Operator(topOperator);
                        var newOp = new Operator(ch);
                        if (newOp.PrecedenceLevel <= inStack.PrecedenceLevel)
                        {
                            operatorStack.Pop();
                            Tree right = stack.Pop();
                            Tree left = stack.Pop();
                            stack.Push(Tree.Join(topOperator, left, right));
                        }
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
    }
}
