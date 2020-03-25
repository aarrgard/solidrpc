using SolidRpc.Abstractions.OpenApi.Binder;
using System;

namespace SolidRpc.Abstractions.OpenApi.Transport.Impl
{
    /// <summary>
    /// Contains the settings for the queue transport.
    /// </summary>
    public class QueueTransport : Transport, IQueueTransport
    {
        /// <summary>
        /// Creates a safe queue name
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static string CreateSafeQueueName(string queueName)
        {
            if(queueName.StartsWith("/"))
            {
                queueName = queueName.Substring(1);
            }
            return queueName.Replace('+', '/');
        }

        /// <summary>
        /// Represents a queue transport
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="connectionString"></param>
        /// <param name="inboundHandler"></param>
        public QueueTransport(string queueName, string connectionString, string inboundHandler)
        {
            QueueName = queueName;
            ConnectionString = connectionString;
            InboundHandler = inboundHandler;
        }

        /// <summary>
        ///  configures this transport
        /// </summary>
        /// <param name="methodBinding"></param>
        public override void Configure(IMethodBinding methodBinding)
        {
            if(string.IsNullOrEmpty(QueueName))
            {
                QueueName = CreateSafeQueueName(methodBinding.Address.AbsolutePath);
            }
            base.Configure(methodBinding);
        }


        /// <summary>
        /// The queue name
        /// </summary>
        public string QueueName { get; private set; }

        /// <summary>
        /// Sets the queue name
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public IQueueTransport SetQueueName(string queueName)
        {
            return new QueueTransport(queueName, ConnectionString, InboundHandler);
        }

        /// <summary>
        /// The connection string
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Sets the connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IQueueTransport SetConnectionString(string connectionString)
        {
            return new QueueTransport(QueueName, connectionString, InboundHandler);
        }

        /// <summary>
        /// The inbound handler
        /// </summary>
        public string InboundHandler { get; }

        /// <summary>
        /// Sets the inbound handler
        /// </summary>
        /// <param name="inboundHandler"></param>
        /// <returns></returns>
        public IQueueTransport SetInboundHandler(string inboundHandler)
        {
            return new QueueTransport(QueueName, ConnectionString, inboundHandler);
        }
    }
}
