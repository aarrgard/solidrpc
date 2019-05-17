using System.Collections.Concurrent;
using System.Reflection;

namespace SolidRpc.Swagger.Binder
{
    /// <summary>
    /// Base class for the method binders in V2 and V3
    /// </summary>
    public abstract class MethodBinderBase : IMethodBinder
    {

        protected MethodBinderBase()
        {
            CachedBindings = new ConcurrentDictionary<MethodInfo, IMethodInfo>();
        }

        private ConcurrentDictionary<MethodInfo, IMethodInfo> CachedBindings { get; }

        public IMethodInfo GetMethodInfo(MethodInfo methodInfo)
        {
            return CachedBindings.GetOrAdd(methodInfo, FindBinding);
        }

        protected abstract IMethodInfo FindBinding(MethodInfo arg);
    }
}
