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
        private HttpHandler HttpHandler => ServiceProvider.GetRequiredService<HttpHandler>();
        private IMethodBinding MethodBinding { get; set; }
        private IHttpTransport Transport { get; set; }

        /// <summary>
        /// Confugures the proxy
        /// </summary>
        /// <param name="config"></param>
        public bool Configure(ISolidRpcOpenApiConfig config)
        {
            if(config.InvocationConfiguration.HasImplementation)
            {
                return false;
            }
            MethodBinding = MethodBinderStore.CreateMethodBindings(
                config.OpenApiSpec,
                config.InvocationConfiguration.MethodInfo,
                config.GetTransports(),
                config.SecurityKey
            ).First();
            Transport = MethodBinding.Transports.OfType<IHttpTransport>().FirstOrDefault();
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
            return HttpHandler.InvokeAsync<TAdvice>(MethodBinding, Transport, invocation.Arguments, InvocationOptions.Http);
        }
    }
}
