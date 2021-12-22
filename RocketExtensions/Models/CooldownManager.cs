using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Commands;

namespace RocketExtensions.Models
{
    public static class CooldownManager
    {
        private static readonly Assembly a_RocketCore = typeof(R).Assembly;
        private static readonly Type t_Cooldown = a_RocketCore.GetTypes().FirstOrDefault(x => x.Name == "RocketCommandCooldown"); // internal class
        private static readonly FieldInfo f_Cooldowns = typeof(RocketCommandManager).GetField("cooldown", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo f_CooldownPlayer = t_Cooldown.GetField("Player", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo f_CooldownCommand = t_Cooldown.GetField("Command", BindingFlags.Public | BindingFlags.Instance);
        private static IList Cooldowns => (IList)f_Cooldowns.GetValue(R.Commands);

        private static IRocketPlayer GetPlayer(object cooldown) => (IRocketPlayer)f_CooldownPlayer.GetValue(cooldown);

        private static IRocketCommand GetCommand(object cooldown) => (IRocketCommand)f_CooldownCommand.GetValue(cooldown);

        /// <summary>
        /// Cancels a cooldown
        /// </summary>
        /// <param name="player">Target player</param>
        /// <param name="command">Target command</param>
        /// <returns>True if the cooldown was found and canceled</returns>
        public static bool CancelCooldown(IRocketPlayer player, IRocketCommand command)
        {
            var cooldowns = Cooldowns;
            lock (cooldowns)
            {
                object cooldown = null;
                foreach (var cd in cooldowns)
                {
                    if (GetCommand(cd).Name.Equals(command.Name, StringComparison.InvariantCultureIgnoreCase) &&
                        GetPlayer(cd).Id.Equals(player.Id, StringComparison.InvariantCultureIgnoreCase))
                    {
                        cooldown = cd;
                        break;
                    }
                }

                if (cooldown == null)
                {
                    return false;
                }

                cooldowns.Remove(cooldown);
                return true;
            }
        }
    }
}