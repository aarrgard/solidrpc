using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace SolidRpc.OpenApi.ApplicationInsights
{
    public class TelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        // next will point to the next TelemetryProcessor in the chain.
        public TelemetryProcessor(ITelemetryProcessor next)
        {
            Next = next;
        }

        public void Process(ITelemetry item)
        {
            if (item is TraceTelemetry traceItem)
            {
                if(traceItem.Extension is TelemetryExtension)
                {
                    Next.Process(item);
                }
            }
            else if (item is ExceptionTelemetry excpetionItem)
            {
                Next.Process(item);
            }
            else if (item is RequestTelemetry requestItem)
            {
                Next.Process(item);
            }
            else if (item is DependencyTelemetry dependencyItem)
            {
                if (dependencyItem.Extension is TelemetryExtension)
                {
                    Next.Process(item);
                }
            }

            // do not send this telemetry
        }
    }
}
