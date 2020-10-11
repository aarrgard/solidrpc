using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using SolidRpc.Abstractions.OpenApi.Transport;
using System.Linq;
using SolidRpc.OpenApi.Binder.Invoker;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder.Http;
using Microsoft.Extensions.Primitives;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SolidRpcOpenApiAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="openApiParser"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="serviceProvider"></param>
        public SolidRpcOpenApiAdvice(
            ILogger<SolidRpcOpenApiAdvice<TObject, TMethod, TAdvice>> logger,
            IServiceProvider serviceProvider)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        private ILogger Logger { get; }
        private IServiceProvider ServiceProvider { get; }
        private IMethodBinderStore MethodBinderStore => ServiceProvider.GetRequiredService<IMethodBinderStore>();
        private bool HasImplementation { get; set; }
        private IMethodBinding MethodBinding { get; set; }
        private IHandler ProxyHandler { get; set; }
        private ITransport ProxyTransport { get; set; }
        private IHandler InvokerHandler { get; set; }
        private ITransport InvokerTransport { get; set; }

        /// <summary>
        /// Confiugures the proxy
        /// </summary>
        /// <param name="config"></param>
        public bool Configure(ISolidRpcOpenApiConfig config)
        {
            HasImplementation = config.InvocationConfiguration.HasImplementation;

            //
            // Determine transport type. If not explititly set use Http
            // if not implementation exists.
            //
            var proxyTransportType = config.ProxyTransportType;
            if (string.IsNullOrEmpty(config.ProxyTransportType))
            {
                if (HasImplementation)
                {
                    proxyTransportType = LocalHandler.TransportType;
                }
                else
                {
                    proxyTransportType = HttpHandler.TransportType;
                }
            }
            if (proxyTransportType == LocalHandler.TransportType)
            {
                return false;
            }

            //
            // get binding
            //
            MethodBinding = MethodBinderStore.CreateMethodBindings(
                config.OpenApiSpec,
                config.InvocationConfiguration.MethodInfo,
                config.GetTransports()
            ).First();

            //
            // Get proxy transport + handler
            //
            ProxyTransport = MethodBinding.Transports.FirstOrDefault(o => o.TransportType == proxyTransportType);
            if (ProxyTransport == null)
            {
                throw new Exception($"Cannot find the transport for {proxyTransportType} transport in configured method.");
            }
            ProxyHandler = ServiceProvider.GetRequiredService<IEnumerable<IHandler>>().Where(o => o.TransportType == ProxyTransport.TransportType).FirstOrDefault();
            if(ProxyHandler == null)
            {
                throw new Exception($"Cannot find the handler for {proxyTransportType} transport.");
            }

            //
            // Get invoker transport + handler
            //
            InvokerTransport = MethodBinding.Transports.FirstOrDefault(o => o.InvocationStrategy == InvocationStrategy.Invoke);
            if (InvokerTransport == null)
            {
                throw new Exception($"No invocation transport configured");
            }
            InvokerHandler = ServiceProvider.GetRequiredService<IEnumerable<IHandler>>().Where(o => o.TransportType == InvokerTransport.TransportType).FirstOrDefault();
            if (InvokerHandler == null)
            {
                throw new Exception($"Cannot find the handler for {proxyTransportType} transport.");
            }

            return true;
        }

        /// <summary>
        /// Handles  the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            IHandler handler;
            if (invocation.Caller is ISolidProxy)
            {
                handler = ProxyHandler;
            }
            else if (HasImplementation)
            {
                return next();
            }
            else
            {
                handler = invocation.GetValue<IHandler>(typeof(IHandler).FullName) ?? InvokerHandler;
            }
            return InvokeAsync(handler, MethodBinding, ProxyTransport, invocation);
        }

        private Task<T> InvokeAsync<T>(IHandler proxyHandler, IMethodBinding methodBinding, ITransport transport, ISolidProxyInvocation<TObject, TMethod, T> invocation)
        {
            var invocationOptions = invocation.GetValue<InvocationOptions>(typeof(InvocationOptions).FullName);
            if(invocationOptions == null)
            {
                invocationOptions = new InvocationOptions(proxyHandler.TransportType, InvocationOptions.MessagePriorityNormal);
            }
            var httpHeaders = invocation.Keys.Where(o => o.StartsWith("http_", StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (httpHeaders.Any())
            {
                invocationOptions = invocationOptions.AddPreInvokeCallback(req =>
                {
                    var data = httpHeaders
                        .SelectMany(o => invocation.GetValue<StringValues>(o).Select(o2 => new { Key = o.Substring(5), Value = o2 }))
                        .Select(o => new SolidHttpRequestDataString("text/plain", o.Key, o.Value)).ToList();
                    req.Headers = req.Headers.Union(data).ToList();
                    return Task.CompletedTask;
                });
            }

            return proxyHandler.InvokeAsync<T>(methodBinding, transport, invocation.Arguments, invocationOptions);
        }
    }
}
