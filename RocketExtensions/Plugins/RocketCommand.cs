using Cysharp.Threading.Tasks;
using Rocket.API;
using Rocket.Core.Logging;
using RocketExtensions.Core;
using RocketExtensions.Models;
using RocketExtensions.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using RocketCaller = Rocket.API.AllowedCaller;

namespace RocketExtensions.Plugins
{
    public abstract class RocketCommand : IRocketCommand
    {
        private AllowedCallerAttribute m_AllowedCaller;

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
            var context = new CommandContext(caller, command);
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
            }
            catch (PlayerNotFoundException player)
            {
                await context.ReplyAsync(player.Message, UnityEngine.Color.red);
            }
            catch (ArgumentMissingException missing)
            {
                await context.ReplyAsync(missing.Message, UnityEngine.Color.red);
                await context.ReplyAsync($"Command Usage: /{Name} {Syntax}", UnityEngine.Color.cyan);
            }
            catch (WrongUsageOfCommandException usage)
            {
                await UniTask.SwitchToMainThread();
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

        public abstract UniTask Execute(CommandContext context);
    }
}