namespace ExpressionGenerator.Entities.ExpressionTree
{
    internal class Operator
    {
        private char _operator;
        public int PrecedenceLevel { get; private set; }

        public Operator(char op)
        {
            _operator = op;
            if (op == '+' || op == '-')
                PrecedenceLevel = 1;
            else if (op == '*' || op == '/')
                PrecedenceLevel = 2;
        }

        public override string ToString()
        {
            return _operator.ToString();
        }
    }
}