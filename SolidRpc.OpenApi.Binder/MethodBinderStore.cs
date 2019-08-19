using Microsoft.Extensions.DependencyInjection;
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
        private object _mutext = new object();

        public MethodBinderStore(IServiceProvider serviceProvider, ISolidProxyConfigurationStore configStore, IOpenApiParser openApiParser)
        {
            Bindings = new ConcurrentDictionary<string, IMethodBinder>();
            OriginalSpecs = new ConcurrentDictionary<string, IOpenApiSpec>();
            ConfigStore = configStore;
            OpenApiParser = openApiParser;
            ServiceProvider = serviceProvider;
        }
        private ConcurrentDictionary<string, IMethodBinder> Bindings { get; }
        private ConcurrentDictionary<string, IOpenApiSpec> OriginalSpecs { get; }
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
                    lock(_mutext)
                    {
                        if (_methodBinders == null)
                        {
                            ConfigStore.ProxyConfigurations.ToList()
                                .SelectMany(o => o.InvocationConfigurations)
                                .Where(o => o.IsAdviceConfigured<ISolidRpcOpenApiConfig>())
                                .Select(o => o.ConfigureAdvice<ISolidRpcOpenApiConfig>())
                                .ToList()
                                .ForEach(o =>
                                {
                                    var config = o.GetOpenApiConfiguration();
                                    var method = o.InvocationConfiguration.MethodInfo;
                                    var assembly = method.DeclaringType.Assembly;
                                    var uriTransformer = o.MethodAddressTransformer;
                                    var methodBinder = GetMethodBinder(uriTransformer, config, assembly);
                                    methodBinder.CreateMethodBinding(method, uriTransformer);
                                });
                            _methodBinders = Bindings.Values;
                        }
                    }
                }
                return _methodBinders;
            }
        }

        private IMethodBinder GetMethodBinder(MethodAddressTransformer methodAddressTransformer, string openApiSpec, Assembly assembly)
        {
            if (methodAddressTransformer == null)
            {
                methodAddressTransformer = (sp, uri, mi) => sp.GetService<IMethodAddressTransformer>()?.TransformUriAsync(uri, null) ?? Task.FromResult(uri);
            }
            if (openApiSpec == null) throw new ArgumentNullException(nameof(openApiSpec));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            var originalSpec = OriginalSpecs.GetOrAdd(openApiSpec, _ => OpenApiParser.ParseSpec(_));
            var baseAddress = methodAddressTransformer(ServiceProvider, originalSpec.BaseAddress, null).Result;
            var key = $"{baseAddress.ToString()}:{assembly.GetName().Name}:{assembly.GetName().Version}:{openApiSpec}";
            return Bindings.GetOrAdd(key, _ => CreateMethodBinder(openApiSpec, baseAddress, assembly));
        }

        private IMethodBinder CreateMethodBinder(string openApiSpec, Uri baseAddress, Assembly assembly)
        {
            var swaggerSpec = OpenApiParser.ParseSpec(openApiSpec);
            swaggerSpec.SetBaseAddress(baseAddress);
            if (swaggerSpec is SwaggerObject v2)
            {
                var mb = new V2.MethodBinderV2(ServiceProvider, v2, assembly);
                return mb;
            }
            throw new NotImplementedException($"Cannot get binder for {swaggerSpec.GetType().FullName}");
        }

        public IMethodBinding CreateMethodBinding(string openApiSpec, MethodInfo methodInfo, MethodAddressTransformer baseUriTransformer = null)
        {
            if (openApiSpec == null) throw new ArgumentNullException(nameof(openApiSpec));
            var methodBinder = GetMethodBinder(baseUriTransformer, openApiSpec, methodInfo.DeclaringType.Assembly);
            return methodBinder.CreateMethodBinding(methodInfo, baseUriTransformer);
        }

        public async Task<Uri> GetUrlAsync<T>(Expression<Action<T>> expression)
        {
            // find the binding
            var (mi, args) = GetMethodInfoAndArguments(expression);
            var imi = MethodBinders
                .SelectMany(o => o.MethodBindings)
                .Where(o => MethodMatches(o.MethodInfo,mi))
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

        private bool MethodMatches(MethodInfo mi1, MethodInfo mi2)
        {
            if (mi1.Name != mi2.Name)
            {
                return false;
            }
            if (mi1.DeclaringType != mi2.DeclaringType)
            {
                return false;
            }
            var mi1p = mi1.GetParameters();
            var mi2p = mi2.GetParameters();
            if (mi1p.Length != mi2p.Length)
            {
                return false;
            }
            for(int i = 0; i < mi1p.Length; i++) 
            {
                if (mi1p[i].Name != mi2p[i].Name)
                {
                    return false;
                }
                if (mi1p[i].ParameterType != mi2p[i].ParameterType)
                {
                    return false;
                }
            }
            return true;
        }

        private (MethodInfo, object[]) GetMethodInfoAndArguments(LambdaExpression expression)
        {
            if (!(expression.Body is MethodCallExpression mce))
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

        public IMethodBinding GetMethodBinding<T>(Expression<Action<T>> expression)
        {
            var (mi, args) = GetMethodInfoAndArguments(expression);
            return GetMethodBinding(mi);
        }

        public IMethodBinding GetMethodBinding(MethodInfo methodInfo)
        {
            return MethodBinders.SelectMany(o => o.MethodBindings)
                .Where(o => o.MethodInfo == methodInfo)
                .FirstOrDefault();
        }
    }
}
