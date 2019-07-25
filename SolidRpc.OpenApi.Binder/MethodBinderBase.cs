using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SolidRpc.OpenApi.Model;

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

        private ConcurrentDictionary<MethodInfo, IMethodInfo> CachedBindings { get; }

        public IMethodInfo GetMethodInfo(MethodInfo methodInfo)
        {
            return CachedBindings.GetOrAdd(methodInfo, o => FindBinding(o, true));
        }

        protected abstract IMethodInfo FindBinding(MethodInfo arg, bool mustExist);
    }
}


