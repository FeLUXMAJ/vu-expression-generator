using ExpressionGenerator.ExpressionTree;
using System;
using System.Collections.Generic;

namespace ExpressionGenerator
{
    /// <summary>
    /// A class describing an expression format.
    /// Format rules:
    ///     1. Placeholders for numbers are denoted by a single '#'
    ///     2. There can't be two #'s in a row
    ///     3. There can't be two operators in a row
    ///     4. If there are brackets, they should be put in a valid manner (so no ')#+#(' or such formats are allowed)
    /// </summary>
    public class ExpressionFormat : IEquatable<ExpressionFormat> // [[13]]
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

        /// <summary>
        /// Replaces each # in the format with numbers, such that the
        /// final expression evaluates to the provided number.
        /// </summary>
        /// <param name="goal">Desired result of the final expression</param>
        /// <returns>Populated expression</returns>
        public string GetPopulatedExpression(int goal)
        {
            _expressionTree.PopulateWithNumbers(goal);
            string expression = _expressionTree.Expression;
            _expressionTree.Clear();
            return expression;
        }

        /// <summary>
        /// Parses the provided expression in string form into an expression tree.
        /// </summary>
        /// <param name="expression">Expression written in infix notation</param>
        /// <returns>Expression tree, which represents the given expression</returns>
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
                    // Pop everything until the first opening bracket
                    while ((c = operatorStack.Pop()) != '(')
                    {
                        Tree right = stack.Pop();
                        Tree left = stack.Pop();
                        stack.Push(Tree.Join(c, left, right));
                    }
                }
                // 'ch' is an operator
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

            // Empty the operator stack
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
