using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.OpenApi.AzFunctions;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.Test.Petstore.AzFunctionsV2;
using System;

[assembly: SolidRpcServiceCollection(typeof(StartupServices))]

namespace SolidRpc.Test.Petstore.AzFunctionsV2
{
    public class StartupServices : StartupSolidRpcServices
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            base.ConfigureServices(services);
            services.AddSolidRpcSwaggerUI(o => o.DefaultOpenApiSpec = "SolidRpc.Security");
            services.AddSolidRpcSecurityFrontend();
            services.AddSolidRpcSecurityBackend((sp, c) => {
                c.OidcClientId = Guid.NewGuid().ToString();
                c.OidcClientSecret = Guid.NewGuid().ToString();
            });
            //services.AddPetstore();
            //services.AddSolidRpcSecurityBackend();
        }
    }
}
