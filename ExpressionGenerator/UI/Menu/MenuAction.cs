using System;

namespace ExpressionGenerator.Menu
{
    /// <summary>
    /// Describes a single menu action
    /// </summary>
    internal class MenuAction
    {
        /// <summary>
        /// Id of the corresponding menu item
        /// </summary>
        public int ItemId { get; private set; }

        /// <summary>
        /// Action to be performed when this menu item is executed
        /// </summary>
        public Action Action { get; private set; }

        public MenuAction(int itemId, Action action)
        {
            ItemId = itemId;
            Action = action;
        }
    }
}
