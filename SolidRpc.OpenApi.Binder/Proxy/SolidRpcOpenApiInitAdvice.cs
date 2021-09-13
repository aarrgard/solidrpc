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
        private ITransport ProxyTransport { get; set; }
        private ITransport InvokerTransport { get; set; }
        private ITransport LocalTransport { get; set; }
        private bool _initialized = false;
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

            var transports = MethodBinding.Transports;

            //
            // Determine transport type. If not explititly set use Http
            // if no implementation exists.
            //
            var proxyTransportType = config.ProxyTransportType;
            if (HasImplementation)
            {
                LocalTransport = transports.OfType<ILocalTransport>().FirstOrDefault();
                if(LocalTransport == null)
                {
                    throw new Exception("Local implementation exists but no local transport configured.");
                }
            }
            var httpTransport = transports.FirstOrDefault(o => o.GetTransportType() == "Http");
            ProxyTransport = transports.FirstOrDefault(o => o.GetTransportType() == proxyTransportType) ?? LocalTransport ?? httpTransport;

            //
            // Get invoker transport + handler
            //
            InvokerTransport = transports.FirstOrDefault();
            if (InvokerTransport == null)
            {
                throw new Exception($"No invocation transport configured");
            }

            _initialized = true;

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
            if (!_initialized) throw new Exception("Not initialized");
            //
            // setup invocation options
            //
            var invocationOptions = invocation.GetValue<InvocationOptions>(typeof(InvocationOptions).FullName);
            if (invocationOptions == null)
            {
                ITransport transport;
                if (invocation.Caller is ISolidProxy)
                {
                    transport = ProxyTransport;
                }
                else if (HasImplementation)
                {
                    transport = LocalTransport;
                }
                else
                {
                    transport = InvokerTransport;
                }

                invocationOptions = new InvocationOptions(transport.GetTransportType(), transport.MessagePriority, null, transport.PreInvokeCallback, transport.PostInvokeCallback);
            }

            //
            // assign to invocation
            //
            invocation.SetValue(typeof(InvocationOptions).FullName, invocationOptions);

            return next();
        }
    }
}
