using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

[assembly: SolidRpc.Abstractions.SolidRpcService(typeof(IMethodBinderStore), typeof(MethodBinderStore))]
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
            MethodBindings = new ConcurrentDictionary<MethodInfo, IEnumerable<IMethodBinding>>();
            ConfigStore = configStore;
            OpenApiParser = openApiParser;
            ServiceProvider = serviceProvider;
        }
        private ConcurrentDictionary<string, IMethodBinder> Bindings { get; }
        private ConcurrentDictionary<string, IOpenApiSpec> ParsedSpecs { get; }
        private ConcurrentDictionary<string, IOpenApiSpecResolver> SpecResolvers { get; }
        private ConcurrentDictionary<MethodInfo, IEnumerable<IMethodBinding>> MethodBindings { get; }
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
                    bool dispatchEvents = false;
                    lock(_mutext)
                    {
                        if (_methodBinders == null)
                        {
                            dispatchEvents = true;
                            Logger.LogInformation("Creating method binders...");
                            ConfigStore.ProxyConfigurations.ToList()
                                .SelectMany(o => o.InvocationConfigurations)
                                .Where(o => o.IsAdviceConfigured<ISolidRpcOpenApiConfig>())
                                .ToList()
                                .ForEach(invocConfig =>
                                {
                                    GetMethodBinding(invocConfig.MethodInfo);
                                });
                            _methodBinders = Bindings.Values;
                            Logger.LogInformation("...created method binders.");
                        }
                    }
                    if(dispatchEvents)
                    {
                        ServiceProvider.GetRequiredService<IEnumerable<IMethodBindingHandler>>().ToList().ForEach(handler =>
                        {
                            Bindings.Values.SelectMany(o => o.MethodBindings).ToList().ForEach(binding =>
                            {
                                handler.BindingCreated(binding);
                            });
                        });
                    }
                }
                return _methodBinders;
            }
        }

        private IMethodBinder GetMethodBinder(IOpenApiSpec openApiSpec, Assembly assembly)
        {
            if (openApiSpec == null) throw new ArgumentNullException(nameof(openApiSpec));
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            var key = $"{assembly.GetName().Name}:{openApiSpec.OpenApiSpecResolverAddress}";
            return Bindings.GetOrAdd(key, _ => CreateMethodBinder(openApiSpec, assembly));
        }

        private IOpenApiSpec GetOpenApiSpec(string openApiSpec, Assembly assembly)
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
            return parsedSpec;
        }

        private IOpenApiSpec GetOpenApiSpec(string strOpenApiSpec, MethodInfo methodInfo)
        {
            IOpenApiSpec parsedSpec;
            var assembly = methodInfo.DeclaringType.Assembly;
            var openApiSpecResolver = GetOpenApiSpecResolver(assembly);
            if (strOpenApiSpec == null)
            {
                if (!openApiSpecResolver.TryResolveApiSpec($"{methodInfo.DeclaringType.FullName}.json", out parsedSpec))
                {
                    return GetOpenApiSpec(strOpenApiSpec, assembly);
                }
            }
            else
            {
                parsedSpec = ParsedSpecs.GetOrAdd(strOpenApiSpec, _ =>
                {
                    parsedSpec = OpenApiParser.ParseSpec(openApiSpecResolver, "local", strOpenApiSpec);
                    var md5Source = parsedSpec.Title + string.Join(";", parsedSpec.Operations.Select(o => o.OperationId));
                    var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(md5Source));
                    var hex = BitConverter.ToString(hash).Replace("-", string.Empty);
                    parsedSpec.SetOpenApiSpecResolver(openApiSpecResolver, hex);
                    return parsedSpec;
                });
            }
            return parsedSpec;
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

        private IMethodBinder CreateMethodBinder(IOpenApiSpec openApiSpec, Assembly assembly)
        {
            if (openApiSpec is SwaggerObject v2)
            {
                var mb = new V2.MethodBinderV2(ServiceProvider, v2, assembly);
                return mb;
            }
            throw new NotImplementedException($"Cannot get binder for {openApiSpec.GetType().FullName}");
        }

        public IEnumerable<IMethodBinding> CreateMethodBindings(
            string openApiSpec,
            MethodInfo methodInfo,
            IEnumerable<ITransport> transports = null,
            KeyValuePair<string, string>? securityKey = null)
        {
            if (transports == null) transports = new ITransport[0];
            var parsedSpec = GetOpenApiSpec(openApiSpec, methodInfo);
            var methodBinder = GetMethodBinder(parsedSpec, methodInfo.DeclaringType.Assembly);
            return methodBinder.CreateMethodBindings(methodInfo, transports, securityKey);
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
            return MethodBindings.GetOrAdd(methodInfo, CreateMethodBinding).FirstOrDefault();
        }

        private IEnumerable<IMethodBinding> CreateMethodBinding(MethodInfo mi)
        {
            var invocConfig = ConfigStore.ProxyConfigurations
                .SelectMany(o => o.InvocationConfigurations)
                .Where(o => o.MethodInfo == mi).FirstOrDefault();
            if(invocConfig == null)
            {
                throw new ArgumentException("No proxy configuration exists for method:"+mi.Name);
            }
            if(!invocConfig.IsAdviceConfigured<ISolidRpcOpenApiConfig>())
            {
                throw new ArgumentException("No solid rpc open api configuration exists for method:"+mi.Name);
            }
            var apiConfig = invocConfig.ConfigureAdvice<ISolidRpcOpenApiConfig>();
            var config = apiConfig.OpenApiSpec;
            var method = apiConfig.InvocationConfiguration.MethodInfo;
            var assembly = method.DeclaringType.Assembly;
            var openApiSpec = GetOpenApiSpec(config, method);
            var methodBinder = GetMethodBinder(openApiSpec, assembly);

            var securityKey = apiConfig.SecurityKey;
            return methodBinder.CreateMethodBindings(method, apiConfig.GetTransports(), securityKey);
        }
    }
}
