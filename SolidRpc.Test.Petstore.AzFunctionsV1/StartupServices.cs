using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.Test.Petstore.AzFunctions;
using SolidRpc.Test.Petstore.AzFunctionsV1;
using System;
using System.Reflection;

[assembly: SolidRpcServiceCollection(typeof(StartupServices))]

namespace SolidRpc.Test.Petstore.AzFunctionsV1
{
    public class StartupServices : StartupSolidRpcServices
    {
        static StartupServices()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var an = new AssemblyName(args.Name);
            if(an.Name == "Newtonsoft.Json")
            {
                return typeof(JsonConvert).Assembly;
            }
            return null;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            base.ConfigureServices(services);
            var service = services.BuildServiceProvider().GetRequiredService<ISolidRpcContentHandler>();
            services.AddSolidRpcSwaggerUI();
            services.AddPetstore();
            services.AddSolidRpcSecurityGoogle((sp, conf) => { sp.ConfigureOptions(conf); });
        }
    }
}
