using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Model;

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
            OpenApiSpec = openApiSpec;
            Assembly = assembly;
            CachedBindings = new ConcurrentDictionary<MethodInfo, IMethodInfo>();
        }

        public IOpenApiSpec OpenApiSpec { get; }

        public Assembly Assembly { get; }

        public IEnumerable<IMethodInfo> MethodInfos
        {
            get
            {
                 return CachedBindings.Values;
            }
        }

        public IServiceProvider ServiceProvider { get; }

        private ConcurrentDictionary<MethodInfo, IMethodInfo> CachedBindings { get; }

        public IMethodInfo GetMethodInfo(MethodInfo methodInfo, BaseUriTransformer baseUriTransformer)
        {
            return CachedBindings.GetOrAdd(methodInfo, _ => CreateBinding(_, true));
        }

        protected abstract IMethodInfo CreateBinding(MethodInfo mi, bool mustExist);
    }
}


