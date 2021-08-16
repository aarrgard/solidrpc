using System.Linq;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// Represents the settings for the queue transport
    /// </summary>
    public interface IQueueTransport : ITransport
    {
        /// <summary>
        /// Returns the qeueue name
        /// </summary>
        string QueueName { get; set; }

        /// <summary>
        /// Returns the connection name to use(key in configuration)
        /// </summary>
        string ConnectionName { get; set; }

        /// <summary>
        /// Specifies the inbound message handler. If this string is empty
        /// then no message handler wiil be configured for the client. 
        /// "azfunction" - means that the azure functions should pickup messages
        /// "generic" - means that we register a generic message handler for service bus.
        /// </summary>
        string InboundHandler { get; set; }
    }
}