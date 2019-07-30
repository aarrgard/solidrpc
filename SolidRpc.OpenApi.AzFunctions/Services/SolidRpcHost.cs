﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.OpenApi.AzFunctions.Functions;
using SolidRpc.OpenApi.Binder;

namespace SolidRpc.OpenApi.AzFunctions.Services
{
    /// <summary>
    /// The setup class
    /// </summary>
    public class SolidRpcHost : ISolidRpcHost
    {
        private static bool s_restartPending = false;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="functionHandler"></param>
        /// <param name="contentTypeProvider"></param>
        public SolidRpcHost(
            ILogger<SolidRpcHost> logger, 
            IMethodBinderStore methodBinderStore,
            IAzFunctionHandler functionHandler,
            IContentTypeProvider contentTypeProvider)
        {
            s_restartPending = false;
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            FunctionHandler = functionHandler;
            ContentTypeProvider = contentTypeProvider;
        }

        private ILogger Logger { get; }

        /// <summary>
        /// The config store
        /// </summary>
        public IMethodBinderStore MethodBinderStore { get; }

        /// <summary>
        /// The function handler
        /// </summary>
        public IAzFunctionHandler FunctionHandler { get; }

        /// <summary>
        /// The content type provider.
        /// </summary>
        public IContentTypeProvider ContentTypeProvider { get; }

        private string CreateFunctionName(IMethodInfo o)
        {
            return $"{o.MethodBinder.Assembly.GetName().Name}.{o.OperationId}"
                .Replace(".", "");
        }

        /// <summary>
        /// Perfomes the setup.
        /// </summary>
        /// <param name="configurationHash"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CheckConfig(string configurationHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            var paths = MethodBinderStore.MethodBinders
                .SelectMany(o => o.MethodInfos)
                .Select(o => new {
                    Path = RemoveWildcardNames(o.Path),
                    Method = o.Method.ToLower()
                }).GroupBy(o => o.Path)
                .ToList();

            var startTime = DateTime.Now;
            var modified = WriteHttpFunctions(paths.ToDictionary(o => o.Key, o => o.Select(o2 => o2.Method).ToList()));

            FunctionHandler.SyncProxiesFile();

            if (modified)
            {
                FunctionHandler.TriggerRestart();
            }

            return Task.CompletedTask;
        }

        private async Task CheckRestarting(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);
            Logger.LogInformation("Host not restarting - writing changes to bin folder...");
            FunctionHandler.Functions
                .OfType<IAzTimerFunction>()
                .Where(o => o.ServiceType == typeof(ISolidRpcHost).FullName)
                .Single()
                .Save(true);
        }

        private string RemoveWildcardNames(string path)
        {
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
            return sb.ToString();
        }

        private bool WriteHttpFunctions(Dictionary<string, List<string>> pathsAndMethods)
        {
            var paths = pathsAndMethods.Keys.ToList();
            var functionNames = paths.Select(o => CreateFunctionName(o)).ToList();
            var functions = FunctionHandler.Functions.ToList();
            var modified = false;

            for(int i = 0; i < paths.Count; i++)
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
                    httpFunction = FunctionHandler.CreateHttpFunction(functionName);
                }
                httpFunction.AuthLevel = "anonymous";
                httpFunction.Route = path;
                httpFunction.Methods = pathsAndMethods[path];
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
            sb.Append("Http");
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
