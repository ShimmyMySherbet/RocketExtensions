using Cysharp.Threading.Tasks;
using RocketExtensions.Models;
using RocketExtensions.Plugins;
using SDG.Unturned;

namespace TestPlugin
{
    [CommandInfo(Syntax: "[TargetPlyer] [Amount]")]
    public class GiveExperienceCommand : RocketCommand
    {
        public override async UniTask Execute(CommandContext context)
        {
            var player = context.Arguments.Get<Player>(0, paramName: "Target Player");
            var amount = context.Arguments.Get(1, defaultValue: 1000u, paramName: "Amount");

            await context.ReplyAsync($"Giving {player.name} {amount}xp...");

            var newAmt = player.skills.experience + amount;

            await UniTask.SwitchToMainThread();

            player.skills.ServerSetExperience(newAmt);
        }
    }
}