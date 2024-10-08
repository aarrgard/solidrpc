using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.OpenApi.AzFunctionsV4Extension;
using System;
using System.Linq;

[assembly: FunctionsStartup(typeof(Startup))]
namespace SolidRpc.OpenApi.AzFunctionsV4Extension
{
    public class Startup : FunctionsStartup
    {
 
        public override void Configure(IFunctionsHostBuilder builder)
        {
            AppDomain.CurrentDomain.GetAssemblies()
               .SelectMany(o => o.GetCustomAttributes(true).OfType<SolidRpcServiceCollectionAttribute>())
               .ToList();
            SolidRpcServiceCollectionAttribute.ConfigureServices(builder.Services);
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            base.ConfigureAppConfiguration(builder);
            builder.ConfigurationBuilder.AddEnvironmentVariables();
        }
    }
}
