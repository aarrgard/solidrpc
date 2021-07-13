using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SolidRpcOpenApiInitAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public static readonly IEnumerable<Type> BeforeAdvices = new Type[] { typeof(SolidRpcOpenApiInvocAdvice<,,>) };

        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="methodBinderStore"></param>
        public SolidRpcOpenApiInitAdvice(
            IMethodBinderStore methodBinderStore)
        {
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
        }

        private IMethodBinderStore MethodBinderStore { get; }
        private bool HasImplementation { get; set; }
        private IMethodBinding MethodBinding { get; set; }
        private string ProxyTransportType { get; set; }
        private string InvokerTransportType { get; set; }
        
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
            if(MethodBinding == null)
            {
                return false;
            }
                 

            HasImplementation = config.InvocationConfiguration.HasImplementation;

            //
            // Determine transport type. If not explititly set use Http
            // if no implementation exists.
            //
            ProxyTransportType = config.ProxyTransportType;
            if (string.IsNullOrEmpty(ProxyTransportType))
            {
                if (HasImplementation)
                {
                    ProxyTransportType = LocalHandler.TransportType;
                }
                else
                {
                    ProxyTransportType = HttpHandler.TransportType;
                }
            }

            //
            // Get invoker transport + handler
            //
            InvokerTransportType = MethodBinding.Transports.Where(o => o.InvocationStrategy == InvocationStrategy.Invoke).Select(o => o.TransportType).FirstOrDefault();
            if (InvokerTransportType == null)
            {
                throw new Exception($"No invocation transport configured");
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
            //
            // setup invocation options
            //
            var invocationOptions = invocation.GetValue<InvocationOptions>(typeof(InvocationOptions).FullName);
            if (invocationOptions == null)
            {
                string transportType;
                if (invocation.Caller is ISolidProxy)
                {
                    transportType = ProxyTransportType;
                }
                else if (HasImplementation)
                {
                    transportType = LocalHandler.TransportType;
                }
                else
                {
                    transportType = InvokerTransportType;
                }

                var transport = MethodBinding.Transports.Single(o => o.TransportType == transportType);
                invocationOptions = new InvocationOptions(transport.TransportType, transport.MessagePriority, transport.PreInvokeCallback, transport.PostInvokeCallback);
            }

            //
            // assign to invocation
            //
            invocation.SetValue(typeof(InvocationOptions).FullName, invocationOptions);

            return next();
        }
    }
}
