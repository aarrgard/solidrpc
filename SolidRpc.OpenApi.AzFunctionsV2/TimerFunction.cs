﻿using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.OpenApi.AzFunctions.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AzFunctions
{
    /// <summary>
    /// Handles timer triggers
    /// </summary>
    public static class TimerFunction
    {
        /// <summary>
        /// Runs the trigger logic.
        /// </summary>
        /// <param name="myTimer"></param>
        /// <param name="log"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="timerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task Run(TimerInfo myTimer, ILogger log, IServiceProvider serviceProvider, string timerId, CancellationToken cancellationToken)
        {
            return AzFunction.DoRun(async () =>
            {
                await serviceProvider.GetRequiredService<ISolidRpcApplication>().WaitForStartupTasks();
                var ts = serviceProvider.GetRequiredService<ITimerStore>();
                var action = ts.GetTimerAction(timerId);
                await action.Invoke(serviceProvider, cancellationToken);
            });
        }
    }
}
