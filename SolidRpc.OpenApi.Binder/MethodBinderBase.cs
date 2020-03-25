using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        protected MethodBinderBase(IOpenApiSpec openApiSpec, Assembly assembly)
        {
            OpenApiSpec = openApiSpec;
            Assembly = assembly;
            CachedBindings = new ConcurrentDictionary<MethodInfo, IMethodBinding>();
        }

        public IOpenApiSpec OpenApiSpec { get; }

        public Assembly Assembly { get; }

        public IEnumerable<IMethodBinding> MethodBindings
        {
            get
            {
                return CachedBindings.Values;
            }
        }

        private ConcurrentDictionary<MethodInfo, IMethodBinding> CachedBindings { get; }

        public IMethodBinding CreateMethodBinding(
            MethodInfo methodInfo,
            IEnumerable<ITransport> transports,
            KeyValuePair<string, string>? securityKey)
        {
            return CachedBindings.GetOrAdd(methodInfo, _ => CreateBinding(_, transports, securityKey));
        }

        private IMethodBinding CreateBinding(
            MethodInfo mi,
            IEnumerable<ITransport> transports,
            KeyValuePair<string, string>? securityKey)
        {
            var methodBinding = DoCreateMethodBinding(mi, transports, securityKey);
            transports.ToList().ForEach(o => o.Configure(methodBinding));
            return methodBinding;
        }

        protected abstract IMethodBinding DoCreateMethodBinding(
            MethodInfo mi,
            IEnumerable<ITransport> transports,
            KeyValuePair<string, string>? securityKey);
    }
}

