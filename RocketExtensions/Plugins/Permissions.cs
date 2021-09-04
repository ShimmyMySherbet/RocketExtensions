using System;
using System.Collections.Generic;
using System.Linq;

namespace RocketExtensions.Plugins
{
    public sealed class Permissions : Attribute
    {
        public List<string> PermissionValues { get; private set; }

        public Permissions(params string[] perms)
        {
            PermissionValues = perms.ToList();
        }
    }
}