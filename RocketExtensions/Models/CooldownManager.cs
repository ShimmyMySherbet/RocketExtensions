using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Core.Commands;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;

namespace RocketExtensions.Models
{
    public static class CooldownManager
    {
        private static readonly Assembly a_RocketCore = typeof(R).Assembly;
        private static readonly Type t_Cooldown = a_RocketCore.GetTypes().FirstOrDefault(x => x.Name == "RocketCommandCooldown"); // internal class
        private static readonly FieldInfo f_Cooldowns = typeof(RocketCommandManager).GetField("cooldown", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo f_CooldownPlayer = t_Cooldown.GetField("Player", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo f_CooldownCommand = t_Cooldown.GetField("Command", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo f_CooldownPermission = t_Cooldown.GetField("ApplyingPermission", BindingFlags.Public | BindingFlags.Instance);
        private static IList Cooldowns => (IList)f_Cooldowns.GetValue(R.Commands);

        private static IRocketPlayer GetPlayer(object cooldown) => (IRocketPlayer)f_CooldownPlayer.GetValue(cooldown);

        private static IRocketCommand GetCommand(object cooldown) => (IRocketCommand)f_CooldownCommand.GetValue(cooldown);

        private static void SetPermission(object cooldown, Permission perm) => f_CooldownPermission.SetValue(cooldown, perm);

        /// <summary>
        /// Cancels a cooldown.
        /// Should be called from the game thread.
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

        /// <summary>
        /// Cancels a cooldown.
        /// Can be called from any thread.
        /// </summary>
        /// <param name="player">Target player</param>
        /// <param name="command">Target command</param>
        /// <returns>True if the cooldown was found and canceled</returns>
        public static async Task<bool> CancelCooldownAsync(IRocketPlayer player, IRocketCommand command) =>
            await ThreadTool.RunOnGameThreadAsync(CancelCooldown, player, command);

        /// <summary>
        /// Sets a cooldown for a command.
        /// To cancel a cooldown, use <see cref="CancelCooldown(IRocketPlayer, IRocketCommand)"/> instead.
        /// Should be called from the game thread
        /// </summary>
        /// <param name="player">Target player</param>
        /// <param name="command">Target command</param>
        /// <param name="cooldown">Cooldown in seconds</param>
        /// <returns>True if a new cooldown was created, or false if an existing cooldown was updated</returns>
        public static bool SetCooldown(IRocketPlayer player, IRocketCommand command, uint cooldown)
        {
            var perm = new Permission(command.Name, cooldown);

            // check for an active cooldown

            var cooldowns = Cooldowns;
            lock (cooldowns)
            {
                foreach (var cd in cooldowns)
                {
                    if (GetCommand(cd).Name.Equals(command.Name, StringComparison.InvariantCultureIgnoreCase) &&
                        GetPlayer(cd).Id.Equals(player.Id, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Found existing cooldown
                        SetPermission(cd, perm);
                        return false;
                    }
                }

                // No existing cooldown, create new

                var cdInst = Activator.CreateInstance(t_Cooldown, args: new object[] { player, command, perm });

                cooldowns.Add(cdInst);

                return true;
            }
        }

        public static async Task<bool> SetCooldownAsync(IRocketPlayer player, IRocketCommand command, uint cooldown) =>
            await ThreadTool.RunOnGameThreadAsync(SetCooldown, player, command, cooldown);
    }
}