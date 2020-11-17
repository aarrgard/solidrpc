using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.Binder.Logger;
using SolidRpc.OpenApi.Binder.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Extension methods for the service provider.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        private static ConcurrentDictionary<string, IList<TraceEvent>> LogScopes = new ConcurrentDictionary<string, IList<TraceEvent>>();
        private static TelemetryClient s_telemetryClient;

        private class TraceEvent
        {
            public TraceEvent(string message, LogLevel logLevel, IDictionary<string, string> properties)
            {
                Message = message;
                SeverityLevel = MapLogLevel(logLevel);
                Properties = properties;
            }

            private SeverityLevel MapLogLevel(LogLevel logLevel)
            {
                switch(logLevel)
                {
                    case LogLevel.None:
                    case LogLevel.Information:
                        return SeverityLevel.Information;
                    case LogLevel.Critical:
                        return SeverityLevel.Critical;
                    case LogLevel.Warning:
                        return SeverityLevel.Warning;
                    case LogLevel.Error:
                        return SeverityLevel.Error;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        return SeverityLevel.Verbose;
                    default:
                        throw new Exception("Cannot handle log level:"+logLevel);
                }
            }

            public string Name { get; }

            public IDictionary<string, string> Properties { get; }
            public string Message { get; internal set; }
            public SeverityLevel SeverityLevel { get; internal set; }
        }

        /// <summary>
        /// Adds the application insigts logger.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="message"></param>
        public static void AddSolidRpcApplicationInsights(this IServiceCollection services)
        {
            var conf = services.GetSolidRpcService<IConfiguration>();
            var instrumentationKey = conf["InstrumentationKey"];
            if(string.IsNullOrEmpty(instrumentationKey))
            {
                throw new Exception("No instrumentation key specified.");
            }
            var telConf = TelemetryConfiguration.CreateDefault();
            s_telemetryClient = new TelemetryClient(telConf);
            s_telemetryClient.InstrumentationKey = instrumentationKey;


            var logProvider = new InvocationLoggingProvider();
            services.AddLogging(o => {
                o.AddProvider(logProvider);
                logProvider.InvocationCreatedEvent += InvocationCreatedEvent;
                logProvider.InvocationDisposedEvent += InvocationDisposedEvent;
                logProvider.InvocationLogEvent += InvocationLogEvent;
                logProvider.LoggerDisposeEvent += LoggerDisposing;
            });
            services.GetSolidRpcService<ISolidRpcApplication>().AddShutdownCallback(() => {
                logProvider.Dispose();
                return Task.CompletedTask;
            });
        }

        private static void LoggerDisposing()
        {
            s_telemetryClient.Flush();
            Thread.Sleep(1000);
        }

        private static void InvocationLogEvent(IEnumerable<KeyValuePair<string, object>> state, LogLevel logLevel, EventId arg3, Exception arg4, string msg)
        {
            var props = state?.Select(o => new { o.Key, Value = o.Value?.ToString() }).ToDictionary(o => o.Key, o => o.Value);
            var traceEvt = new TraceEvent(msg, logLevel, props);
            if(props != null && props.TryGetValue("InvocationId", out string invocationId))
            {
                if (LogScopes.TryGetValue(invocationId, out IList<TraceEvent> msgs))
                {
                    lock (msgs)
                    {
                        msgs.Add(traceEvt);
                    }
                }
            }
            else
            {
                s_telemetryClient.TrackTrace(traceEvt.Message, traceEvt.SeverityLevel, traceEvt.Properties);
            }
        }

        private static void InvocationDisposedEvent(InvocationState state)
        {
            LogScopes.TryRemove(state.InvocationId, out IList<TraceEvent> msgs);
            msgs.ToList().ForEach(o =>
            {
                s_telemetryClient.TrackTrace(o.Message, o.SeverityLevel, o.Properties);
            });
        }

        private static void InvocationCreatedEvent(InvocationState state)
        {
            LogScopes.GetOrAdd(state.InvocationId, _ => new List<TraceEvent>());
        }
    }
}
