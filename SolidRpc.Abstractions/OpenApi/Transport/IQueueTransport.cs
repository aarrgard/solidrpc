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
        /// Returns the connection string to use
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Sets the connectin string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        IQueueTransport SetConnectionString(string connectionString);

        /// <summary>
        /// Specifies the inbound message handler. If this string is empty
        /// then no message handler wiil be configured for the client. 
        /// "azfunctions" - means that the azure functions should pickup messages
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