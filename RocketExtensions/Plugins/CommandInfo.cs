using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Unturned.Player;

namespace RocketExtensions.Plugins
{
    public sealed class CommandInfo : Attribute
    {
        public string Syntax { get; private set; }
        public string Help { get; private set; }

        public CommandInfo(string Help = "", string Syntax = "")
        {
            this.Syntax = Syntax;
            this.Help = Help;
        }
    }
}
