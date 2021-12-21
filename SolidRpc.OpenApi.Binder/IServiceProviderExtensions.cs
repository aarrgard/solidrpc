using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace System
{
    /// <summary>
    /// Extension methods for the service provider.
    /// </summary>
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

        /// <summary>
        /// Logs an error messsage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="e"></param>
        /// <param name="message"></param>
        public static void LogError<T>(this IServiceProvider sp, Exception e, string message)
        {
            var logger = sp.GetService<ILogger<T>>();
            if (logger == null || !logger.IsEnabled(LogLevel.Error))
            {
                return;
            }
            logger.LogError(e, message);
        }

        /// <summary>
        /// Configures the supplied options from the configuration(if it exists)
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceProvider ConfigureOptions<T>(this IServiceProvider sp, T options)
        {
            if (options == null) return sp;
            var config = sp.GetService<IConfiguration>();
            var name = options.GetType().Name;
            foreach(var prop in options.GetType().GetProperties())
            {
                if (!prop.CanWrite)
                {
                    continue;
                }
                if (!prop.CanRead)
                {
                    continue;
                }
                var val = config[$"{name}:{prop.Name}"];
                if(!string.IsNullOrWhiteSpace(val))
                {
                    object propVal;
                    if(prop.PropertyType == typeof(Guid))
                    {
                        propVal = Guid.Parse(val);
                    }
                    else
                    {
                        propVal = Convert.ChangeType(val, prop.PropertyType);
                    }
                    prop.SetValue(options, propVal);
                }
            }
            return sp;
        }
    }
}
