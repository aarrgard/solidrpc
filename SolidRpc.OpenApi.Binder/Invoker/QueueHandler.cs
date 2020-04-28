using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.Binder.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Invoker
{
    public abstract class QueueHandler : Handler
    {
        public QueueHandler(
            ILogger<QueueHandler> logger, 
            IServiceProvider serviceProvider, 
            ISerializerFactory serializerFactory,
            ISolidRpcApplication solidRpcApplication)
            :base(logger, serviceProvider)
        {
            SerializerFactory = serializerFactory;
            SolidRpcApplication = solidRpcApplication;
        }

        public ISerializerFactory SerializerFactory { get; }
        public ISolidRpcApplication SolidRpcApplication { get; }

        public override async Task<IHttpResponse> InvokeAsync<TResp>(IMethodBinding methodBinding, ITransport transport, IHttpRequest httpReq, InvocationOptions invocationOptions, CancellationToken cancellationToken)
        {
            var httpReqData = new HttpRequest();
            await httpReq.CopyToAsync(httpReqData);

            string message;
            SerializerFactory.SerializeToString(out message, httpReqData);

            await SolidRpcApplication.WaitForStartupTasks();

            await InvokeAsync(methodBinding, (IQueueTransport)transport, message, invocationOptions, cancellationToken);

            return new SolidHttpResponse()
            {
                StatusCode = 200
            };
        }

        protected abstract Task InvokeAsync(IMethodBinding methodBinding, IQueueTransport transport, string message, InvocationOptions invocationOptions, CancellationToken cancellationToken);
    }
}
