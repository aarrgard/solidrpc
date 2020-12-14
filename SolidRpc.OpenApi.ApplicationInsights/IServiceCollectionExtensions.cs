using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.ApplicationInsights;
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
        private static LogSettings LogSettings;
        private static string PropertyActivator;
        private static ConcurrentDictionary<string, IList<AIEvent>> LogScopes = new ConcurrentDictionary<string, IList<AIEvent>>();
        private static TelemetryClient s_telemetryClient;

        private class AIEvent
        {
            public AIEvent(LogLevel logLevel, string message, Exception exception, IDictionary<string, string> properties)
            {
                SeverityLevel = MapLogLevel(logLevel);
                Message = message;
                Exception = exception;
                Properties = properties;
                Timestamp = DateTimeOffset.Now;
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
            public Exception Exception { get; }
            public IDictionary<string, string> Properties { get; }
            public DateTimeOffset Timestamp { get; }
            public string Message { get; }
            public SeverityLevel SeverityLevel { get; }
        }

        /// <summary>
        /// Adds the application insigts logger.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="message"></param>
        public static void AddSolidRpcApplicationInsights(this IServiceCollection services, LogSettings logSettings, string propertyActivator = null)
        {
            var conf = services.GetSolidRpcService<IConfiguration>();
            var instrumentationKey = conf["InstrumentationKey"];
            if(string.IsNullOrEmpty(instrumentationKey))
            {
                throw new Exception("No instrumentation key specified.");
            }
            var telConf = TelemetryConfiguration.CreateDefault();
            telConf.TelemetryInitializers.Add(new TelemetryInitializer(services.GetSolidRpcService<IConfiguration>()));
            s_telemetryClient = new TelemetryClient(telConf);
            s_telemetryClient.InstrumentationKey = instrumentationKey;

            LogSettings = logSettings;
            PropertyActivator = propertyActivator;
            var logProvider = new InvocationLoggingProvider(PropertyActivator);
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

        private static void InvocationLogEvent(IDictionary<string, string> props, LogLevel logLevel, EventId eventId, Exception exception, string msg)
        {
            if (LogSettings == LogSettings.AllEvents)
            {
                TrackTrace(new AIEvent(logLevel, msg, exception, props));
                return;
            }
            if (PropertyActivator != null)
            {
                if (props != null && props.TryGetValue(PropertyActivator, out string invocationId))
                {
                    if (LogScopes.TryGetValue(invocationId, out IList<AIEvent> msgs))
                    {
                        lock (msgs)
                        {
                            msgs.Add(new AIEvent(logLevel, msg, exception, props));
                        }
                    }
                }
            }
        }

        private static void InvocationDisposedEvent(string activatorPropertyValue)
        {
            LogScopes.TryRemove(activatorPropertyValue, out IList<AIEvent> msgs);
            if(msgs.Any(o => o.SeverityLevel >= SeverityLevel.Warning))
            {
                msgs.ToList().ForEach(msg =>
                {
                    TrackTrace(msg);
                });
            }
        }

        private static void TrackTrace(AIEvent msg)
        {
            var tt = new TraceTelemetry()
            {
                Message = msg.Message,
                SeverityLevel = msg.SeverityLevel,
                Timestamp = msg.Timestamp
            };
            msg.Properties?.ToList().ForEach(o => tt.Properties[o.Key] = o.Value);
            s_telemetryClient.TrackTrace(tt);

            if(msg.Exception != null)
            {
                var et = new ExceptionTelemetry()
                {
                    Message = msg.Message,
                    Exception = msg.Exception,
                    SeverityLevel = msg.SeverityLevel,
                    Timestamp = msg.Timestamp
                };
                msg.Properties?.ToList().ForEach(o => et.Properties[o.Key] = o.Value);
                s_telemetryClient.TrackException(et);
            }
        }

        private static void InvocationCreatedEvent(string activatorPropertyValue)
        {
            LogScopes.GetOrAdd(activatorPropertyValue, _ => new List<AIEvent>());
        }
    }
}
