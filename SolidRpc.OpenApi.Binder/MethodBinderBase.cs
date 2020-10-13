using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.Transport;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Base class for the method binders in V2 and V3
    /// </summary>
    public abstract class MethodBinderBase : IMethodBinder
    {

        protected MethodBinderBase(IServiceProvider serviceProvider, IOpenApiSpec openApiSpec, Assembly assembly)
        {
            ServiceProvider = serviceProvider;
            OpenApiSpec = openApiSpec ?? throw new ArgumentNullException(nameof(openApiSpec));
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            CachedBindings = new ConcurrentDictionary<MethodInfo, IEnumerable<IMethodBinding>>();
            HostedAddress = TransformAddress(OpenApiSpec.BaseAddress);
            ConfigStore = ServiceProvider.GetRequiredService<ISolidProxyConfigurationStore>();
        }

        private Uri TransformAddress(Uri uri)
        {
            var configuration = (IConfiguration)ServiceProvider.GetService(typeof(IConfiguration));
            if (configuration == null) return uri;
            var methodAddressResolver = (IMethodAddressTransformer)ServiceProvider.GetService(typeof(IMethodAddressTransformer));
            if (methodAddressResolver == null) return uri;
            return methodAddressResolver.TransformUri(uri, null);
        }

        public IServiceProvider ServiceProvider { get; }
        public IOpenApiSpec OpenApiSpec { get; }
        public Assembly Assembly { get; }

        public IEnumerable<IMethodBinding> MethodBindings
        {
            get
            {
                return CachedBindings.Values.SelectMany(o => o);
            }
        }

        public Uri HostedAddress { get; }
        protected ISolidProxyConfigurationStore ConfigStore { get; }
        private ConcurrentDictionary<MethodInfo, IEnumerable<IMethodBinding>> CachedBindings { get; }

        public IEnumerable<IMethodBinding> CreateMethodBindings(
            MethodInfo methodInfo,
            IEnumerable<ITransport> transports)
        {
            return CachedBindings.GetOrAdd(methodInfo, _ => CreateBindings(_, transports));
        }

        private IEnumerable<IMethodBinding> CreateBindings(
            MethodInfo mi,
            IEnumerable<ITransport> transports)
        {
            var methodBindings = DoCreateMethodBinding(mi, transports);

            // sort em according to the preferred method
            methodBindings = methodBindings.OrderBy(o => GetMethodIdx(o.Method));

            // Configure transports
            methodBindings.ToList().ForEach(methodBinding =>
            {
                transports.ToList().ForEach(o =>
                {
                    o.Configure(methodBinding);
                });
                
            });

            return methodBindings;
        }

        private int GetMethodIdx(string method)
        {
            switch(method.ToLower())
            {
                case "get": return 1;
                case "options": return 2;
                case "put": return 3;
                case "delete": return 4;
                default: return 100;
            }
        }

        protected abstract IEnumerable<IMethodBinding> DoCreateMethodBinding(
            MethodInfo mi,
            IEnumerable<ITransport> transports);
    }
}

