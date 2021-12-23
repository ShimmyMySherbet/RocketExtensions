using Cysharp.Threading.Tasks;
using Rocket.API;
using RocketExtensions.Models;
using RocketExtensions.Plugins;
using SDG.Unturned;

namespace TestPlugin
{
    [CommandInfo(Syntax: "[TargetPlyer] [Amount]"), AllowedCaller(AllowedCaller.Both)]
    public class GiveExperienceCommand : RocketCommand
    {
        public override async UniTask Execute(CommandContext context)
        {
            var player = context.Arguments.Get<Player>(0, paramName: "Target Player");
            var amount = context.Arguments.Get(1, defaultValue: 1000u, paramName: "Amount");

            await context.ReplyKeyAsync("Give_Player_XP", player.name, amount);

            var newAmt = player.skills.experience + amount;

            await UniTask.SwitchToMainThread();

            player.skills.ServerSetExperience(newAmt);

        }
    }
}