using SolidRpc.OpenApi.Model.V2;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Implementation for the method binder.
    /// </summary>
    public class MethodBinderStore : IMethodBinderStore
    {
        public MethodBinderStore()
        {
            Bindings = new ConcurrentDictionary<string, IMethodBinder>();
        }
        private ConcurrentDictionary<string, IMethodBinder> Bindings { get; }

        public IMethodBinder GetMethodBinder(string openApiSpec, Assembly assembly)
        {
            var key = $"{assembly.GetName().FullName}:{assembly.GetName().Version}:{openApiSpec}";
            return Bindings.GetOrAdd(key, _ => CreateMethodBinder(openApiSpec, assembly));
        }

        private IMethodBinder CreateMethodBinder(string openApiSpec, Assembly assembly)
        {
            var swaggerSpec = SolidRpc.OpenApi.Model.OpenApiParser.ParseOpenApiSpec(openApiSpec);
            if (swaggerSpec is SwaggerObject v2)
            {
                var mb = new Binder.V2.MethodBinderV2(v2, assembly);
                return mb;
            }
            throw new NotImplementedException($"Cannot get binder for {swaggerSpec.GetType().FullName}");
        }

        public IMethodInfo GetMethodInfo(string openApiSpec, MethodInfo methodInfo)
        {
            return GetMethodBinder(openApiSpec, methodInfo.DeclaringType.Assembly).GetMethodInfo(methodInfo);
        }
    }
}
