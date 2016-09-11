using System;
using ExpressionGenerator.ExpressionTree;
using System.Xml;

namespace ExpressionGenerator
{
    class ExpressionGenerator
    {
        private static ExpressionGenerator _instance = null;
        public  static Random Random = new Random();

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

        public void ReadSettingsFile()
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

        public string BuildExpression(int desiredValue, int numberOfOperands = 2)
        {
            var expressionTree = new Tree(desiredValue, numberOfOperands);
            return expressionTree.Expression;
        }
    }
}
