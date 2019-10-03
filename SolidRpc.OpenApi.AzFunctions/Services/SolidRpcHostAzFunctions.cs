using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
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
        private static bool s_restartPending = false;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="functionHandler"></param>
        public SolidRpcHostAzFunctions(
            ILogger<SolidRpcHost> logger, 
            IConfiguration configuration,
            IMethodBinderStore methodBinderStore,
            ISolidRpcContentHandler contentHandler,
            IAzFunctionHandler functionHandler)
            : base(logger, configuration)
        {
            s_restartPending = false;
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            ContentHandler = contentHandler;
            FunctionHandler = functionHandler;
        }

        private ILogger Logger { get; }
        private IMethodBinderStore MethodBinderStore { get; }
        private ISolidRpcContentHandler ContentHandler { get; }
        private IAzFunctionHandler FunctionHandler { get; }

        /// <summary>
        /// Perfomes the setup.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task IsAlive(CancellationToken cancellationToken = default(CancellationToken))
        {
            var paths = MethodBinderStore.MethodBinders
                .SelectMany(o => o.MethodBindings)
                .Where(o => o.IsLocal)
                .Select(o => new {
                    Path = FixupPath(o.Address.LocalPath),
                    Method = o.Method.ToLower()
                }).GroupBy(o => o.Path)
                .ToList();

            var startTime = DateTime.Now;
            var pathsAndMethods = paths.ToDictionary(o => o.Key, o => o.Select(o2 => o2.Method).ToList());
            WriteHttpFunctions(FunctionHandler.DevDir, pathsAndMethods);
            var modified = WriteHttpFunctions(FunctionHandler.BaseDir, pathsAndMethods);

            var staticRoutes = await ContentHandler.GetPathMappingsAsync(cancellationToken); 
            FunctionHandler.SyncProxiesFile(staticRoutes.ToDictionary(o => o.Name, o => o.Value));

            if (modified)
            {
                FunctionHandler.TriggerRestart();
            }
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

        private bool WriteHttpFunctions(DirectoryInfo baseDir, Dictionary<string, List<string>> pathsAndMethods)
        {
            var paths = pathsAndMethods.Keys.ToList();
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
                httpFunction.AuthLevel = "anonymous";
                httpFunction.Route = FunctionHandler.CreateRoute(path);
                httpFunction.Methods = pathsAndMethods[path].OrderBy(o => o).ToArray();
                if (httpFunction.Save())
                {
                    Logger.LogInformation($"Wrote new function.json for {functionName}.");
                    ScheduleRestart();
                }
            }
            return modified;
        }

        private void ScheduleRestart()
        {
            Task.Run(async () =>
            {
                s_restartPending = true;
                await Task.Delay(2000);
                if (s_restartPending)
                {
                    FunctionHandler.TriggerRestart();
                }
            });
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
