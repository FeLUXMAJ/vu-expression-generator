using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator.Menu
{
    internal class MenuAction
    {
        public int ItemId { get; private set; }
        public Action Action { get; private set; }

        public MenuAction(int itemId, Action action)
        {
            ItemId = itemId;
            Action = action;
        }
    }
}
