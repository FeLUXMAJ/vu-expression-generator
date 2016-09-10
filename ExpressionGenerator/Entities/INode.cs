using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator.Entities.ExpressionTree
{
    interface INode
    {
        INode Left { get; }
        INode Right { get; }
        INode Expand(int numberOfNewOperands = 2);
        string Evaluate();
    }
}
