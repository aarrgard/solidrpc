using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
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
                //builder.Services.AddSolidRpcBindings(typeof(IPet).Assembly, typeof(PetImpl).Assembly);
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
    }
}