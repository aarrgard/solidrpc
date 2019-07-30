using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Model.V2;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

[assembly: SolidRpc.Abstractions.SolidRpcAbstractionProvider(typeof(IMethodBinderStore), typeof(MethodBinderStore))]
namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Implementation for the method binder.
    /// </summary>
    public class MethodBinderStore : IMethodBinderStore
    {
        public MethodBinderStore(IServiceProvider serviceProvider, ISolidProxyConfigurationStore configStore, IOpenApiParser openApiParser)
        {
            Bindings = new ConcurrentDictionary<string, IMethodBinder>();
            ConfigStore = configStore;
            OpenApiParser = openApiParser;
            ServiceProvider = serviceProvider;
        }
        private ConcurrentDictionary<string, IMethodBinder> Bindings { get; }
        private ISolidProxyConfigurationStore ConfigStore { get; }
        private IOpenApiParser OpenApiParser { get; }
        private IServiceProvider ServiceProvider { get; }

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
                        .ToList()
                        .ForEach(o =>
                        {
                            var config = o.GetOpenApiConfiguration();
                            var method = o.InvocationConfiguration.MethodInfo;
                            var assembly = method.DeclaringType.Assembly;
                            var uriTransformer = o.BaseUriTransformer;
                            var methodBinder = GetMethodBinder(config, assembly, uriTransformer);
                            methodBinder.GetMethodInfo(method);
                        });
                    _methodBinders = Bindings.Values;
                }
                return _methodBinders;
            }
        }

        public IMethodBinder GetMethodBinder(string openApiSpec, Assembly assembly, BaseUriTransformer baseUriTransformer = null)
        {
            if (openApiSpec == null) throw new ArgumentNullException(nameof(openApiSpec));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            var key = $"{assembly.GetName().FullName}:{assembly.GetName().Version}:{openApiSpec}";
            return Bindings.GetOrAdd(key, _ => CreateMethodBinder(openApiSpec, assembly, baseUriTransformer));
        }

        private IMethodBinder CreateMethodBinder(string openApiSpec, Assembly assembly, BaseUriTransformer baseUriTransformer)
        {
            if (baseUriTransformer == null) baseUriTransformer = (sp, uri) => uri;
            var swaggerSpec = OpenApiParser.ParseSpec(openApiSpec);
            if (swaggerSpec is SwaggerObject v2)
            {
                var mb = new V2.MethodBinderV2(ServiceProvider, v2, assembly, baseUriTransformer);
                return mb;
            }
            throw new NotImplementedException($"Cannot get binder for {swaggerSpec.GetType().FullName}");
        }

        public IMethodInfo GetMethodInfo(string openApiSpec, MethodInfo methodInfo, BaseUriTransformer baseUriTransformer)
        {
            if (openApiSpec == null) throw new ArgumentNullException(nameof(openApiSpec));
            return GetMethodBinder(openApiSpec, methodInfo.DeclaringType.Assembly, baseUriTransformer).GetMethodInfo(methodInfo);
        }

        public async Task<Uri> GetUrlAsync<T>(Expression<Action<T>> expression)
        {
            // find the binding
            var (mi, args) = GetMethodInfoAndArguments(expression);
            var imi = MethodBinders
                .Where(o => o.Assembly == mi.DeclaringType.Assembly)
                .SelectMany(o => o.MethodInfos)
                .Where(o => o.MethodInfo == mi)
                .FirstOrDefault();
            if(imi == null)
            {
                return null;
            }
            var req = new SolidHttpRequest();
            await imi.BindArgumentsAsync(req, args);
            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = req.Scheme;
            var hostParts = req.HostAndPort.Split(':');
            uriBuilder.Host = hostParts[0];
            if (hostParts.Length > 1)
            {
                uriBuilder.Port = int.Parse(hostParts[1]);
            }
            uriBuilder.Path = req.Path;
            uriBuilder.Query = string.Join("&", req.Query.Select(o => $"{o.Name}={o.GetStringValue()}"));
            return uriBuilder.Uri;
        }

        private (MethodInfo, object[]) GetMethodInfoAndArguments(LambdaExpression expression)
        {
            var mce = expression.Body as MethodCallExpression;
            if(mce == null)
            {
                throw new Exception("expression should be a method call.");
            }
            var args = new object[mce.Arguments.Count];
            for(int i = 0; i < args.Length; i++)
            {
                var argExpr = Expression.Convert(mce.Arguments[i], typeof(object));
                var argExpression = Expression.Lambda<Func<object>>(argExpr, new ParameterExpression[0]);
                args[i] = argExpression.Compile()();
            }
            return (mce.Method, args);
        }
    }
}
