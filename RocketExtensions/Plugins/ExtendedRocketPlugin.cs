//using Cysharp.Threading.Tasks;
//using Rocket.API;
//using Rocket.Core.Logging;
//using Rocket.Core.Plugins;
//using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
//using System;





// issues with UniTask init needs to be solved









//namespace RocketExtensions.Plugins
//{
//    public abstract class ExtendedRocketPlugin : RocketPlugin, IRocketPlugin
//    {
//        public override void LoadPlugin()
//        {
//            base.LoadPlugin();
//            UniTask.RunOnThreadPool(RunLoadAsync);
//        }

//        public void LogError(Exception e)
//        {
//            var asm = GetType().Assembly.GetName();
//            Logger.LogError($"Error: {e.Message}");
//            Logger.LogError($"Plugin: {asm.Name} v{asm.Version}");
//            Logger.LogError($"Source: {e.Source}");
//            Logger.LogError($"{e.StackTrace}");

//            if (e.InnerException != null)
//            {
//                Logger.LogError($"Inner: {e.InnerException.Message}");
//                Logger.LogError($"{e.InnerException.StackTrace}");
//            }
//        }

//        private async UniTask RunLoadAsync()
//        {
//            await UniTask.SwitchToThreadPool();
//            try
//            {
//                await LoadAsync();
//            }
//            catch (Exception e)
//            {
//                Logger.LogError($"An exception occurred during plugin load (async)");
//                LogError(e);
//                await UnloadAsync(PluginState.Failure);
//            }
//        }

//        public async UniTask UnloadAsync(PluginState state = PluginState.Unloaded)
//        {
//            await ThreadTool.RunOnGameThreadAsync(UnloadPlugin, state);
//        }

//        public virtual UniTask LoadAsync()
//        {
//            return UniTask.CompletedTask;
//        }
//    }
//}