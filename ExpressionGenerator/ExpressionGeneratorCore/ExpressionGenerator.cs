using System;
using ExpressionGenerator.ExpressionTree;
using System.Xml;

namespace ExpressionGenerator
{
    /// <summary>
    /// The core class for generating expressions.
    /// Designed after the singleton pattern.
    /// </summary>
    public class ExpressionGenerator
    {
        private static ExpressionGenerator _instance = null;
        public  static Random Random = new Random();

        /// <summary>
        /// Get the instance of the ExpressionGenerator
        /// </summary>
        public static ExpressionGenerator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ExpressionGenerator();
                return _instance;
            }
        }

        private ExpressionGenerator()
        {
            ReadSettingsFile();
        }

        /// <summary>
        /// Generate an expression for a given format.
        /// </summary>
        /// <param name="format">The format of which the final expression should be</param>
        /// <param name="goal">The value of the final expression when evaluated</param>
        /// <returns>Expression designed after the specified format and with the specified value</returns>
        public string FromFormat(ExpressionFormat format, int goal)
        {
            return format.GetPopulatedExpression(goal);
        }

        /// <summary>
        /// Read global program settings from an XML file
        /// </summary>
        public void ReadSettingsFile() //[[7]]
        {
            var doc = new XmlDocument();
            doc.Load("Settings.xml");

            var root = doc.DocumentElement.SelectSingleNode("/Root");

            foreach (XmlNode operatorNode in root.SelectSingleNode("AllowedOperations").ChildNodes)
            {
                switch (operatorNode.InnerText)
                {
                    case "+":
                        Configuration.AllowedOperators |= Configuration.Operators.ADD;
                        break;
                    case "-":
                        Configuration.AllowedOperators |= Configuration.Operators.SUB;
                        break;
                    case "*":
                        Configuration.AllowedOperators |= Configuration.Operators.MUL;
                        break;
                    case "/":
                        Configuration.AllowedOperators |= Configuration.Operators.DIV;
                        break;
                }
            }

            Configuration.MaxOperandValue = int.Parse(root.SelectSingleNode("MaxOperandSize").InnerText);
            Configuration.AllowZero       = root.SelectSingleNode("AllowZero").InnerText == "1" ? true : false;
        }

        /// <summary>
        /// Builds an expression which evaluates to the specified
        /// value and has the specified number of operands.
        /// </summary>
        /// <param name="desiredValue">The value of the final expression when evaluated</param>
        /// <param name="numberOfOperands">The number of operands in the final expression</param>
        /// <returns>Final expression</returns>
        public string BuildExpression(int desiredValue, int numberOfOperands = 2)
        {
            var expressionTree = new Tree(desiredValue, numberOfOperands);
            return expressionTree.Expression;
        }
    }
}
