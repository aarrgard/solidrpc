using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        /// <param name="queueType"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static string CreateSafeQueueName(string queueType, string queueName)
        {
            if (string.IsNullOrEmpty(queueType)) throw new ArgumentNullException(queueName);
            if(queueName.StartsWith("/"))
            {
                queueName = queueName.Substring(1);
            }
            queueName = CompressArgNames(queueName);

            if (queueType.Equals("azsvcbus", StringComparison.InvariantCultureIgnoreCase))
            {

            }
            else if (queueType.Equals("azqueue", StringComparison.InvariantCultureIgnoreCase))
            {
                queueName = CompressName(queueName.ToLower(), 63, "-");
            }

            return queueName;
        }

        private static string CompressArgNames(string queueName)
        {
            var idx = 0;
            while (idx > -1) 
            {
                idx = queueName.IndexOf('{', idx);
                if(idx > -1)
                {
                    var endIdx = queueName.IndexOf('}', idx);
                    queueName = queueName.Substring(0, idx) + "arg" + queueName.Substring(endIdx + 1);
                }
            }
            return queueName;
        }

        private static string CompressName(string queueName, int maxLength, string pathSeparator)
        {
            var workSet = queueName.Split('/');
            for (int i = 0; i < workSet.Length; i++)
            {
                if(string.Join(pathSeparator, workSet).Length < maxLength)
                {
                    break;
                }
                var workProspect = $"{workSet[i].First()}{workSet[i].Length}{workSet[i].Last()}";
                if(workProspect.Length < workSet[i].Length)
                {
                    workSet[i] = workProspect;
                }
            }
            return string.Join(pathSeparator, workSet);
        }


        /// <summary>
        /// Represents a queue transport
        /// </summary>
        /// <param name="invocationStrategy"></param>
        /// <param name="messagePriority"></param>
        /// <param name="queueName"></param>
        /// <param name="connectionString"></param>
        /// <param name="transportType"></param>
        /// <param name="inboundHandler"></param>
        /// <param name="preInvokeCallback"></param>
        /// <param name="postInvokeCallback"></param>
        public QueueTransport(
            InvocationStrategy invocationStrategy,
            int messagePriority,
            string queueName,
            string connectionString, 
            string transportType,
            string inboundHandler,
            Func<IHttpRequest, Task> preInvokeCallback,
            Func<IHttpResponse, Task> postInvokeCallback)
            : base(transportType, invocationStrategy, messagePriority, preInvokeCallback, postInvokeCallback)
        {
            QueueName = queueName;
            ConnectionName = connectionString;
            InboundHandler = inboundHandler;
        }

        /// <summary>
        ///  configures this transport
        /// </summary>
        /// <param name="methodBinding"></param>
        public override void Configure(IMethodBinding methodBinding)
        {
            if (string.IsNullOrEmpty(ConnectionName))
            {
                ConnectionName = $"SolidRpc{TransportType}Connection";
            }
            if (string.IsNullOrEmpty(QueueName))
            {
                QueueName = CreateSafeQueueName(TransportType, methodBinding.LocalPath);
            }
            base.Configure(methodBinding);
        }

        /// <summary>
        /// The queue name
        /// </summary>
        public string QueueName { get; private set; }

        /// <summary>
        /// The connection name
        /// </summary>
        public string ConnectionName { get; private set; }

        /// <summary>
        /// The inbound handler
        /// </summary>
        public string InboundHandler { get; }

        /// <summary>
        /// The operation address should be the same as in open api spec.
        /// </summary>
        public override Uri OperationAddress => null;

        /// <summary>
        /// Sets the connection name
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public IQueueTransport SetConnectionName(string connectionName)
        {
            return new QueueTransport(InvocationStrategy, MessagePriority, QueueName, connectionName, TransportType, InboundHandler, PreInvokeCallback, PostInvokeCallback);
        }

        /// <summary>
        /// Sets the queue name
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public IQueueTransport SetQueueName(string queueName)
        {
            return new QueueTransport(InvocationStrategy, MessagePriority, queueName, ConnectionName, TransportType, InboundHandler, PreInvokeCallback, PostInvokeCallback);
        }

        /// <summary>
        /// Sets the inbound handler
        /// </summary>
        /// <param name="inboundHandler"></param>
        /// <returns></returns>
        public IQueueTransport SetInboundHandler(string inboundHandler)
        {
            return new QueueTransport(InvocationStrategy, MessagePriority, QueueName, ConnectionName, TransportType, inboundHandler, PreInvokeCallback, PostInvokeCallback);
        }

        /// <summary>
        /// Sets the invocation strategy
        /// </summary>
        /// <param name="invocationStrategy"></param>
        /// <returns></returns>
        public QueueTransport SetInvocationStrategy(InvocationStrategy invocationStrategy)
        {
            return new QueueTransport(invocationStrategy, MessagePriority, QueueName, ConnectionName, TransportType, InboundHandler, PreInvokeCallback, PostInvokeCallback);
        }

    }
}
