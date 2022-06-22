using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Rocket.API;
using Rocket.Core;
using RocketExtensions.Models.Exceptions;

namespace RocketExtensions.Utilities
{
    internal static class CallingPluginUtility
    {
        public static IRocketPlugin GetCallingPlugin(bool throwErrors = true)
        {
            var pluginLookup = new Dictionary<string, IRocketPlugin>();

            foreach (var plugin in R.Plugins.GetPlugins())
            {
                var name = plugin.GetType().Assembly.GetName().Name;
                pluginLookup[name] = plugin;
            }

            var checkedAsms = new HashSet<Assembly>();

            var stack = new StackTrace();
            for (int i = 0; i < stack.FrameCount; i++)
            {
                var frame = stack.GetFrame(i);

                var asm = frame.GetMethod().DeclaringType.Assembly;

                if (checkedAsms.Contains(asm))
                {
                    continue;
                }

                var name = asm.GetName().Name;

                if (pluginLookup.TryGetValue(name, out var plugin))
                {
                    return plugin;
                }
            }

            if (throwErrors)
            {
                throw new PluginNotFoundException("Failed to find the calling plugin in the current stack.");
            }
            return null;
        }
    }
}