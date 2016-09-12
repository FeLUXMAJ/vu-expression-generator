using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator.Menu
{
    internal class MenuItem
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public IEnumerable<string> Aliases { get; private set; }

        public MenuItem(int id, string description, IEnumerable<string> aliases)
        {
            Id = id;
            Description = description;
            Aliases = aliases;
        }
    }
}
