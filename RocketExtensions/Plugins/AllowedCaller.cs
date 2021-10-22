using System;

namespace RocketExtensions.Plugins
{
    public class AllowedCallerAttribute : Attribute
    {
        public Rocket.API.AllowedCaller Caller { get; private set; }

        public const Rocket.API.AllowedCaller Player = Rocket.API.AllowedCaller.Player;
        public const Rocket.API.AllowedCaller Console = Rocket.API.AllowedCaller.Console;
        public const Rocket.API.AllowedCaller Both = Rocket.API.AllowedCaller.Both;

        public AllowedCallerAttribute(Rocket.API.AllowedCaller allowedCaller)
        {
            Caller = allowedCaller;
        }
    }
}