using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.ApplicationInsights;
using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the service provider.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the application insigts logger.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="message"></param>
        public static void AddSolidRpcApplicationInsights(this IServiceCollection services, LogSettings logSettings, string propertyActivator = null)
        {
            var conf = services.GetSolidRpcService<IConfiguration>();
            var instrumentationKey = conf["APPINSIGHTS_INSTRUMENTATIONKEY"];
            if(string.IsNullOrEmpty(instrumentationKey))
            {
                throw new Exception("No APPINSIGHTS_INSTRUMENTATIONKEY key specified.");
            }

            services.AddApplicationInsightsTelemetry(instrumentationKey);
            services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

            // not working ! https://github.com/Azure/azure-webjobs-sdk/issues/2584
            //services.AddApplicationInsightsTelemetryProcessor<TelemetryProcessor>();


            services.AddLogging(o => {
                o.SetMinimumLevel(LogLevel.Trace);
                o.Services.AddSingleton(new InvocationLoggingProviderOptions(logSettings, propertyActivator));
                o.Services.AddSingleton<ILoggerProvider, InvocationLoggingProvider>();
            });

            services.GetSolidRpcService<ISolidRpcApplication>().AddShutdownCallback(() => {
                return Task.CompletedTask;
            });
        }
    }
}
