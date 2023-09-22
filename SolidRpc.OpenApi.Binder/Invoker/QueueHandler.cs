using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.InternalServices;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Invoker
{
    public abstract class QueueHandler<TTransport> : TransportHandler<TTransport> where TTransport : IQueueTransport
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
            if (queueName.StartsWith("/"))
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
                if (idx > -1)
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
                if (string.Join(pathSeparator, workSet).Length < maxLength)
                {
                    break;
                }
                var workProspect = $"{workSet[i].First()}{workSet[i].Length}{workSet[i].Last()}";
                if (workProspect.Length < workSet[i].Length)
                {
                    workSet[i] = workProspect;
                }
            }
            return string.Join(pathSeparator, workSet);
        }

        public QueueHandler(
            ILogger<QueueHandler<TTransport>> logger, 
            IMethodBinderStore methodBinderStore, 
            ISerializerFactory serializerFactory,
            ISolidRpcApplication solidRpcApplication)
            :base(logger, methodBinderStore)
        {
            SerializerFactory = serializerFactory;
            SolidRpcApplication = solidRpcApplication;
        }

        public ISerializerFactory SerializerFactory { get; }
        public ISolidRpcApplication SolidRpcApplication { get; }

        public virtual void Configure(IMethodBinding methodBinding, TTransport transport)
        {
            if (string.IsNullOrEmpty(transport.ConnectionName))
            {
                transport.ConnectionName = $"AzureWebJobsStorage";
            }
            if (string.IsNullOrEmpty(transport.QueueName))
            {
                transport.QueueName = CreateSafeQueueName(TransportType, methodBinding.LocalPath);
            }
            base.Configure(methodBinding, transport);
        }

        public override async Task<IHttpResponse> InvokeAsync(IServiceProvider serviceProvider, IMethodBinding methodBinding, TTransport transport, IHttpRequest httpReq, CancellationToken cancellationToken)
        {
            var httpReqData = new HttpRequest();
            await httpReq.CopyToAsync(httpReqData);

            string message;
            SerializerFactory.SerializeToString(out message, httpReqData);

            await SolidRpcApplication.WaitForStartupTasks();

            await InvokeAsync(serviceProvider, methodBinding, transport, message, cancellationToken);

            return new SolidHttpResponse()
            {
                StatusCode = 200
            };
        }

        protected abstract Task InvokeAsync(IServiceProvider serviceProvider, IMethodBinding methodBinding, TTransport transport, string message, CancellationToken cancellationToken);
    }
}
