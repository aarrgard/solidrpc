using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.OpenApi.AzFunctionsV2Extension;
using System;
using System.Linq;

[assembly: WebJobsStartup(typeof(Startup))]
namespace SolidRpc.OpenApi.AzFunctionsV2Extension
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(o => o.GetCustomAttributes(true).OfType<SolidRpcServiceCollectionAttribute>())
                .ToList();
            SolidRpcServiceCollectionAttribute.ConfigureServices(builder.Services);
        }
    }
}
