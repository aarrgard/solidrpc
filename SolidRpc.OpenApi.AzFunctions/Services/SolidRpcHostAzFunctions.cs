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
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.OpenApi.Transport.Impl;
using SolidRpc.Abstractions.Services;
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
            public FunctionDef(IAzFunctionHandler functionHandler, string protocol, string path)
            {
                FunctionHandler = functionHandler;
                Protocol = protocol;
                Path = FixupPath(path);
                FunctionName = CreateFunctionName();
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
            private string CreateFunctionName()
            {
                int argCount = 0;
                var sb = new StringBuilder();
                sb.Append(Protocol.Substring(0, 1).ToUpper());
                sb.Append(Protocol.Substring(1).ToLower());
                sb.Append("_");
                foreach (var c in Path)
                {
                    switch (c)
                    {
                        case '}':
                            break;
                        case '.':
                        case '/':
                            sb.Append('_');
                            break;
                        case '{':
                            sb.Append($"arg{argCount++}");
                            break;
                        default:
                            sb.Append(c);
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
            public HttpFunctionDef(IAzFunctionHandler functionHandler, string protocol, string path) : base(functionHandler,protocol, path) { }

            public string Method { get; set; }
            public string AuthLevel { get; set; }
        }

        private class QueueFunctionDef : FunctionDef
        {
            public QueueFunctionDef(IAzFunctionHandler functionHandler, string protocol, string path) : base(functionHandler, protocol, path) { }
            public string QueueName { get; set; }
            public string Connection { get; set; }
            public string InboundHandler { get; set; }
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="configurationStore"></param>
        /// <param name="contentHandler"></param>
        /// <param name="functionHandler"></param>
        public SolidRpcHostAzFunctions(
            ILogger<SolidRpcHost> logger, 
            IConfiguration configuration,
            IMethodBinderStore methodBinderStore,
            ISolidProxyConfigurationStore configurationStore,
            ISolidRpcContentHandler contentHandler,
            IAzFunctionHandler functionHandler)
            : base(logger, configuration)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            ContentHandler = contentHandler;
            FunctionHandler = functionHandler;
            ConfigurationStore = configurationStore;
        }

        private ILogger Logger { get; }
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

            var staticRoutes = await ContentHandler.GetPathMappingsAsync(cancellationToken); 
            FunctionHandler.SyncProxiesFile(staticRoutes.ToDictionary(o => o.Name, o => o.Value));

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
            if (openApiConfig.HttpTransport != null)
            {
                //
                // handle auth level
                //
                var authLevel = azConfig.HttpAuthLevel;
                if (string.IsNullOrEmpty(authLevel))
                {
                    throw new Exception($"AuthLevel not set for {mb.MethodInfo.DeclaringType.FullName}.{mb.MethodInfo.Name}");
                }

                functions.Add(new HttpFunctionDef(FunctionHandler, "http", mb.Address.LocalPath)
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

                if (string.IsNullOrEmpty(connection))
                {
                    connection = "SolidRpcQueueConnection";
                }

                if(new[] { "azqueue", "azsvcbus"}.Contains(inboundHandler))
                {
                    functions.Add(new QueueFunctionDef(FunctionHandler, "queue", mb.Address.LocalPath)
                    {
                        QueueName = queueName,
                        Connection = connection,
                        InboundHandler = inboundHandler
                    });
                }
            }
            return functions;
        }

        private async Task<bool> WriteHttpFunctionsAsync(List<FunctionDef> functionDefs, CancellationToken cancellationToken)
        {
            var existingFunctions = FunctionHandler.GetFunctions().OfType<IAzHttpFunction>().ToList();
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
                var inboundHandler = queueFunctionDef.InboundHandler;
                if (inboundHandler.Equals("azqueue", StringComparison.InvariantCultureIgnoreCase))
                {
                    var queueFunction = FunctionHandler.GetOrCreateFunction<IAzQueueFunction>(functionName);
                    queueFunction.QueueName = queueFunctionDef.QueueName;
                    queueFunction.Connection = queueFunctionDef.Connection;
                    touchedFunctions.Add(queueFunction);
                }
                else if (inboundHandler.Equals("azsvcbus", StringComparison.InvariantCultureIgnoreCase))
                {
                    var queueFunction = FunctionHandler.GetOrCreateFunction<IAzSvcBusFunction>(functionName);
                    queueFunction.QueueName = queueFunctionDef.QueueName;
                    queueFunction.Connection = queueFunctionDef.Connection;
                    touchedFunctions.Add(queueFunction);
                }
                else
                {
                    throw new Exception("Cannot handle inbound handler:" + inboundHandler);
                }
            }
            await Task.WhenAll(tasks);

            // write all touched functions
            touchedFunctions.ForEach(o => modified = o.Save() || modified);

            return modified;
        }

        protected abstract Task CheckQueueAsync(string connection, string queueName, CancellationToken cancellationToken);
    }
}
