using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.Test.Petstore.AzFunctions;
using SolidRpc.Test.Petstore.AzFunctionsV1;

[assembly: SolidRpcServiceCollection(typeof(StartupServices))]

namespace SolidRpc.Test.Petstore.AzFunctionsV1
{
    public class StartupServices : StartupSolidRpcServices
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            base.ConfigureServices(services);
            var service = services.BuildServiceProvider().GetRequiredService<ISolidRpcStaticContent>();
            services.AddSolidRpcSwaggerUI();
            services.AddPetstore();
        }
    }
}
