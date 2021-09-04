using System;

namespace RocketExtensions.Plugins
{
    public sealed class AllowedCaller : Attribute
    {
        public Rocket.API.AllowedCaller Caller { get; private set; }

        public AllowedCaller(Rocket.API.AllowedCaller allowedCaller)
        {
            Caller = allowedCaller;
        }
    }
}