﻿using SolidRpc.Abstractions.OpenApi.Binder;
using System;
using System.Linq;

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
        /// <param name="queueName"></param>
        /// <param name="connectionString"></param>
        /// <param name="queueType"></param>
        /// <param name="inboundHandler"></param>
        public QueueTransport(string queueName, string connectionString, string queueType, string inboundHandler)
        {
            QueueName = queueName;
            ConnectionName = connectionString;
            QueueType = queueType ?? throw new ArgumentNullException(nameof(queueType));
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
                ConnectionName = $"SolidRpc{QueueType}Connection";
            }
            if (string.IsNullOrEmpty(QueueName))
            {
                QueueName = CreateSafeQueueName(QueueType, methodBinding.AbsolutePath);
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
            return new QueueTransport(queueName, ConnectionName, QueueType, InboundHandler);
        }

        /// <summary>
        /// The connection name
        /// </summary>
        public string ConnectionName { get; private set; }

        /// <summary>
        /// Sets the connection name
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public IQueueTransport SetConnectionName(string connectionName)
        {
            return new QueueTransport(QueueName, connectionName, QueueType, InboundHandler);
        }

        /// <summary>
        /// The queue type
        /// </summary>
        public string QueueType { get; }

        /// <summary>
        /// Sets the queue type
        /// </summary>
        /// <param name="queueType"></param>
        /// <returns></returns>
        public IQueueTransport SetQueueType(string queueType)
        {
            return new QueueTransport(QueueName, ConnectionName, queueType, InboundHandler);
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
            return new QueueTransport(QueueName, ConnectionName, QueueType, inboundHandler);
        }

    }
}
