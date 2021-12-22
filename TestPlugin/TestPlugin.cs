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
            //var asm = typeof(R)?.Assembly?.GetType("Rocket.Core.Commands.RocketCommandCooldown");
            //Console.WriteLine($"via f: {asm != null}");

            //var t = Type.GetType("Rocket.Core.Commands.RocketCommandCooldown");
            //Console.WriteLine($"HT: {t != null}");
            //t = Type.GetType("Rocket.Core.Rocket.Core.Commands.RocketCommandCooldown");
            //Console.WriteLine($"HT: {t != null}");

            //var asmr = typeof(R).Assembly;

            //var tt = asmr.GetTypes().FirstOrDefault(x => x.Name == "RocketCommandCooldown");
            //Console.WriteLine($"VIAD: {tt != null}");
            
            //Console.ReadLine();
            Logger.Log("Loaded!");




        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "Give_Player_XP", "[color=green]Giving {0} {1}xp...[/color]"},
            { "Give_Player_XP_Fail", "[color=red]XP cannot be negative[/color]"},
        };
    }
}