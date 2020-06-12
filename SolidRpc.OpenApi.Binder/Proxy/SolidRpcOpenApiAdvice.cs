using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using SolidRpc.Abstractions.OpenApi.Transport;
using System.Linq;
using SolidRpc.OpenApi.Binder.Invoker;
using SolidRpc.Abstractions.OpenApi.Invoker;

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
        private IHandler Handler { get; set; }
        private ITransport Transport { get; set; }

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
                config.GetTransports(),
                config.SecurityKey
            ).First();

            //
            // Get configured transport
            //
            Transport = MethodBinding.Transports.FirstOrDefault(o => o.TransportType == proxyTransportType);
            if (Transport == null)
            {
                throw new Exception($"Cannot find the transport for {proxyTransportType} transport in configured method.");
            }
            //
            // get the transport handler
            //
            Handler = ServiceProvider.GetRequiredService<IEnumerable<IHandler>>().Where(o => o.TransportType == proxyTransportType).FirstOrDefault();
            if(Handler == null)
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
            if(invocation.Caller is ISolidProxy || !HasImplementation)
            {
                return Handler.InvokeAsync<TAdvice>(MethodBinding, Transport, invocation.Arguments, InvocationOptions.Http);
            }
            else if(HasImplementation)
            {
                return next();
            }
            else
            {
                throw new Exception("Cannot handle call.");
            }
        }
    }
}
