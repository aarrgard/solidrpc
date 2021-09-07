using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SolidRpc.OpenApi.ApplicationInsights
{
    public class InvocationLoggingProvider : Binder.Logging.InvocationLoggingProvider
    {
        private static ConcurrentDictionary<string, IList<AIEvent>> LogScopes = new ConcurrentDictionary<string, IList<AIEvent>>();

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
                switch (logLevel)
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
                        throw new Exception("Cannot handle log level:" + logLevel);
                }
            }
            public Exception Exception { get; }
            public IDictionary<string, string> Properties { get; }
            public DateTimeOffset Timestamp { get; }
            public string Message { get; }
            public SeverityLevel SeverityLevel { get; }
        }


        public InvocationLoggingProvider(TelemetryClient telemetryClient, InvocationLoggingProviderOptions options)
            : base (options.PropertyActivator)
        {
            TelemetryClient = telemetryClient;

            var tpcb = TelemetryClient.TelemetryConfiguration.TelemetryProcessorChainBuilder;
            tpcb.Use(next => new TelemetryProcessor(next));
            tpcb.Build();

            Options = options;
            InvocationCreatedEvent += DoInvocationCreatedEvent;
            InvocationDisposedEvent += DoInvocationDisposedEvent;
            InvocationLogEvent += DoInvocationLogEvent;
            LoggerDisposeEvent += DoLoggerDisposeEvent;


        }

        public TelemetryClient TelemetryClient { get; }
        public InvocationLoggingProviderOptions Options { get; }

        private void DoLoggerDisposeEvent()
        {
            TelemetryClient.Flush();
            Thread.Sleep(1000);
        }

        private void DoInvocationLogEvent(IDictionary<string, string> props, LogLevel logLevel, EventId eventId, Exception exception, string msg)
        {
            if (Options.LogSettings == LogSettings.AllEvents)
            {
                TrackTrace(new AIEvent(logLevel, msg, exception, props));
                return;
            }
            if (Options.PropertyActivator != null)
            {
                
                if (props != null && props.TryGetValue(Options.PropertyActivator, out string invocationId))
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

        private void DoInvocationDisposedEvent(string activatorPropertyValue)
        {
            LogScopes.TryRemove(activatorPropertyValue, out IList<AIEvent> msgs);
            if (msgs.Any(o => o.SeverityLevel >= SeverityLevel.Warning))
            {
                msgs.ToList().ForEach(msg =>
                {
                    TrackTrace(msg);
                });
            }
        }

        private void TrackTrace(AIEvent msg)
        {
            var tt = new TraceTelemetry()
            {
                Message = msg.Message,
                SeverityLevel = msg.SeverityLevel,
                Timestamp = msg.Timestamp,
                Extension = TelemetryExtension.Instance
            };
            msg.Properties?.ToList().ForEach(o => tt.Properties[o.Key] = o.Value);
            TelemetryClient.TrackTrace(tt);

            if (msg.Exception != null)
            {
                var et = new ExceptionTelemetry()
                {
                    Message = msg.Exception.Message,
                    Exception = msg.Exception,
                    SeverityLevel = msg.SeverityLevel,
                    Timestamp = msg.Timestamp,
                    Extension = TelemetryExtension.Instance
                };
                msg.Properties?.ToList().ForEach(o => et.Properties[o.Key] = o.Value);
                TelemetryClient.TrackException(et);
            }
        }

        private static void DoInvocationCreatedEvent(string activatorPropertyValue)
        {
            LogScopes.GetOrAdd(activatorPropertyValue, _ => new List<AIEvent>());
        }
    }
}
