using Cysharp.Threading.Tasks;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using RocketExtensions.Plugins;

namespace TestPlugin
{
    public class Plugin : ExtendedRocketPlugin
    {
        public override void LoadPlugin()
        {
            base.LoadPlugin();
            Logger.Log("Loaded!");
        }

        public override UniTask LoadAsync()
        {
            Logger.Log("Loaded! (async)");
            return base.LoadAsync();
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "Give_Player_XP", "Giving {0} {1}xp..."}
        };
    }
}