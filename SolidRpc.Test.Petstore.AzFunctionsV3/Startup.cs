using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SolidRpc.OpenApi.AzFunctions;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.OpenApi.AzFunctions.Functions.Impl;
using SolidRpc.OpenApi.AzFunctionsV4Extension;
using SolidRpc.OpenApi.Binder;
using System;
using System.Collections.Generic;
using System.IO;
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
            var context = builder.GetContext();
            var azFuncHandler = new AzFunctionHandler(new DirectoryInfo(context.ApplicationRootPath), typeof(HttpFunction).Assembly);
            builder.ConfigurationBuilder
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { ConfigurationMethodAddressTransformer.ConfigPathPrefix.First(), azFuncHandler.HttpRouteBackendPrefix },
                    { ConfigurationMethodAddressTransformer.ConfigPathRewrites.First(), $"/front:,/api:,{azFuncHandler.HttpRouteBackendPrefix}:" }
                })
                .AddEnvironmentVariables();
        }
    }
}
