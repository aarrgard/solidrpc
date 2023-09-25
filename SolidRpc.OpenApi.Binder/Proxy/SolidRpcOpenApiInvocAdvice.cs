using System;
using System.Threading.Tasks;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System.Linq;
using SolidRpc.OpenApi.Binder.Invoker;
using SolidRpc.Abstractions.OpenApi.Invoker;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.Abstractions.OpenApi.Transport;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SolidRpcOpenApiInvocAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public static readonly IEnumerable<Type> AfterAdvices = new Type[] { typeof(SolidRpcOpenApiInitAdvice<,,>) };

        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="openApiParser"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="serviceProvider"></param>
        public SolidRpcOpenApiInvocAdvice(
            IMethodBinderStore methodBinderStore,
            IEnumerable<ITransportHandler> handlers)
        {
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
            Handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
        }

        private IMethodBinderStore MethodBinderStore { get; }
        private IEnumerable<ITransportHandler> Handlers { get; }

        /// <summary>
        /// Confiugures the proxy
        /// </summary>
        /// <param name="config"></param>
        public bool Configure(ISolidRpcOpenApiConfig config)
        {
            //
            // get binding
            //
            var transports = config.GetTransports().ToList();
            MethodBinding = MethodBinderStore.CreateMethodBindings(
                config.OpenApiSpec,
                config.InvocationConfiguration.MethodInfo,
                transports
            ).FirstOrDefault();
            if (MethodBinding == null)
            {
                return false;
            }

            return true;
        }
        private IMethodBinding MethodBinding { get; set; }

        /// <summary>
        /// Handles  the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            var invocationOptions = InvocationOptions.GetOptions(invocation.SolidProxyInvocationConfiguration.MethodInfo);
            var transportType = invocationOptions.TransportType;
            if (transportType == LocalHandler.TransportType)
            {
                return next();
            }
            else
            {
                var handler = Handlers.Single(o => o.TransportType == transportType);
                var additionalValues = new Dictionary<string, object>();

                //
                // add http headers
                //
                if (!string.IsNullOrEmpty(invocationOptions.ContinuationToken))
                {
                    invocationOptions = invocationOptions.SetKeyValue(MethodInvoker.RequestHeaderContinuationTokenInInvocation, (StringValues)invocationOptions.ContinuationToken);
                }
                if (invocationOptions.Priority != InvocationOptions.MessagePriorityNormal)
                {
                    invocationOptions = invocationOptions.SetKeyValue(MethodInvoker.RequestHeaderPriorityInInvocation, (StringValues)invocationOptions.Priority.ToString());
                }
                var httpHeaders = invocationOptions.Keys
                    .Where(o => o.StartsWith(MethodInvoker.RequestHeaderPrefixInInvocation, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
                if(httpHeaders.Any())
                {
                    invocationOptions = invocationOptions.AddPreInvokeCallback(req =>
                    {
                        var data = httpHeaders
                            .SelectMany(o => {
                                invocationOptions.TryGetValue(o, out StringValues headerValues);
                                return headerValues.Select(o2 => new { Key = o.Substring(MethodInvoker.RequestHeaderPrefixInInvocation.Length), Value = o2 });
                            }).Select(o => new SolidHttpRequestDataString("text/plain", o.Key, o.Value));
                        req.Headers = req.Headers.Union(data).ToList();
                        return Task.CompletedTask;
                    });

                }

                var transport = MethodBinding.Transports.Single(o => o.GetTransportType() == invocationOptions.TransportType);
                using (invocationOptions.Attach())
                {
                    return handler.InvokeAsync<TAdvice>(invocation.ServiceProvider, MethodBinding, transport, invocation.Arguments);
                }
            }

        }
    }
}
