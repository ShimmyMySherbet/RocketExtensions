using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using RocketExtensions.Models.Exceptions;
using RocketExtensions.Plugins;
using RocketExtensions.Utilities;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
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

        public RocketCommand Command { get; }

        public CommandContext(IRocketPlayer player, RocketCommand callingCommand, string[] args)
        {
            Command = callingCommand;
            Player = player;
            CommandRawArguments = args;
            Arguments = new ArgumentList(args);
        }

        /// <summary>
        /// Sends a message to the caller
        /// </summary>
        public async Task ReplyAsync(string message, Color? messageColor = null, bool rich = true)
        {
            if (messageColor == null)
            {
                messageColor = Color.green;
            }
            if (rich)
            {
                message = message.ReformatColor();
            }
            await ThreadTool.RunOnGameThreadAsync(UnturnedChat.Say, Player, message, messageColor.Value, rich);
        }

        /// <summary>
        /// Translates and sends the specified message to the command caller.
        /// </summary>
        /// <param name="translationKey">Translations key as set in your plugin's Translations</param>
        public async Task ReplyKeyAsync(string translationKey, params object[] arguments)
        {
            if (Command.Plugin == null)
            {
                throw new PluginNotFoundException($"Failed to find plugin instance for assembly {GetType().Assembly.GetName().Name}");
            }
            var translated = Command.Plugin.DefaultTranslations.Translate(translationKey, arguments);
            await ReplyAsync(translated);
        }

        /// <summary>
        /// Sends a message to the specified player
        /// </summary>
        public async Task SayAsync(IRocketPlayer player, string message, Color? messageColor = null, bool rich = true)
        {
            if (messageColor == null)
            {
                messageColor = Color.green;
            }
            if (rich)
            {
                message = message.ReformatColor();
            }
            await ThreadTool.RunOnGameThreadAsync(UnturnedChat.Say, player, message, messageColor.Value, rich);
        }

        /// <summary>
        /// Sends a message to all online players.
        /// </summary>
        public async Task AnnounceAsync(string message, Color? messageColor = null, bool rich = true)
        {
            if (messageColor == null)
            {
                messageColor = Color.green;
            }
            if (rich)
            {
                message = message.ReformatColor();
            }
            await ThreadTool.RunOnGameThreadAsync(UnturnedChat.Say, message, messageColor.Value, rich);
        }

        /// <summary>
        /// Translates and sends the specified message to the specified player.
        /// </summary>
        /// <param name="translationKey">Translations key as set in your plugin's Translations</param>
        public async Task SayKeyAsync(IRocketPlayer player, string translationKey, params object[] arguments)
        {
            if (Command.Plugin == null)
            {
                throw new PluginNotFoundException($"Failed to find plugin instance for assembly {GetType().Assembly.GetName().Name}");
            }

            var translated = Command.Plugin.DefaultTranslations.Translate(translationKey, arguments);
            await SayAsync(player, translated);
        }

        /// <summary>
        /// Translates and sends the specified message to all online players
        /// </summary>
        /// <param name="translationKey">Translations key as set in your plugin's Translations</param>
        public async Task AnnounceKeyAsync(string translationKey, params object[] arguments)
        {
            if (Command.Plugin == null)
            {
                throw new PluginNotFoundException($"Failed to find plugin instance for assembly {GetType().Assembly.GetName().Name}");
            }

            var translated = Command.Plugin.DefaultTranslations.Translate(translationKey, arguments);
            await AnnounceAsync(translated);
        }

        /// <summary>
        /// Cancels the command cooldown for the current command.
        /// </summary>
        /// <returns>True if the cooldown was found and cancled</returns>
        public async Task<bool> CancelCooldownAsync()
        {
            return await CooldownManager.CancelCooldownAsync(Player, Command);
        }

        /// <summary>
        /// Sets a command cooldown for the current command
        /// </summary>
        /// <param name="cooldown">Cooldown time in seconds</param>
        /// <returns>True if a new cooldown was created, or false if an existing one was updated</returns>
        public async Task<bool> SetCooldownAsync(uint cooldown)
        {
            return await CooldownManager.SetCooldownAsync(Player, Command, cooldown);
        }
    }
}