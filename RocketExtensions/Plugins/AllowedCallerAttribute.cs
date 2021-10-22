using Rocket.API;
using System;

namespace RocketExtensions.Plugins
{
    public sealed class AllowedCallerAttribute : Attribute
    {
        public const AllowedCaller Console = AllowedCaller.Console;
        public const AllowedCaller Player = AllowedCaller.Player;
        public const AllowedCaller Both = AllowedCaller.Both;
        public Rocket.API.AllowedCaller Caller { get; private set; }

        public AllowedCallerAttribute(Rocket.API.AllowedCaller allowedCaller)
        {
            Caller = allowedCaller;
        }
    }
}