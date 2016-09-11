namespace ExpressionGenerator.ExpressionTree
{
    interface INode
    {
        INode Left { get; }
        INode Right { get; }
        INode Expand(int numberOfNewOperands = 2);
        int? GetValue();
        void SetValue(int value);
        string Evaluate();
    }
}
