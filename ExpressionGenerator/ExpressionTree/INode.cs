namespace ExpressionGenerator.ExpressionTree
{
    /// <summary>
    /// Single node of an expression tree
    /// </summary>
    internal interface INode
    {
        /// <summary>
        /// Left child of this node
        /// </summary>
        INode Left { get; }

        /// <summary>
        /// Right child of this node
        /// </summary>
        INode Right { get; }

        /// <summary>
        /// Split this node into two new ones (if possible) and
        /// continue splitting recursively.
        /// </summary>
        /// <param name="numberOfNewOperands">Number of new operands to get after completely splitting up this node</param>
        /// <returns>The parent node after the splitting is complete</returns>
        INode Expand(int numberOfNewOperands = 2);

        /// <summary>
        /// Get numerical value of this node
        /// </summary>
        /// <returns>Value of node</returns>
        int? GetValue();

        /// <summary>
        /// Set the value of this node
        /// </summary>
        /// <param name="value">New value</param>
        /// <param name="presetFormat">Specifies, whether this node is part of a format or not</param>
        void SetValue(int value, bool presetFormat = false);

        /// <summary>
        /// Clears the value of this node
        /// </summary>
        void Clear();

        /// <summary>
        /// Evaluates the node and its children into an expression
        /// </summary>
        /// <returns>The expression which this node represents</returns>
        string Evaluate();
    }
}
