using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AspNetCore.Services;
using SolidRpc.OpenApi.AzFunctions.Functions;
using SolidRpc.OpenApi.AzFunctions.Services;
using SolidRpc.OpenApi.AzFunctionsWorker.Services;
using System;

[assembly: SolidRpcService(typeof(ISolidRpcHost), typeof(SolidRpcHostAzWorker))]
namespace SolidRpc.OpenApi.AzFunctionsWorker.Services
{
    /// <summary>
    /// The solid rpc host in an azure functions environment
    /// </summary>
    public class SolidRpcHostAzWorker : SolidRpcHostAzFunctions
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="configuration"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="configurationStore"></param>
        /// <param name="timerStore"></param>
        /// <param name="functionHandler"></param>
        public SolidRpcHostAzWorker(
            ILogger<SolidRpcHost> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration, 
            IMethodBinderStore methodBinderStore, 
            ISolidProxyConfigurationStore configurationStore, 
            ITimerStore timerStore, 
            IAzFunctionHandler functionHandler) : 
            base(logger,
                serviceProvider,
                configuration, 
                methodBinderStore, 
                configurationStore,
                timerStore, 
                functionHandler)
        {
        }

        protected override AzFunctionEmitSettings EmitSettings { get; set; } = new AzFunctionEmitSettings()
        {
            NameAttribute = "Function",
            Usings = @"using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;",
            HttpRequestClass = "HttpRequest",
            HttpResponseClass = "IActionResult"
        };
    }
}
