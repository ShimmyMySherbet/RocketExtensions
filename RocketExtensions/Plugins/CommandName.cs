using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketExtensions.Plugins
{
    public sealed class CommandName : Attribute
    {
        public string Name { get; private set; }

        public CommandName(string name)
        {
            Name = name;
        }

    }
}
