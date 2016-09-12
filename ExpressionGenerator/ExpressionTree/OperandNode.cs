namespace ExpressionGenerator.ExpressionTree
{
    /// <summary>
    /// Operand node of an expression tree.
    /// Holds the numerical value of a node.
    /// </summary>
    internal class OperandNode : INode
    {
        /// <summary>
        /// The numerical value of this node.
        /// Null if this node is part of a format (so its value is #)
        /// </summary>
        public int?  Value { get; private set; }
        public INode Left { get { return null; } }
        public INode Right { get { return null; } }

        /// <summary>
        /// Instantiate an empty operand node (represent the # in a format)
        /// </summary>
        public OperandNode()
        {
            Value = null;
        }

        /// <summary>
        /// Instantiate an operand node with the specified value
        /// </summary>
        /// <param name="value">The value of this operand node</param>
        public OperandNode(int value)
        {
            Value = value;
        }
        
        public INode Expand(int numberOfNewOperands = 2)
        {
            // If the node cant be split, ignore the request
            if (numberOfNewOperands == 1 || Value == null)
                return this;
            return new OperatorNode(Value.Value, numberOfNewOperands);
        }
        
        public void SetValue(int value, bool presetFormat)
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

        public int? GetValue()
        {
            return Value;
        }

        public void Clear()
        {
            Value = null;
        }
    }
}
