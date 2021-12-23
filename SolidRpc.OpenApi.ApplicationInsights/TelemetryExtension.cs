using Microsoft.ApplicationInsights.Extensibility;

namespace SolidRpc.OpenApi.ApplicationInsights
{
    /// <summary>
    /// We use this extension to mark the telemetry instances
    /// that should be sent to insights
    /// </summary>
    public class TelemetryExtension : IExtension
    {
        public static IExtension Instance = new TelemetryExtension();

        public IExtension DeepClone()
        {
            return this;
        }

        public void Serialize(ISerializationWriter serializationWriter)
        {
        }
    }
}
