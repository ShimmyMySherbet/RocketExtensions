using Rocket.API;
using Rocket.Core.Plugins;
using RocketExtensions.Plugins;
namespace TestPlugin
{
    [AllowedCaller(AllowedCaller.Both)]
    public class Plugin : RocketPlugin
    {
    }
}