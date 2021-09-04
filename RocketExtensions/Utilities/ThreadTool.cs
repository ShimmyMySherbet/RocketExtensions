namespace RocketExtensions.Utilities
{
    using RocketExtensions.Models;
    using SDG.Unturned;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    namespace ShimmyMySherbet.Extensions
    {
        /// <summary>
        /// A collection of embedded tools to help manage threading and async operations
        /// </summary>
        public static class ThreadTool
        {
            public delegate void VoidPattern();

            public delegate void VoidPattern<A>(A arg1);

            public delegate void VoidPattern<A, B>(A arg1, B arg2);

            public delegate void VoidPattern<A, B, C>(A arg1, B arg2, C arg3);

            public delegate void VoidPattern<A, B, C, D>(A arg1, B arg2, C arg3, D arg4);

            public delegate void VoidPattern<A, B, C, D, E>(A arg1, B arg2, C arg3, D arg4, E arg5);

            public delegate Task TaskPattern();

            public delegate Task TaskPattern<A>(A arg1);

            public delegate Task TaskPattern<A, B>(A arg1, B arg2);

            public delegate Task TaskPattern<A, B, C>(A arg1, B arg2, C arg3);

            public delegate Task TaskPattern<A, B, C, D>(A arg1, B arg2, C arg3, D arg4);

            public delegate Task TaskPattern<A, B, C, D, E>(A arg1, B arg2, C arg3, D arg4, E arg5);

            public delegate T FuncPattern<T>();

            public delegate T FuncPattern<T, A>(A arg1);

            public delegate T FuncPattern<T, A, B>(A arg1, B arg2);

            public delegate T FuncPattern<T, A, B, C>(A arg1, B arg2, C arg3);

            public delegate T FuncPattern<T, A, B, C, D>(A arg1, B arg2, C arg3, D arg4);

            public delegate T FuncPattern<T, A, B, C, D, E>(A arg1, B arg2, C arg3, D arg4, E arg5);

            public static async Task RunOnGameThreadAsync(VoidPattern action)
            {
                var callback = new TaskCompletionSource<Exception>();

                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        action();
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });

                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
            }

            public static async Task RunOnGameThreadAsync<A>(VoidPattern<A> action, A arg1)
            {
                var callback = new TaskCompletionSource<Exception>();

                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        action(arg1);
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });

                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
            }

            public static async Task RunOnGameThreadAsync<A, B>(VoidPattern<A, B> action, A arg1, B arg2)
            {
                var callback = new TaskCompletionSource<Exception>();

                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        action(arg1, arg2);
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });

                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
            }

            public static async Task RunOnGameThreadAsync<A, B, C>(VoidPattern<A, B, C> action, A arg1, B arg2, C arg3)
            {
                var callback = new TaskCompletionSource<Exception>();

                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        action(arg1, arg2, arg3);
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });

                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
            }

            public static async Task RunOnGameThreadAsync<A, B, C, D>(VoidPattern<A, B, C, D> action, A arg1, B arg2, C arg3, D arg4)
            {
                var callback = new TaskCompletionSource<Exception>();

                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        action(arg1, arg2, arg3, arg4);
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });

                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
            }

            public static async Task RunOnGameThreadAsync<A, B, C, D, E>(VoidPattern<A, B, C, D, E> action, A arg1, B arg2, C arg3, D arg4, E arg5)
            {
                var callback = new TaskCompletionSource<Exception>();

                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        action(arg1, arg2, arg3, arg4, arg5);
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });

                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
            }

            public static async Task<T> RunOnGameThreadAsync<T>(FuncPattern<T> action)
            {
                var callback = new TaskCompletionSource<Exception>();
                T argue = default(T);
                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        argue = action();
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });

                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
                return argue;
            }

            public static async Task<T> RunOnGameThreadAsync<T, A>(FuncPattern<T, A> action, A arg1)
            {
                var callback = new TaskCompletionSource<Exception>();
                T argue = default(T);
                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        argue = action(arg1);
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });

                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
                return argue;
            }

            public static async Task<T> RunOnGameThreadAsync<T, A, B>(FuncPattern<T, A, B> action, A arg1, B arg2)
            {
                var callback = new TaskCompletionSource<Exception>();
                T argue = default(T);
                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        argue = action(arg1, arg2);
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });

                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
                return argue;
            }

            public static async Task<T> RunOnGameThreadAsync<T, A, B, C>(FuncPattern<T, A, B, C> action, A arg1, B arg2, C arg3)
            {
                var callback = new TaskCompletionSource<Exception>();
                T argue = default(T);
                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        argue = action(arg1, arg2, arg3);
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });

                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
                return argue;
            }

            public static async Task<T> RunOnGameThreadAsync<T, A, B, C, D>(FuncPattern<T, A, B, C, D> action, A arg1, B arg2, C arg3, D arg4)
            {
                var callback = new TaskCompletionSource<Exception>();
                T argue = default(T);
                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        argue = action(arg1, arg2, arg3, arg4);
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });

                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
                return argue;
            }

            public static async Task<T> RunOnGameThreadAsync<T, A, B, C, D, E>(FuncPattern<T, A, B, C, D, E> action, A arg1, B arg2, C arg3, D arg4, E arg5)
            {
                var callback = new TaskCompletionSource<Exception>();
                T argue = default(T);
                FastTaskDispatcher.QueueOnMainThread(() =>
                {
                    try
                    {
                        argue = action(arg1, arg2, arg3, arg4, arg5);
                        callback.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        callback.SetResult(ex);
                    }
                });
                var err = await callback.Task;
                if (err != null)
                {
                    throw err;
                }
                return argue;
            }

            public static void QueueOnThreadPool(TaskPattern task)
            {
                ThreadPool.QueueUserWorkItem(async (_) => await task());
            }

            public static void QueueOnThreadPool<A>(TaskPattern<A> task, A arg1)
            {
                ThreadPool.QueueUserWorkItem(async (_) => await task(arg1));
            }

            public static void QueueOnThreadPool<A, B>(TaskPattern<A, B> task, A arg1, B arg2)
            {
                ThreadPool.QueueUserWorkItem(async (_) => await task(arg1, arg2));
            }

            public static void QueueOnThreadPool<A, B, C>(TaskPattern<A, B, C> task, A arg1, B arg2, C arg3)
            {
                ThreadPool.QueueUserWorkItem(async (_) => await task(arg1, arg2, arg3));
            }

            public static void QueueOnThreadPool<A, B, C, D>(TaskPattern<A, B, C, D> task, A arg1, B arg2, C arg3, D arg4)
            {
                ThreadPool.QueueUserWorkItem(async (_) => await task(arg1, arg2, arg3, arg4));
            }

            public static void QueueOnThreadPool<A, B, C, D, E>(TaskPattern<A, B, C, D, E> task, A arg1, B arg2, C arg3, D arg4, E arg5)
            {
                ThreadPool.QueueUserWorkItem(async (_) => await task(arg1, arg2, arg3, arg4, arg5));
            }

            public static void QueueOnThreadPool(VoidPattern task)
            {
                ThreadPool.QueueUserWorkItem((_) => task());
            }

            public static void QueueOnThreadPool<A>(VoidPattern<A> task, A arg1)
            {
                ThreadPool.QueueUserWorkItem((_) => task(arg1));
            }

            public static void QueueOnThreadPool<A, B>(VoidPattern<A, B> task, A arg1, B arg2)
            {
                ThreadPool.QueueUserWorkItem((_) => task(arg1, arg2));
            }

            public static void QueueOnThreadPool<A, B, C>(VoidPattern<A, B, C> task, A arg1, B arg2, C arg3)
            {
                ThreadPool.QueueUserWorkItem((_) => task(arg1, arg2, arg3));
            }

            public static void QueueOnThreadPool<A, B, C, D>(VoidPattern<A, B, C, D> task, A arg1, B arg2, C arg3, D arg4)
            {
                ThreadPool.QueueUserWorkItem((_) => task(arg1, arg2, arg3, arg4));
            }

            public static void QueueOnThreadPool<A, B, C, D, E>(VoidPattern<A, B, C, D, E> task, A arg1, B arg2, C arg3, D arg4, E arg5)
            {
                ThreadPool.QueueUserWorkItem((_) => task(arg1, arg2, arg3, arg4, arg5));
            }
        }
    }
}