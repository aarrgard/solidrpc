using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SolidProxy.Core.Configuration.Runtime;
using SolidRpc.Abstractions;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AspNetCore.Services;
using SolidRpc.OpenApi.AzFunctions.Functions;
using SolidRpc.OpenApi.AzFunctions.Services;
using SolidRpc.OpenApi.AzFunctionsV2Extension.Services;
using System;

[assembly: SolidRpcService(typeof(ISolidRpcHost), typeof(SolidRpcHostAzFunctionsV1))]
namespace SolidRpc.OpenApi.AzFunctionsV2Extension.Services
{
    /// <summary>
    /// Represents a rpc host in the azure functions environment
    /// </summary>
    public class SolidRpcHostAzFunctionsV1 : SolidRpcHostAzFunctions
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="configuration"></param>
        /// <param name="methodBinderStore"></param>
        /// <param name="configurationStore"></param>
        /// <param name="contentHandler"></param>
        /// <param name="functionHandler"></param>
        public SolidRpcHostAzFunctionsV1 (
            ILogger<SolidRpcHost> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration, 
            IMethodBinderStore methodBinderStore, 
            ISolidProxyConfigurationStore configurationStore, 
            ISolidRpcContentHandler contentHandler, 
            IAzFunctionHandler functionHandler) : 
            base(logger,
                serviceProvider,
                configuration, 
                methodBinderStore, 
                configurationStore, 
                contentHandler, 
                functionHandler)
        {
        }
    }
}
