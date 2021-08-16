using SolidRpc.Abstractions.OpenApi.Transport;

namespace SolidRpc.OpenApi.AzSvcBus.Invoker
{
    /// <summary>
    /// Configures the svc bus transport
    /// </summary>
    public interface ISvcBusTransport : IQueueTransport
    {
    }
}