using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
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
using System.Threading;
using System.Threading.Tasks;

[assembly: SolidRpcService(typeof(ISolidRpcHost), typeof(SolidRpcHostAzFunctionsV2))]
namespace SolidRpc.OpenApi.AzFunctionsV2Extension.Services
{
    public class SolidRpcHostAzFunctionsV2 : SolidRpcHostAzFunctions
    {
        public SolidRpcHostAzFunctionsV2(
            ILogger<SolidRpcHost> logger, 
            IConfiguration configuration, 
            IMethodBinderStore methodBinderStore, 
            ISolidProxyConfigurationStore configurationStore, 
            ISolidRpcContentHandler contentHandler, 
            IAzFunctionHandler functionHandler) : 
            base(logger, 
                configuration, 
                methodBinderStore, 
                configurationStore, 
                contentHandler, 
                functionHandler)
        {
        }

        protected override async Task CheckQueueAsync(string connection, string queueName, CancellationToken cancellationToken)
        {
            var connectionString = Configuration[connection];
            if(string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"No connection string specified for connectin {connection}.");
            }
            var mgmt = new ManagementClient(connectionString);
            try
            {
                var queue = await mgmt.GetQueueAsync(queueName, cancellationToken);
            }
            catch (Exception e)
            {
                var queue = await mgmt.CreateQueueAsync(queueName, cancellationToken);
            }
        }
    }
}
