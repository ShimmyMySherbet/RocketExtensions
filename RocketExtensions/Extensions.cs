using System;
using System.Linq;
using System.Reflection;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;

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
            if (typeof(T).IsAssignableFrom(instance.GetType()))
            {
                return (T)instance;
            }
            return default(T);
        }

        public static IRocketPlugin TryGetPlugin(this Assembly assembly, Type pluginType)
        {
            var instance = R.Plugins.GetPlugin(assembly);

            if (instance == null)
                return null;

            if (pluginType.IsAssignableFrom(instance.GetType()))
            {
                return instance;
            }
            return null;
        }

        public static RocketPlayer GetRocketPlayer(this SteamPlayer spl)
        {
            var plid = spl.playerID;
            return new RocketPlayer(plid.steamID.m_SteamID.ToString(), plid.characterName, spl.isAdmin);
        }

        public static bool HasPermission(this Player player, string permission)
        {
            return R.Permissions.HasPermission(player.channel.owner.GetRocketPlayer(), permission);
        }

        public static string[] GetPermissions(this Player player, string permission)
        {
            return R.Permissions.GetPermissions(player.channel.owner.GetRocketPlayer(), permission)
                .Select(x => x.Name)
                .ToArray();
        }

        public static bool HasPermission(this SteamPlayer player, string permission)
        {
            return R.Permissions.HasPermission(player.GetRocketPlayer(), permission);
        }

        public static string[] GetPermissions(this SteamPlayer player, string permission)
        {
            return R.Permissions.GetPermissions(player.GetRocketPlayer(), permission)
                .Select(x => x.Name)
                .ToArray();
        }
    }
}