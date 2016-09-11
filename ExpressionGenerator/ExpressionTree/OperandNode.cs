namespace ExpressionGenerator.ExpressionTree
{
    class OperandNode : INode
    {

        public int?  Value { get; private set; }
        public INode Left { get { return null; } }
        public INode Right { get { return null; } }

        public OperandNode()
        {
            Value = null;
        }

        public OperandNode(int value)
        {
            Value = value;
        }

        public INode Expand(int numberOfNewOperands = 2)
        {
            if (numberOfNewOperands == 1 || Value == null)
                return this;
            return new OperatorNode(Value.Value, numberOfNewOperands);
        }

        public void SetValue(int value)
        {
            if (Value != null)
                return;
            Value = value;
        }

        public string Evaluate()
        {
            if (Value == null)
                return "#";
            return Value.ToString();
        }
    }
}
