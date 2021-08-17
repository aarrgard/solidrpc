﻿using SolidRpc.Abstractions;
using SolidRpc.OpenApi.AzFunctions.Services;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcServiceAttribute(typeof(ITimerStore), typeof(TimerStore), SolidRpcServiceLifetime.Singleton)]
namespace SolidRpc.OpenApi.AzFunctions.Services
{
    /// <summary>
    /// Implements the timer store logic
    /// </summary>
    public class TimerStore : ITimerStore
    {
        private ConcurrentDictionary<string, Func<IServiceProvider, CancellationToken, Task>> actions = new ConcurrentDictionary<string, Func<IServiceProvider, CancellationToken, Task>>();

        /// <summary>
        /// Adds a new timer to the store
        /// </summary>
        /// <param name="timerId"></param>
        /// <param name="action"></param>
        public void AddTimerAction(string timerId, Func<IServiceProvider, CancellationToken, Task> action)
        {
            actions[timerId] = action;
        }

        public Func<IServiceProvider, CancellationToken, Task> GetTimerAction(string timerId)
        {
            return actions[timerId];
        }
    }
}