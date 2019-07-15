using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.OpenApi.Binder.Proxy;
using SolidRpc.OpenApi.Model.V2;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Implementation for the method binder.
    /// </summary>
    public class MethodBinderStore : IMethodBinderStore
    {
        public MethodBinderStore(ISolidProxyConfigurationStore configStore)
        {
            Bindings = new ConcurrentDictionary<string, IMethodBinder>();
            ConfigStore = configStore;
        }
        private ConcurrentDictionary<string, IMethodBinder> Bindings { get; }
        public ISolidProxyConfigurationStore ConfigStore { get; }

        private IEnumerable<IMethodBinder> _methodBinders;
        public IEnumerable<IMethodBinder> MethodBinders
        {
            get
            {
                if(_methodBinders == null)
                {
                    ConfigStore.ProxyConfigurations
                        .SelectMany(o => o.InvocationConfigurations)
                        .Where(o => o.IsAdviceConfigured<ISolidRpcOpenApiConfig>())
                        .Select(o => o.ConfigureAdvice<ISolidRpcOpenApiConfig>())
                        .Select(o => new { o.OpenApiConfiguration, o.InvocationConfiguration.MethodInfo.DeclaringType.Assembly })
                        .Distinct()
                        .ToList()
                        .ForEach(o => GetMethodBinder(o.OpenApiConfiguration, o.Assembly));
                    _methodBinders = Bindings.Values;
                }
                return _methodBinders;
            }
        }

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
