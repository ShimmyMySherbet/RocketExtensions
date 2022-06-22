using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using RocketExtensions.Plugins;

namespace TestPlugin
{
    public class TestPlugin : RocketPlugin
    {
        public override void LoadPlugin()
        {
            base.LoadPlugin();
            Logger.Log("Loaded!");

        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "Give_Player_XP", "[color=green]Giving {0} {1}xp...[/color]"},
            { "Give_Player_XP_Fail", "[color=red]XP cannot be negative[/color]"},
        };
    }
}