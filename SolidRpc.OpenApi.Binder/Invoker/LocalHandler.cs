using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ITransportHandler), typeof(LocalHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Invokes methods on the local implementation.
    /// </summary>
    public class LocalHandler : TransportHandler<ILocalTransport>
    {
        public LocalHandler(ILogger<LocalHandler> logger, IServiceProvider serviceProvider)
            :base(logger, serviceProvider)
        {
        }
        public override Task<object> InvokeAsync(IMethodBinding mb, object target, MethodInfo mi, object[] args, InvocationOptions invocationOptions)
        {
            if (target == null)
            {
                throw new Exception("Cannot find service:" + mi.DeclaringType.FullName);
            }
            var proxy = target as ISolidProxy;
            if (proxy == null)
            {
                // the service is not proxied - invoke directly
                return HandleResponse(mi.Invoke(target, args));
            }

            var methodBinding = MethodBinderStore.GetMethodBinding(mi);
            if (methodBinding == null)
            {
                // service does not have a openapi specification - invoke directly
                return HandleResponse(mi.Invoke(target, args));
            }

            return HandleResponse(proxy.Invoke(this, mi, args));
        }

        private async Task<object> HandleResponse(object res)
        {
            if(res is Task)
            {
                await ((Task)res);
                var awaitedRes = res.GetType().GetProperty("Result")?.GetValue(res);
                return awaitedRes;
            }
            return res;
        }

        public override Task<IHttpResponse> InvokeAsync(IMethodBinding methodBinding, ILocalTransport transport, IHttpRequest httpReq, InvocationOptions invocationOptions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override void Configure(IMethodBinding methodBinding, ILocalTransport transport)
        {
        }
    }
}
