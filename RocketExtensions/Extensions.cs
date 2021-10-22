using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Reflection;

namespace RocketExtensions
{
    public static class Extensions
    {
        public static bool PlayerIsOnline(this IRocketPlayer rocketPlayer)
        {
            if (rocketPlayer == null)
            {
                return false;
            }

            if (rocketPlayer is ConsolePlayer)
                return true;

            if (rocketPlayer is UnturnedPlayer up)
                return up.Player != null && up.Player.enabled;

            return false;
        }

        public static bool PlayerIsOnline(this Player player)
        {
            if (player == null)
            {
                return false;
            }
            return player.enabled;
        }

        public static T TryGetPlugin<T>(this Assembly assembly) where T : IRocketPlugin
        {
            var instance = R.Plugins.GetPlugin(assembly);

            if (instance == null)
                return default(T);

            if (instance is T t)
            {
                return t;
            }
            return default(T);
        }
    }
}