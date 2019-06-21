using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace System
{
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// Logs a trace messsage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="message"></param>
        public static void LogInformation<T>(this IServiceProvider sp, string message)
        {
            var logger = sp.GetService<ILogger<T>>();
            if (logger == null || !logger.IsEnabled(LogLevel.Information))
            {
                return;
            }
            logger.LogInformation(message);
        }

        /// <summary>
        /// Logs a trace messsage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="message"></param>
        public static void LogTrace<T>(this IServiceProvider sp, string message)
        {
            var logger = sp.GetService<ILogger<T>>();
            if (logger == null || !logger.IsEnabled(LogLevel.Trace))
            {
                return;
            }
            logger.LogTrace(message);
        }
    }
}
