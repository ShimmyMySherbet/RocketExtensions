using Cysharp.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Player;
using RocketExtensions.Models;
using RocketExtensions.Plugins;

namespace TestPlugin
{
    [AllowedCaller(AllowedCaller.Player)]
    [CommandInfo("Grants XP to a player", "[Player] [Amount]")]
    [CommandName("TXP")]
    public class TestXPCommand : RocketCommand
    {
        public override async UniTask Execute(CommandContext context)
        {
            var targetPlayer = context.Arguments.Get<UnturnedPlayer>(0, paramName: "Player");
            var xp = context.Arguments.Get(1, 100, paramName: "XP");

            if (xp < 0)
            {
                await context.ReplyKeyAsync("Give_Player_XP_Fail");
                await context.CancelCooldownAsync();
                return;
            }
            else
            {
                await context.ReplyKeyAsync("Give_Player_XP", targetPlayer.DisplayName, xp);
            }
            await UniTask.SwitchToMainThread();
            targetPlayer.Experience += (uint)xp;
        }
    }
}