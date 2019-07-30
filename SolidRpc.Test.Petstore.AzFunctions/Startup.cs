using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.Test.Petstore.Impl;
using SolidRpc.Test.Petstore.Services;
using System;

[assembly: FunctionsStartup(typeof(MyNamespace.Startup))]

namespace MyNamespace
{
    public class Startup : SolidRpc.OpenApi.AzFunctions.Startup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            try
            {
                builder.Services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();

                builder.Services.AddSolidRpcBindings(typeof(IPet).Assembly, typeof(PetImpl).Assembly, GetBaseUrl);
                builder.Services.GetSolidRpcStaticContent().AddContent(typeof(PetImpl).Assembly, "www", "/");

                builder.Services.AddSolidRpcSwaggerUI(GetBaseUrl);

                base.Configure(builder);
            }
            catch (Exception e)
            {
                Log("Exception caught:" + e);
            }
            finally
            {
                Log("Configured");
            }
        }

        private Uri GetBaseUrl(IServiceProvider serviceProvider, Uri baseUri)
        {
            throw new NotImplementedException();
        }
    }
}