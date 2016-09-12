using System.Collections.Generic;

namespace ExpressionGenerator.Menu
{
    /// <summary>
    /// Describes a single menu item
    /// </summary>
    internal class MenuItem
    {
        /// <summary>
        /// Id of this item
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Short description of the item
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Aliases of the command to execute this item
        /// </summary>
        public IEnumerable<string> Aliases { get; private set; }

        public MenuItem(int id, string description, IEnumerable<string> aliases)
        {
            Id = id;
            Description = description;
            Aliases = aliases;
        }
    }
}
