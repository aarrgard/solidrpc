using Microsoft.ApplicationInsights.Extensibility;

namespace SolidRpc.OpenApi.ApplicationInsights
{
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
