using System;

namespace RocketExtensions.Models.Exceptions
{
    public sealed class PluginNotFoundException : Exception
    {
        public PluginNotFoundException(string msg) : base(msg)
        {
        }
    }
}