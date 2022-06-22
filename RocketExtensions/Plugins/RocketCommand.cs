using Cysharp.Threading.Tasks;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using RocketExtensions.Core;
using RocketExtensions.Models;
using RocketExtensions.Models.Exceptions;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using RocketCaller = Rocket.API.AllowedCaller;

namespace RocketExtensions.Plugins
{
    public abstract class RocketCommand : IRocketCommand
    {
        private AllowedCallerAttribute m_AllowedCaller;
        private IRocketPlugin m_Plugin;
        private bool m_PluginInit = false;

        public IRocketPlugin Plugin
        {
            get
            {
                if (!m_PluginInit)
                {
                    m_Plugin = R.Plugins.GetPlugin(GetType().Assembly);
                    m_PluginInit = true;
                }
                return m_Plugin;
            }
        }

        public RocketCaller AllowedCaller
        {
            get
            {
                var typ = GetType();
                if (m_AllowedCaller == null)
                {
                    m_AllowedCaller = typ.GetCustomAttribute<AllowedCallerAttribute>();
                    if (m_AllowedCaller == null)
                    {
                        m_AllowedCaller = new AllowedCallerAttribute(RocketCaller.Both);
                    }
                }

                return m_AllowedCaller.Caller;
            }
        }

        private string m_Name;

        public string Name
        {
            get
            {
                if (m_Name == null)
                {
                    var typ = GetType();

                    var nm = typ.GetCustomAttribute<CommandName>();
                    if (nm != null)
                    {
                        m_Name = nm.Name;
                    }
                    else
                    {
                        var className = typ.Name;

                        var commandstrInex = className.IndexOf("command", 0, StringComparison.InvariantCultureIgnoreCase);

                        if (commandstrInex != -1)
                        {
                            className = className.Remove(commandstrInex, 7);
                        }

                        m_Name = className;
                    }
                }

                return m_Name;
            }
        }

        private string m_Help;
        private string m_Syntax;

        private void m_InitInfo()
        {
            var typ = GetType();

            var info = typ.GetCustomAttribute<CommandInfo>();

            if (info != null)
            {
                m_Help = info.Help;

                if (!string.IsNullOrEmpty(info.Syntax))
                {
                    m_Syntax = info.Syntax;
                }
                else
                {
                    m_Syntax = Name;
                }
            }
            else
            {
                m_Help = "";
                m_Syntax = Name;
            }
        }

        public string Help
        {
            get
            {
                if (m_Help == null)
                {
                    m_InitInfo();
                }
                return m_Help;
            }
        }

        public string Syntax
        {
            get
            {
                if (m_Syntax == null)
                {
                    m_InitInfo();
                }
                return m_Syntax;
            }
        }

        private List<string> m_Aliases = null;

        public List<string> Aliases
        {
            get
            {
                if (m_Aliases == null)
                {
                    var info = GetType().GetCustomAttribute<Aliases>();
                    if (info != null)
                    {
                        m_Aliases = info.AliasList;
                    }
                    else
                    {
                        m_Aliases = new List<string>();
                    }
                }
                return m_Aliases;
            }
        }

        private List<string> m_Permissions = new List<string>();

        public List<string> Permissions
        {
            get
            {
                var typ = GetType();
                var inst = typ.GetCustomAttribute<Permissions>();
                if (inst != null)
                {
                    m_Permissions = inst.PermissionValues;
                }
                else
                {
                    var asmName = typ.Assembly.GetName().Name;

                    m_Permissions = new List<string>() { $"{asmName}.{Name}" };
                }

                return m_Permissions;
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            CoreSetup.CheckInit();
            var context = new CommandContext(caller, this, command);
            UniTask.Run(async () => await Run(context));
        }

        private async UniTask Run(CommandContext context)
        {
            await UniTask.SwitchToThreadPool();

            try
            {
                await Execute(context);
            }
            catch (InvalidArgumentException invalid)
            {
                await context.ReplyAsync(invalid.Message, UnityEngine.Color.red);
                await context.ReplyAsync($"Command Usage: /{Name} {Syntax}", UnityEngine.Color.cyan);
                await context.CancelCooldownAsync();
            }
            catch (PlayerNotFoundException player)
            {
                await context.ReplyAsync(player.Message, UnityEngine.Color.red);
                await context.CancelCooldownAsync();
            }
            catch (ArgumentMissingException missing)
            {
                await context.ReplyAsync(missing.Message, UnityEngine.Color.red);
                await context.ReplyAsync($"Command Usage: /{Name} {Syntax}", UnityEngine.Color.cyan);
                await context.CancelCooldownAsync();
            }
            catch (WrongUsageOfCommandException usage)
            {
                await UniTask.SwitchToMainThread();
                await context.CancelCooldownAsync();
                throw usage;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error while executing /{Name}");
                Logger.LogError($"[{ex.GetType().FullName}] {ex.Message}");
                Logger.LogError(ex.StackTrace);
                await context.ReplyAsync("<color=red>An error occurred during the execution of this command</color>");
            }
        }

        public T GetPluginInstance<T>() where T : IRocketPlugin
        {
            return typeof(T).Assembly.TryGetPlugin<T>();
        }

        public T GetPluginConfig<T>() where T : IRocketPluginConfiguration
        {
            var cType = typeof(RocketPlugin<>).MakeGenericType(typeof(T));
            dynamic plugin = typeof(T).Assembly.TryGetPlugin(cType);
            if (plugin == null)
            {
                return default(T);
            }
            var configInstance = plugin.Configuration.Instance;

            if (configInstance == null)
            {
                return default(T);
            }

            if (configInstance is T t)
            {
                return t;
            }

            if (typeof(T).IsAssignableFrom(configInstance.GetType()))
            {
                return (T)configInstance;
            }
            return default(T);
        }

        public abstract UniTask Execute(CommandContext context);

        /// <summary>
        /// Sends a message to the specified player
        /// </summary>
        public async Task SayAsync(IRocketPlayer player, string message, Color? messageColor = null, bool rich = true)
        {
            if (messageColor == null)
            {
                messageColor = Color.green;
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
            await ThreadTool.RunOnGameThreadAsync(UnturnedChat.Say, message, messageColor.Value, rich);
        }

        /// <summary>
        /// Translates and sends the specified message to the specified player.
        /// </summary>
        /// <param name="translationKey">Translations key as set in your plugin's Translations</param>
        public async Task SayKeyAsync(IRocketPlayer player, string translationKey, params object[] arguments)
        {
            if (Plugin == null)
            {
                throw new PluginNotFoundException($"Failed to find plugin instance for assembly {GetType().Assembly.GetName().Name}");
            }

            var translated = Plugin.Translations.Instance.Translate(translationKey, arguments);
            await SayAsync(player, translated);
        }

        /// <summary>
        /// Translates and sends the specified message to all online players
        /// </summary>
        /// <param name="translationKey">Translations key as set in your plugin's Translations</param>
        public async Task AnnounceKeyAsync(IRocketPlayer player, string translationKey, params object[] arguments)
        {
            if (Plugin == null)
            {
                throw new PluginNotFoundException($"Failed to find plugin instance for assembly {GetType().Assembly.GetName().Name}");
            }

            var translated = Plugin.Translations.Instance.Translate(translationKey, arguments);
            await AnnounceAsync(translated);
        }
    }
}