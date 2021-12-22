using Rocket.API;

namespace RocketExtensions.Plugins
{
    public abstract class PluginConfig : IRocketPluginConfiguration
    {
        public virtual void LoadDefaults()
        {
        }
    }
}