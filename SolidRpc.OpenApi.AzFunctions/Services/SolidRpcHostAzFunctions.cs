using System;
using System.Collections.Generic;
using System.IO;
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
            : base(logger, serviceProvider)
        {
            MethodBinderStore = methodBinderStore;
            FunctionHandler = functionHandler;
            ConfigurationStore = configurationStore;
            WriteSemaphore = new SemaphoreSlim(1);

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
        private IAzFunctionHandler FunctionHandler { get; }
        private ISolidProxyConfigurationStore ConfigurationStore { get; }
        private SemaphoreSlim WriteSemaphore { get; }

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
            await WriteAzFunctionsAsync(functionDefs,cancellationToken);
        }

        private IEnumerable<FunctionDef> GetFunctionDefinitions(IMethodBinding mb)
        {
            var config = ConfigurationStore.ProxyConfigurations
                .SelectMany(o => o.InvocationConfigurations)
                .Where(o => o.MethodInfo == mb.MethodInfo)
                .Where(o => o.IsAdviceConfigured<ISolidRpcOpenApiConfig>())
                .Where(o => o.ConfigureAdvice<ISolidRpcOpenApiConfig>().Enabled)
                .Single();

            var functions = new List<FunctionDef>();
            var openApiConfig = config.ConfigureAdvice<ISolidRpcOpenApiConfig>();
            var httpTransport = openApiConfig.GetTransports().OfType<IHttpTransport>().FirstOrDefault();
            if (httpTransport != null)
            {
                var functionPath = httpTransport.OperationAddress.LocalPath;
                functions.Add(new HttpFunctionDef(FunctionHandler, "http", mb.LocalPath, httpTransport.OperationAddress.LocalPath)
                {
                    Method = mb.Method.ToLower(),
                    AuthLevel = "anonymous"
                });
            }
            var queueTransport = openApiConfig.GetTransports().OfType<IQueueTransport>().FirstOrDefault();
            if (queueTransport != null)
            {
                var queueName = queueTransport.QueueName;
                var connection = queueTransport.ConnectionName;
                var inboundHandler = queueTransport.InboundHandler;
                var transportType = queueTransport.GetTransportType();

                if(string.Equals(inboundHandler, "azfunctions", StringComparison.CurrentCultureIgnoreCase))
                {
                    functions.Add(new QueueFunctionDef(FunctionHandler, "queue", mb.LocalPath, mb.LocalPath)
                    {
                        QueueName = queueName,
                        Connection = connection,
                        TransportType = transportType
                    });
                }
            }
            return functions;
        }

        private async Task WriteAzFunctionsAsync(List<FunctionDef> functionDefs, CancellationToken cancellationToken)
        {
            await WriteSemaphore.WaitAsync(cancellationToken);
            try
            {
                var functionTypes = new Type[]
                {
                    typeof(IAzHttpFunction),
                    typeof(IAzSvcBusFunction),
                    typeof(IAzQueueFunction),
                    typeof(IAzTimerFunction),
                };
                var existingFunctions = FunctionHandler.GetFunctions()
                    .Where(f => functionTypes.Any(ft => ft.IsAssignableFrom(f.GetType()))).ToList();
                var touchedFunctions = new List<IAzFunction>();
                var functionNames = new HashSet<string>(functionDefs.Select(o => o.FunctionName));

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
                {
                    var path = "/{*restOfPath}";
                    var functionName = "WildcardFunc";
                    var methods = (new[] { "get", "post", "options" }).OrderBy(o => o).ToArray();
                    var authLevel = "Anonymous";

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
                    var queueType = queueFunctionDef.TransportType;

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
                var solidRpcFunctionsCs = new StringBuilder();
                touchedFunctions.ForEach(o => o.Save());

                if(FunctionHandler.DevDir != null)
                {
                    var f = new FileInfo(Path.Combine(FunctionHandler.DevDir.FullName, "SolidRpcFunctions.cs"));
                    using (var fs = f.CreateText())
                    {
                        fs.WriteLine($@"
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    namespace SolidRpc.OpenApi.AzFunctions
    {{
    {string.Join(Environment.NewLine, FunctionHandler.FunctionCode.OrderBy(o => o.Key).Select(o => o.Value))}
    }}");
                    }
                }

            }
            finally
            {
                WriteSemaphore.Release();
            }
        }
    }
}
