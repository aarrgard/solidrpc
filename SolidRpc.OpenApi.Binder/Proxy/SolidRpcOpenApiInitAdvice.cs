﻿using System;
using System.Threading.Tasks;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;
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
    public class SolidRpcOpenApiInitAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public static IEnumerable<Type> BeforeAdvices = new Type[] { typeof(SolidRpcOpenApiInvocAdvice<,,>) };

        /// <summary>
        /// Constucts a new instance
        /// </summary>
        /// <param name="methodBinderStore"></param>
        /// <param name="handlers"></param>
        public SolidRpcOpenApiInitAdvice(
            IMethodBinderStore methodBinderStore,
            IEnumerable<IHandler> handlers)
        {
            MethodBinderStore = methodBinderStore ?? throw new ArgumentNullException(nameof(methodBinderStore));
            Handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
        }

        private IMethodBinderStore MethodBinderStore { get; }
        private IEnumerable<IHandler> Handlers { get; }
        private bool HasImplementation { get; set; }
        private IMethodBinding MethodBinding { get; set; }
        private IHandler LocalHandler { get; set; }
        private IHandler ProxyHandler { get; set; }
        private IHandler InvokerHandler { get; set; }

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

            LocalHandler = Handlers.Where(o => o.TransportType == "Local").FirstOrDefault();

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

            //
            // Get proxy handler
            //
            ProxyHandler = Handlers.Where(o => o.TransportType == proxyTransportType).FirstOrDefault();
            if(ProxyHandler == null)
            {
                throw new Exception($"Cannot find the handler for {proxyTransportType} transport.");
            }

            //
            // Get invoker transport + handler
            //
            var invokerTransportType = MethodBinding.Transports.Where(o => o.InvocationStrategy == InvocationStrategy.Invoke).Select(o => o.TransportType).FirstOrDefault();
            if (invokerTransportType == null)
            {
                throw new Exception($"No invocation transport configured");
            }
            InvokerHandler = Handlers.Where(o => o.TransportType == invokerTransportType).FirstOrDefault();
            if (InvokerHandler == null)
            {
                throw new Exception($"Cannot find the handler for {invokerTransportType} transport.");
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
            // determine handler
            //
            IHandler handler = invocation.GetValue<IHandler>(typeof(IHandler).FullName);
            if(handler == null)
            {
                if (invocation.Caller is ISolidProxy)
                {
                    handler = ProxyHandler;
                }
                else if (HasImplementation)
                {
                    handler = LocalHandler;
                }
                else
                {
                    handler = InvokerHandler;
                }
            }

            //
            // setup invocation options
            //
            var invocationOptions = invocation.GetValue<InvocationOptions>(typeof(InvocationOptions).FullName);
            if (invocationOptions == null)
            {
                invocationOptions = new InvocationOptions(handler.TransportType, InvocationOptions.MessagePriorityNormal);
            }

            //
            // assign to invocation
            //
            invocation.SetValue(typeof(IHandler).FullName, handler);
            invocation.SetValue(typeof(InvocationOptions).FullName, invocationOptions);

            return next();
        }
    }
}