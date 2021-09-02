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

    /// <summary>
    /// Extension methods for the transport
    /// </summary>
    public static class IQueueTransportExtensions
    {

        /// <summary>
        /// Sets the queue transport options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static T SetConnectionName<T>(this T t, string connectionName) where T : IQueueTransport
        {
            t.ConnectionName = connectionName ?? t.ConnectionName;
            return t;
        }

        /// <summary>
        /// Sets the queue transport options
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static T SetQueueName<T>(this T t, string queueName) where T : IQueueTransport
        {
            t.QueueName = queueName ?? t.QueueName;
            return t;
        }

        /// <summary>
        /// Sets the inbound handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="inboundHandler"></param>
        /// <returns></returns>

        public static T SetInboundHandler<T>(this T t, string inboundHandler) where T : IQueueTransport
        {
            t.InboundHandler = inboundHandler;
            return t;
        }
    }
}