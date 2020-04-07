using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services;
using SolidRpc.Abstractions.Types;
using SolidRpc.OpenApi.AspNetCore.Services;
using SolidRpc.OpenApi.AzFunctions.Functions;
using SolidRpc.OpenApi.AzFunctions.Functions.Impl;

namespace SolidRpc.OpenApi.AzFunctions.Services
{
    /// <summary>
    /// The setup class
    /// </summary>
    public abstract class SolidRpcHostAzFunctions : SolidRpcHost
    {
        private class FunctionDef
        {
            public FunctionDef(IAzFunctionHandler functionHandler, string protocol, string openApiPath, string path)
            {
                FunctionHandler = functionHandler;
                Protocol = protocol;
                FunctionName = CreateFunctionName(openApiPath);
                Path = FixupPath(path);
            }

            private string FixupPath(string path)
            {
                // remove frontend prefix
                if (path.StartsWith($"{FunctionHandler.HttpRouteFrontendPrefix}/"))
                {
                    path = path.Substring(FunctionHandler.HttpRouteFrontendPrefix.Length + 1);
                }
                //
                // transform wildcard names
                //
                var level = 0;
                var sb = new StringBuilder();
                for (int i = 0; i < path.Length; i++)
                {
                    if (path[i] == '}') level--;
                    if (level == 0)
                    {
                        sb.Append(path[i]);
                    }
                    if (path[i] == '{') level++;
                }

                return sb.ToString();
            }
            private string CreateFunctionName(string functionName)
            {
                int argCount = 0;
                var sb = new StringBuilder();
                sb.Append(Protocol.Substring(0, 1).ToUpper());
                sb.Append(Protocol.Substring(1).ToLower());
                int level = 0;
                foreach (var c in functionName)
                {
                    switch (c)
                    {
                        case '{':
                            sb.Append($"arg{argCount++}");
                            level++;
                            break;
                        case '}':
                            level--;
                            break;
                        case '.':
                        case '/':
                            sb.Append('_');
                            break;
                        default:
                            if(level == 0)
                            {
                                sb.Append(c);
                            }
                            break;
                    }
                }
                return sb.ToString();
            }

            public IAzFunctionHandler FunctionHandler { get; }
            public string Protocol { get; }
            public string Path { get; }
            public string FunctionName { get; }
        }

        private class HttpFunctionDef : FunctionDef
        {
            public HttpFunctionDef(IAzFunctionHandler functionHandler, string protocol, string openApiPath, string path) : base(functionHandler, protocol, openApiPath, path) { }

            public string Method { get; set; }
            public string AuthLevel { get; set; }
        }

        private class QueueFunctionDef : FunctionDef
        {
            public QueueFunctionDef(IAzFunctionHandler functionHandler, string protocol, string openApiPath, string path) : base(functionHandler, protocol, openApiPath, path) { }
            public string QueueName { get; set; }
            public string Connection { get; set; }
            public string QueueType { get; set; }
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="configuration"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="configurationStore"></param>
        /// <param name="contentHandler"></param>
        /// <param name="functionHandler"></param>
        public SolidRpcHostAzFunctions(
            ILogger<SolidRpcHost> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            IMethodBinderStore methodBinderStore,
            ISolidProxyConfigurationStore configurationStore,
            ISolidRpcContentHandler contentHandler,
            IAzFunctionHandler functionHandler)
            : base(logger, serviceProvider, configuration)
        {
            MethodBinderStore = methodBinderStore;
            ContentHandler = contentHandler;
            FunctionHandler = functionHandler;
            ConfigurationStore = configurationStore;

            var instanceid = configuration["WEBSITE_INSTANCE_ID"];
            if (!string.IsNullOrEmpty(instanceid))
            {
                HttpCookies = new Dictionary<string, string>()
                {
                    { "ARRAffinity", instanceid }
                };
            }
        }

        private IMethodBinderStore MethodBinderStore { get; }
        private ISolidRpcContentHandler ContentHandler { get; }
        private IAzFunctionHandler FunctionHandler { get; }
        private ISolidProxyConfigurationStore ConfigurationStore { get; }

        /// <summary>
        /// Perfomes the setup.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task IsAlive(CancellationToken cancellationToken = default(CancellationToken))
        {
            var functionDefs = MethodBinderStore.MethodBinders
                .SelectMany(o => o.MethodBindings)
                .Where(o => o.IsLocal)
                .Where(o => o.IsEnabled)
                .SelectMany(o => GetFunctionDefinitions(o))
                .ToList();

            var startTime = DateTime.Now;
            var modified = await WriteHttpFunctionsAsync(functionDefs,cancellationToken);

            //
            // get the static routes
            //
            var staticRoutes = (await ContentHandler.GetPathMappingsAsync(false, cancellationToken)).ToList();
            var wildcardRoutes = staticRoutes.Where(o => o.Name.EndsWith("*")).Where(o => o.Value.EndsWith("*")).ToList();
            staticRoutes.RemoveAll(o => wildcardRoutes.Any(o2 => o.Name == o2.Name));
            staticRoutes.AddRange(wildcardRoutes.Select(o => new NameValuePair() {
                Name = o.Name.Replace("*", "{arg}"),
                Value = o.Value.Replace("*", "{arg}")
            }));

            //
            // get the redirects
            //
            var redirects = (await ContentHandler.GetPathMappingsAsync(true, cancellationToken)).ToList();

            FunctionHandler.SyncProxiesFile(
                staticRoutes.ToDictionary(o => o.Name, o => o.Value),
                redirects.ToDictionary(o => o.Name, o => o.Value)
                );

            if (modified)
            {
                FunctionHandler.TriggerRestart();
            }
        }

        private IEnumerable<FunctionDef> GetFunctionDefinitions(IMethodBinding mb)
        {
            var config = ConfigurationStore.ProxyConfigurations
                .SelectMany(o => o.InvocationConfigurations)
                .Where(o => o.MethodInfo == mb.MethodInfo)
                .Where(o => o.IsAdviceConfigured<ISolidRpcOpenApiConfig>())
                .Where(o => o.ConfigureAdvice<ISolidRpcOpenApiConfig>().Enabled)
                .Single();
            if(!config.IsAdviceConfigured<ISolidAzureFunctionConfig>())
            {
                throw new Exception($"The method {mb.MethodInfo.DeclaringType.FullName}.{mb.MethodInfo.Name} does not have a configuration for ISolidAzureFunctionConfig.");
            }

            var functions = new List<FunctionDef>();
            var openApiConfig = config.ConfigureAdvice<ISolidRpcOpenApiConfig>();
            var azConfig = config.ConfigureAdvice<ISolidAzureFunctionConfig>();
            var httpTransport = openApiConfig.HttpTransport;
            if (httpTransport != null)
            {
                //
                // handle auth level
                //
                var authLevel = azConfig.HttpAuthLevel;
                if (string.IsNullOrEmpty(authLevel))
                {
                    throw new Exception($"AuthLevel not set for {mb.MethodInfo.DeclaringType.FullName}.{mb.MethodInfo.Name}");
                }

                var functionPath = openApiConfig.HttpTransport.OperationAddress.LocalPath;
                functions.Add(new HttpFunctionDef(FunctionHandler, "http", mb.LocalPath, httpTransport.OperationAddress.LocalPath)
                {
                    Method = mb.Method.ToLower(),
                    AuthLevel = authLevel
                });
            }
            if (openApiConfig.QueueTransport != null)
            {
                var queueName = openApiConfig.QueueTransport.QueueName;
                var connection = openApiConfig.QueueTransport.ConnectionName;
                var inboundHandler = openApiConfig.QueueTransport.InboundHandler;
                var queueType = openApiConfig.QueueTransport.QueueType;

                if(string.Equals(inboundHandler, "azfunctions", StringComparison.CurrentCultureIgnoreCase))
                {
                    functions.Add(new QueueFunctionDef(FunctionHandler, "queue", mb.LocalPath, mb.LocalPath)
                    {
                        QueueName = queueName,
                        Connection = connection,
                        QueueType = queueType
                    });
                }
            }
            return functions;
        }

        private async Task<bool> WriteHttpFunctionsAsync(List<FunctionDef> functionDefs, CancellationToken cancellationToken)
        {
            var functionTypes = new Type[]
            {
                typeof(IAzHttpFunction),
                typeof(IAzSvcBusFunction),
                typeof(IAzQueueFunction),
            };
            var existingFunctions = FunctionHandler.GetFunctions()
                .Where(f => functionTypes.Any(ft => ft.IsAssignableFrom(f.GetType()))).ToList();
            var touchedFunctions = new List<IAzFunction>();
            var functionNames = new HashSet<string>(functionDefs.Select(o => o.FunctionName));
            var modified = false;

            //
            // remove functions that are not available any more
            //
            existingFunctions.Where(o => !functionNames.Contains(o.Name))
                .Where(o => o.GeneratedBy?.StartsWith($"{typeof(AzTimerFunction).Assembly.GetName().Name}") ?? false)
                .ToList()
                .ForEach(o =>
                {
                    o.Delete();
                });

            //
            // handle http functions
            //
            var httpFunctionDefs = functionDefs.OfType<HttpFunctionDef>().ToList();
            var httpPaths = httpFunctionDefs.Select(o => o.Path).Distinct().ToList();
            foreach (var path in httpPaths)
            {
                var functionName = httpFunctionDefs.Where(o => o.Path == path).Select(o => o.FunctionName).Single();
                var methods = httpFunctionDefs.Where(o => o.Path == path).Select(o => o.Method).OrderBy(o => o).ToArray();
                var authLevel = httpFunctionDefs.Where(o => o.Path == path).Select(o => o.AuthLevel).Single();

                var httpFunction = FunctionHandler.GetOrCreateFunction<IAzHttpFunction>(functionName);
                httpFunction.AuthLevel = authLevel;
                httpFunction.Route = FunctionHandler.CreateRoute(path);
                httpFunction.Methods = methods;
                touchedFunctions.Add(httpFunction);
            }

            //
            // handle queue functions
            //
            var tasks = new List<Task>();
            var queueFunctionDefs = functionDefs.OfType<QueueFunctionDef>().ToList();
            foreach(var queueFunctionDef in queueFunctionDefs)
            {
                var functionName = queueFunctionDef.FunctionName;
                var queueType = queueFunctionDef.QueueType;

                if (queueType.Equals("azsvcbus", StringComparison.InvariantCultureIgnoreCase))
                {
                    var queueFunction = FunctionHandler.GetOrCreateFunction<IAzSvcBusFunction>(functionName);
                    queueFunction.QueueName = queueFunctionDef.QueueName;
                    queueFunction.Connection = queueFunctionDef.Connection;
                    touchedFunctions.Add(queueFunction);
                }
                else if(queueType.Equals("azqueue", StringComparison.InvariantCultureIgnoreCase))
                {
                    var queueFunction = FunctionHandler.GetOrCreateFunction<IAzQueueFunction>(functionName);
                    queueFunction.QueueName = queueFunctionDef.QueueName;
                    queueFunction.Connection = queueFunctionDef.Connection;
                    touchedFunctions.Add(queueFunction);
                }
                else
                {
                    throw new Exception("Cannot handle queue type:" + queueType);
                }
            }
            await Task.WhenAll(tasks);

            // write all touched functions
            touchedFunctions.ForEach(o => modified = o.Save() || modified);

            return modified;
        }
    }
}
