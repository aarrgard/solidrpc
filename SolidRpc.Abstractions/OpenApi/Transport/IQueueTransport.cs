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
        string QueueName { get; }

        /// <summary>
        /// Sets the queue name
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        IQueueTransport SetQueueName(string queueName);

        /// <summary>
        /// Returns the connection name to use(key in configuration)
        /// </summary>
        string ConnectionName { get; }

        /// <summary>
        /// Sets the connection name
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        IQueueTransport SetConnectionName(string connectionName);

        /// <summary>
        /// Specifies the inbound message handler. If this string is empty
        /// then no message handler wiil be configured for the client. 
        /// "azqueue" - means that the azure functions should pickup messages
        /// "azsvcbus" - means that the azure functions should pickup messages
        /// "generic" - means that we should configure a generic handler. 
        /// </summary>
        string InboundHandler { get; }

        /// <summary>
        /// Sets the inbound handler.
        /// </summary>
        /// <param name="inboundHandler"></param>
        /// <returns></returns>
        IQueueTransport SetInboundHandler(string inboundHandler);
    }
}