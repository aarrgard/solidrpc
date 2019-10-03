using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.Test.Petstore.AzFunctions;
using SolidRpc.Test.Petstore.AzFunctionsV2;
using System.Linq;

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
            //services.AddPetstore();
            services.AddSolidRpcSecurityBackend();
        }
    }
}
