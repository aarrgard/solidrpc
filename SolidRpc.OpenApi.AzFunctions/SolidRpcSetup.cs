using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.OpenApi.AzFunctions.Functions;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Binder.Proxy;

namespace SolidRpc.OpenApi.AzFunctions
{
    /// <summary>
    /// The setup class
    /// </summary>
    public class SolidRpcSetup : ISolidRpcSetup
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="functionHandler"></param>
        public SolidRpcSetup(ILogger<SolidRpcSetup> logger, IMethodBinderStore methodBinderStore, IAzFunctionHandler functionHandler)
        {
            Logger = logger;
            MethodBinderStore = methodBinderStore;
            FunctionHandler = functionHandler;
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

        private string CreateFunctionName(IMethodInfo o)
        {
            return $"{o.MethodBinder.Assembly.GetName().Name}.{o.OperationId}"
                .Replace(".", "");
        }

        /// <summary>
        /// Perfomes the setup.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Setup(CancellationToken cancellationToken = default(CancellationToken))
        {
            var paths = MethodBinderStore.MethodBinders
                .SelectMany(o => o.MethodInfos)
                .Select(o => new { o.Path, Method = o.Method.ToLower() })
                .GroupBy(o => o.Path)
                .ToList();
            WriteHttpFunctions(paths.ToDictionary(o => o.Key, o => o.Select(o2 => o2.Method).ToList()));
            return Task.CompletedTask;
        }

        private void WriteHttpFunctions(Dictionary<string, List<string>> pathsAndMethods)
        {
            var paths = pathsAndMethods.Keys.ToList();
            var functionNames = paths.Select(o => CreateFunctionName(o)).ToList();
            var functions = FunctionHandler.Functions.ToList();

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
                httpFunction.Route = path;
                httpFunction.Methods = pathsAndMethods[path];
                httpFunction.Save();
            }
        }

        private string CreateFunctionName(string path)
        {
            return path.Replace("/", "")
                .Replace(".", "_")
                .Replace("{", "")
                .Replace("}", "");
        }
    }
}
