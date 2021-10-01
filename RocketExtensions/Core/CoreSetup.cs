using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine.LowLevel;

namespace RocketExtensions.Core
{
    internal static class CoreSetup
    {
        private static bool m_Initialized { get; set; } = false;

        internal static void CheckInit()
        {
            if (!m_Initialized)
            {
                m_Initialized = true;
                Init();
            }
        }

        private static void Init()
        {
            // Origonal from https://github.com/openmod/openmod/blob/main/unityengine/OpenMod.UnityEngine/UnityHostLifetime.cs
            // Origonal Author: Trojaner

            if (!IsOpenmodPresent())
            {
                var unitySynchronizationContextField = typeof(PlayerLoopHelper).GetField("unitySynchronizationContext", BindingFlags.Static | BindingFlags.NonPublic);

                unitySynchronizationContextField.SetValue(null, SynchronizationContext.Current);

                var mainThreadIdField =
                    typeof(PlayerLoopHelper).GetField("mainThreadId", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Could not find PlayerLoopHelper.mainThreadId field");
                mainThreadIdField.SetValue(null, Thread.CurrentThread.ManagedThreadId);

                var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
                PlayerLoopHelper.Initialize(ref playerLoop);
            }
        }

        public static bool IsOpenmodPresent() => AppDomain.CurrentDomain.GetAssemblies().Any(x => x.GetName().Name == "OpenMod.Core");
    }
}