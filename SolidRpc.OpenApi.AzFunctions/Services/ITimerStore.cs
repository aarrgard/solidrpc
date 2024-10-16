﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions.Services
{
    /// <summary>
    /// The timer store is used to invoke timers
    /// </summary>
    public interface ITimerStore
    {
        /// <summary>
        /// Adds the timer action
        /// </summary>
        /// <param name="timerId"></param>
        /// <param name="action"></param>
        void AddTimerAction(string timerId, Func<IServiceProvider, CancellationToken, Task> action);

        /// <summary>
        /// Returns the action associated with the supplied timer id
        /// </summary>
        /// <param name="timerId"></param>
        /// <returns></returns>
        Func<IServiceProvider, CancellationToken, Task> GetTimerAction(string timerId);
    }
}
