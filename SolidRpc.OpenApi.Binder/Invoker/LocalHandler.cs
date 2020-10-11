using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(IHandler), typeof(LocalHandler), SolidRpcServiceLifetime.Singleton, SolidRpcServiceInstances.Many)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Invokes methods on the local implementation.
    /// </summary>
    public class LocalHandler : Handler
    {
        /// <summary>
        /// Returns the "Local" transport type
        /// </summary>
        public static new string TransportType => GetTransportType(typeof(LocalHandler));

        public LocalHandler(ILogger<LocalHandler> logger, IServiceProvider serviceProvider)
            :base(logger, serviceProvider)
        {
        }
        public override async Task<TRes> InvokeAsync<TRes>(MethodInfo mi, object[] args, InvocationOptions invocationOptions)
        {
            var service = ServiceProvider.GetService(mi.DeclaringType);
            if (service == null)
            {
                throw new Exception("Cannot find service:" + mi.DeclaringType.FullName);
            }
            var proxy = service as ISolidProxy;
            if (proxy == null)
            {
                // the service is not proxied - invoke directly
                return (TRes)mi.Invoke(service, args);
            }

            var methodBinding = MethodBinderStore.GetMethodBinding(mi);
            if (methodBinding == null)
            {
                // service does not have a openapi specification - invoke directly
                return (TRes)mi.Invoke(service, args);
            }

            return (TRes)proxy.Invoke(this, mi, args);
        }
        public override Task<IHttpResponse> InvokeAsync<TResp>(IMethodBinding methodBinding, ITransport transport, IHttpRequest httpReq, InvocationOptions invocationOptions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
