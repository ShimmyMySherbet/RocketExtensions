using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using System.Threading.Tasks;
using UnityEngine;

namespace RocketExtensions.Models
{
    /// <summary>
    /// Contains the context for command execution
    /// </summary>
    public struct CommandContext
    {
        public IRocketPlayer Player { get; private set; }
        public string[] CommandRawArguments { get; private set; }
        public ArgumentList Arguments { get; private set; }

        /// <summary>
        /// The caller as an UnturnedPlayer. Null when called from console.
        /// </summary>
        public UnturnedPlayer UnturnedPlayer => Player as UnturnedPlayer ?? null;

        public IRocketCommand Command { get; }

        public CommandContext(IRocketPlayer player, IRocketCommand callingCommand, string[] args)
        {
            Command = callingCommand;
            Player = player;
            CommandRawArguments = args;
            Arguments = new ArgumentList(args);
        }

        /// <summary>
        /// Sends a message to the caller
        /// </summary>
        public async Task ReplyAsync(string message, Color? messageColor = null)
        {
            if (messageColor == null)
            {
                messageColor = Color.green;
            }
            await ThreadTool.RunOnGameThreadAsync(UnturnedChat.Say, Player, message, messageColor.Value);
        }

        /// <summary>
        /// Sends a message to the specified player
        /// </summary>
        public async Task SayAsync(IRocketPlayer player, string message, Color? messageColor = null)
        {
            if (messageColor == null)
            {
                messageColor = Color.green;
            }
            await ThreadTool.RunOnGameThreadAsync(UnturnedChat.Say, player, message, messageColor.Value);
        }

        /// <summary>
        /// Sends a message to all online players.
        /// </summary>
        public async Task Announceasync(string message, Color? messageColor = null)
        {
            if (messageColor == null)
            {
                messageColor = Color.green;
            }
            await ThreadTool.RunOnGameThreadAsync(UnturnedChat.Say, message, messageColor.Value);
        }

        /// <summary>
        /// Cancels the command cooldown for the current command.
        /// </summary>
        public async Task<bool> SendCancelCooldown()
        {
            return await ThreadTool.RunOnGameThreadAsync(CooldownManager.CancelCooldown, Player, Command);
        }
    }
}