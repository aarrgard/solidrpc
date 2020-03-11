using Microsoft.Extensions.Logging;
using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Model;
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

        public MethodBinderStore(ILogger<MethodBinderStore> logger, IServiceProvider serviceProvider, ISolidProxyConfigurationStore configStore, IOpenApiParser openApiParser)
        {
            Logger = logger;
            Bindings = new ConcurrentDictionary<string, IMethodBinder>();
            ParsedSpecs = new ConcurrentDictionary<string, IOpenApiSpec>();
            SpecResolvers = new ConcurrentDictionary<string, IOpenApiSpecResolver>();
            ConfigStore = configStore;
            OpenApiParser = openApiParser;
            ServiceProvider = serviceProvider;
        }
        private ConcurrentDictionary<string, IMethodBinder> Bindings { get; }
        private ConcurrentDictionary<string, IOpenApiSpec> ParsedSpecs { get; }
        private ConcurrentDictionary<string, IOpenApiSpecResolver> SpecResolvers { get; }
        private ISolidProxyConfigurationStore ConfigStore { get; }
        private ILogger Logger { get; }
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
                            Logger.LogInformation("Creating method binders...");
                            ConfigStore.ProxyConfigurations.ToList()
                                .SelectMany(o => o.InvocationConfigurations)
                                .Where(o => o.IsAdviceConfigured<ISolidRpcOpenApiConfig>())
                                .ToList()
                                .ForEach(invocConfig =>
                                {
                                    var apiConfig = invocConfig.ConfigureAdvice<ISolidRpcOpenApiConfig>();
                                    var config = apiConfig.OpenApiSpec;
                                    var method = apiConfig.InvocationConfiguration.MethodInfo;
                                    var assembly = method.DeclaringType.Assembly;
                                    var uriTransformer = apiConfig.MethodAddressTransformer;
                                    var securityKey = apiConfig.SecurityKey;
                                    var openApiSpec = GetOpenApiSpec(config, invocConfig.HasImplementation, method, uriTransformer);
                                    var methodBinder = GetMethodBinder(openApiSpec, assembly);
                                    var methodBinding = methodBinder.CreateMethodBinding(method, uriTransformer, securityKey);
                                });
                            _methodBinders = Bindings.Values;
                            Logger.LogInformation("...created method binders.");
                        }
                    }
                }
                return _methodBinders;
            }
        }

        private IMethodBinder GetMethodBinder(IOpenApiSpec openApiSpec, Assembly assembly)
        {
            if (openApiSpec == null) throw new ArgumentNullException(nameof(openApiSpec));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            var key = $"{assembly.GetName().Name}:{assembly.GetName().Version}:{openApiSpec.GetHashCode()}";
            return Bindings.GetOrAdd(key, _ => CreateMethodBinder(openApiSpec, assembly));
        }

        private IOpenApiSpec GetOpenApiSpec(string openApiSpec, bool localApi, Assembly assembly, MethodAddressTransformer methodAddressTransformer)
        {
            IOpenApiSpec parsedSpec;
            var openApiSpecResolver = GetOpenApiSpecResolver(assembly);
            if (openApiSpec == null)
            {
                var assemblyName = assembly.GetName().Name;
                if(!openApiSpecResolver.TryResolveApiSpec($".{assemblyName}.json", out parsedSpec))
                {
                    throw new Exception("Cannot find openapi spec in assembly:" + assemblyName);
                }
            }
            else
            {
                parsedSpec = ParsedSpecs.GetOrAdd(openApiSpec, _ =>
                {
                    return OpenApiParser.ParseSpec(openApiSpecResolver, "local", openApiSpec);
                });
            }
            return GetOpenApiSpec(parsedSpec, localApi, methodAddressTransformer);
        }

        private IOpenApiSpec GetOpenApiSpec(string openApiSpec, bool localApi, MethodInfo methodInfo, MethodAddressTransformer methodAddressTransformer)
        {
            IOpenApiSpec parsedSpec;
            var assembly = methodInfo.DeclaringType.Assembly;
            var openApiSpecResolver = GetOpenApiSpecResolver(assembly);
            if (openApiSpec == null)
            {
                if (!openApiSpecResolver.TryResolveApiSpec($"{methodInfo.DeclaringType.FullName}.json", out parsedSpec))
                {
                    return GetOpenApiSpec(openApiSpec, localApi, assembly, methodAddressTransformer);
                }
            }
            else
            {
                parsedSpec = ParsedSpecs.GetOrAdd(openApiSpec, _ =>
                {
                    return OpenApiParser.ParseSpec(openApiSpecResolver, "local", openApiSpec);
                });
            }
            return GetOpenApiSpec(parsedSpec, localApi, methodAddressTransformer);
        }

        public IOpenApiSpecResolver GetOpenApiSpecResolver(Assembly assembly)
        {
            return SpecResolvers.GetOrAdd(assembly.GetName().Name, _ =>
            {
                var resolver = new OpenApiSpecResolverAssembly(OpenApiParser);
                resolver.AddAssemblyResources(assembly);
                return resolver;
            });
        }

        private IOpenApiSpec GetOpenApiSpec(IOpenApiSpec openApiSpec, bool localApi, MethodAddressTransformer methodAddressTransformer)
        {
            if (methodAddressTransformer == null)
            {
                methodAddressTransformer = (sp, uri, mi) =>
                {
                    if(!localApi)
                    {
                        return Task.FromResult(uri);
                    }
                    var trans = (IMethodAddressTransformer)sp.GetService(typeof(IMethodAddressTransformer));
                    if(trans != null)
                    {
                        return trans.TransformUriAsync(uri, mi);
                    }
                    else
                    {
                        return Task.FromResult(uri);
                    }
                };
            }
            var newBaseAddress = methodAddressTransformer(ServiceProvider, openApiSpec.BaseAddress, null).Result;
            if (newBaseAddress == openApiSpec.BaseAddress)
            {
                return openApiSpec;
            }
            var key = $"{newBaseAddress.ToString()}{openApiSpec.GetHashCode()}";
            IOpenApiSpec newParsedSpec;
            if(!ParsedSpecs.TryGetValue(key, out newParsedSpec)) {
                newParsedSpec = openApiSpec.SetBaseAddress(newBaseAddress);
                ParsedSpecs[key] = newParsedSpec;
            }
            return newParsedSpec;
        }

        private IMethodBinder CreateMethodBinder(IOpenApiSpec openApiSpec, Assembly assembly)
        {
            if (openApiSpec is SwaggerObject v2)
            {
                var mb = new V2.MethodBinderV2(ServiceProvider, v2, assembly);
                return mb;
            }
            throw new NotImplementedException($"Cannot get binder for {openApiSpec.GetType().FullName}");
        }

        public IMethodBinding CreateMethodBinding(string openApiSpec, bool localApi, MethodInfo methodInfo, MethodAddressTransformer baseUriTransformer = null)
        {
            var parsedSpec = GetOpenApiSpec(openApiSpec, localApi, methodInfo, baseUriTransformer);
            var methodBinder = GetMethodBinder(parsedSpec, methodInfo.DeclaringType.Assembly);
            return methodBinder.CreateMethodBinding(methodInfo, baseUriTransformer, null);
        }

        public async Task<Uri> GetUrlAsync<T>(Expression<Action<T>> expression, bool includeQueryString = true)
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
            if(includeQueryString)
            {
                uriBuilder.Query = string.Join("&", req.Query.Select(o => $"{o.Name}={o.GetStringValue()}"));
            }
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
