using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketExtensions.Plugins
{
    public sealed class Aliases : Attribute
    {
        public List<string> AliasList { get; private set; }

        public Aliases(params string[] aliases)
        {
            AliasList = aliases.ToList();
        }
    }
}