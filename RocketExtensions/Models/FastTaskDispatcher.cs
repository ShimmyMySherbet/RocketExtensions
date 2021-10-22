using RocketExtensions.Core;
using SDG.Unturned;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Action = System.Action;
using Logger = Rocket.Core.Logging.Logger;

namespace RocketExtensions.Models
{
    /// <summary>
    /// A faster high-performance version of <seealso cref="Rocket.Core.Utils.TaskDispatcher"/>
    /// </summary>
    public class FastTaskDispatcher : MonoBehaviour
    {
        private static bool m_Initialized { get; set; } = false;
        private static GameObject m_Object { get; set; }
        private static FastTaskDispatcher m_Component { get; set; }

        private static void CheckInit()
        {
            if (!m_Initialized)
            {
                m_Initialized = true;
                m_Object = new GameObject("RocketExtensions.FastTaskDispatcher");
                GameObject.DontDestroyOnLoad(m_Object);
                m_Component = m_Object.AddComponent<FastTaskDispatcher>();
            }
            CoreSetup.CheckInit();
        }

        /// <summary>
        /// Queues the action onto the game thread. Or, if called from game thread runs it immediately 
        /// </summary>
        public static void QueueOnMainThread(Action action)
        {
            CheckInit();
            if (Thread.CurrentThread.IsGameThread())
            {
                action();
            }
            else
            {
                lock (m_Component.m_Queue)
                {
                    m_Component.m_Queue.Add(action);
                    m_Component.m_QueueLength++;
                }
            }
        }

        private int m_QueueLength { get; set; } = 0;
        private List<Action> m_Queue { get; set; } = new List<Action>();

        public void FixedUpdate()
        {
            while (m_QueueLength > 0)
            {
                Action act = null;
                lock (m_Queue)
                {
                    m_QueueLength -= 1;
                    act = m_Queue[0];
                    m_Queue.RemoveAtFast(0);
                }

                try
                {
                    act();
                }
                catch (System.Exception ex)
                {
                    Logger.LogError($"Task Dispatcher Error: {ex.Message}");
                    Logger.LogError(ex.StackTrace);
                }
            }
        }
    }
}