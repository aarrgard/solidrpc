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

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SolidRpcOpenApiInvocAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        private const string s_HeaderPrefix = "http_req_";
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
            IEnumerable<IHandler> handlers)
        {
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
            Handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
        }

        private IMethodBinderStore MethodBinderStore { get; }
        private IEnumerable<IHandler> Handlers { get; }

        /// <summary>
        /// Confiugures the proxy
        /// </summary>
        /// <param name="config"></param>
        public bool Configure(ISolidRpcOpenApiConfig config)
        {
            //
            // get binding
            //
            MethodBinding = MethodBinderStore.CreateMethodBindings(
                config.OpenApiSpec,
                config.InvocationConfiguration.MethodInfo,
                config.GetTransports()
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
            var invocationOptions = invocation.GetValue<InvocationOptions>(typeof(InvocationOptions).FullName) ?? throw new Exception("No invocation options assigned to the invocation");
            if (invocationOptions.TransportType == LocalHandler.TransportType)
            {
                return next();
            }
            else
            {
                var handler = Handlers.Single(o => o.TransportType == invocationOptions.TransportType);
                var transport = MethodBinding.Transports.Single(o => o.TransportType == invocationOptions.TransportType);

                //
                // add http headers
                //
                var httpHeaders = invocation.Keys.Where(o => o.StartsWith(s_HeaderPrefix, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (httpHeaders.Any())
                {
                    invocationOptions = invocationOptions.AddPreInvokeCallback(req =>
                    {
                        var data = httpHeaders
                            .SelectMany(o => invocation.GetValue<StringValues>(o).Select(o2 => new { Key = o.Substring(s_HeaderPrefix.Length), Value = o2 }))
                            .Select(o => new SolidHttpRequestDataString("text/plain", o.Key, o.Value)).ToList();
                        req.Headers = req.Headers.Union(data).ToList();
                        return Task.CompletedTask;
                    });
                }

                return handler.InvokeAsync<TAdvice>(MethodBinding, transport, invocation.Arguments, invocationOptions);
            }

        }
    }
}
