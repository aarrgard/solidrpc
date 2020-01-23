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
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AspNetCore.Services;
using SolidRpc.OpenApi.AzFunctions.Functions;
using SolidRpc.OpenApi.AzFunctions.Functions.Impl;

namespace SolidRpc.OpenApi.AzFunctions.Services
{
    /// <summary>
    /// The setup class
    /// </summary>
    public class SolidRpcHostAzFunctions : SolidRpcHost
    {
        private class FunctionDef
        {
            public string Path { get; set; }
            public string Method { get; set; }
            public string AuthLevel { get; set; }
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
                .Select(o => new FunctionDef() {
                    Path = FixupPath(o.Address.LocalPath),
                    Method = o.Method.ToLower(),
                    AuthLevel = GetAuthLevel(o)
                }).ToList();

            var startTime = DateTime.Now;
            WriteHttpFunctions(FunctionHandler.DevDir, functionDefs);
            var modified = WriteHttpFunctions(FunctionHandler.BaseDir, functionDefs);

            var staticRoutes = await ContentHandler.GetPathMappingsAsync(cancellationToken); 
            FunctionHandler.SyncProxiesFile(staticRoutes.ToDictionary(o => o.Name, o => o.Value));

            if (modified)
            {
                FunctionHandler.TriggerRestart();
            }
        }

        private string GetAuthLevel(IMethodBinding mb)
        {
            var config = ConfigurationStore.ProxyConfigurations
                .SelectMany(o => o.InvocationConfigurations)
                .Where(o => o.MethodInfo == mb.MethodInfo)
                .Single();
            if(!config.IsAdviceConfigured<ISolidAzureFunctionConfig>())
            {
                throw new Exception($"The method {mb.MethodInfo.DeclaringType.FullName}.{mb.MethodInfo.Name} does not have a configuration for ISolidAzureFunctionConfig.");
            }
            var authLevel = config.ConfigureAdvice<ISolidAzureFunctionConfig>().AuthLevel;
            if(string.IsNullOrEmpty(authLevel))
            {
                throw new Exception($"AuthLevel not set for {mb.MethodInfo.DeclaringType.FullName}.{mb.MethodInfo.Name}");
            }
            return authLevel;
        }

        private string FixupPath(string path)
        {
            //
            // transform wildcard names
            //
            var level = 0;
            var sb = new StringBuilder();
            for(int i = 0; i < path.Length; i++)
            {
                if (path[i] == '}') level--;
                if(level == 0)
                {
                    sb.Append(path[i]);
                }
                if (path[i] == '{') level++;
            }

            path = sb.ToString();
            // remove frontend prefix
            if(path.StartsWith($"{FunctionHandler.HttpRouteFrontendPrefix}/"))
            {
                path = path.Substring(FunctionHandler.HttpRouteFrontendPrefix.Length + 1);
            }
            return path;
        }

        private bool WriteHttpFunctions(DirectoryInfo baseDir, IEnumerable<FunctionDef> functionDefs)
        {
            var paths = functionDefs.Select(o => o.Path).Distinct().ToList();
            var functionNames = paths.Select(o => CreateFunctionName(o)).ToList();
            var functions = FunctionHandler.GetFunctions(baseDir).ToList();
            var modified = false;

            //
            // remove functions that are not available any more
            //
            functions.Where(o => !functionNames.Contains(o.Name))
                .Where(o => o.GeneratedBy?.StartsWith($"{typeof(AzTimerFunction).Assembly.GetName().Name}") ?? false)
                .ToList()
                .ForEach(o =>
                {
                    o.Delete();
                });

            //
            // add / modify functions
            //
            for (int i = 0; i < paths.Count; i++)
            {
                var path = paths[i];
                var methods = functionDefs.Where(o => o.Path == path).Select(o => o.Method).OrderBy(o => o).ToArray();
                var authLevel = functionDefs.Where(o => o.Path == path).Select(o => o.AuthLevel).FirstOrDefault();
                var functionName = functionNames[i];
                var function = functions.Where(o => o.Name == functionName).FirstOrDefault();
                var httpFunction = function as IAzHttpFunction;
                if (httpFunction == null && function != null)
                {
                    function.Delete();
                }
                if (httpFunction == null)
                {
                    httpFunction = FunctionHandler.CreateHttpFunction(baseDir, functionName);
                }
                httpFunction.AuthLevel = authLevel;
                httpFunction.Route = FunctionHandler.CreateRoute(path);
                httpFunction.Methods = methods;
                if (httpFunction.Save())
                {
                    Logger.LogInformation($"Wrote new function.json for {functionName}.");
                    modified = true;
                }
            }
            return modified;
        }

        private string CreateFunctionName(string path)
        {
            int argCount = 0;
            var sb = new StringBuilder();
            sb.Append("Http_");
            foreach(var c in path)
            {
                switch(c)
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
    }
}
