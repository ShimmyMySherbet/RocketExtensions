using Rocket.API;
using Rocket.Unturned.Chat;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using System.Threading.Tasks;

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

        public CommandContext(IRocketPlayer player, string[] args)
        {
            Player = player;
            CommandRawArguments = args;
            Arguments = new ArgumentList(args);
        }

        public async Task ReplyAsync(string message)
        {
            await ThreadTool.RunOnGameThreadAsync((IRocketPlayer player, string msg) =>
            {
                UnturnedChat.Say(player, msg, true);
            }, Player, message);
        }
    }
}