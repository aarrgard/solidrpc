using Microsoft.Extensions.DependencyInjection;
using SolidProxy.Core.Configuration.Builder;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzFunctions;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.OpenApi.SwaggerUI.Services;
using SolidRpc.Test.Petstore.AzFunctionsV2;
using System;
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
            services.AddSolidRpcSwaggerUI(o => o.DefaultOpenApiSpec = "SolidRpc.Security", ConfigureAzureFunction);
            services.AddSolidRpcSecurityFrontend((sp, c) => { }, ConfigureAzureFunction);
            services.AddSolidRpcSecurityBackend((sp, c) => {
                c.OidcClientId = Guid.NewGuid().ToString();
                c.OidcClientSecret = Guid.NewGuid().ToString();
            }, ConfigureAzureFunction);
            //services.AddPetstore();
            //services.AddSolidRpcSecurityBackend();
        }

        protected override bool ConfigureAzureFunction(ISolidRpcOpenApiConfig c)
        {
            //
            // enable anonyous access to the swagger methods and static content.
            //
            if (c.Methods.First().DeclaringType.Assembly == typeof(ISwaggerUI).Assembly)
            {
                c.GetAdviceConfig<ISolidAzureFunctionConfig>().AuthLevel = "anonymous";
                return true;
            }
            if (c.Methods.First().DeclaringType == typeof(ISolidRpcContentHandler))
            {
                c.GetAdviceConfig<ISolidAzureFunctionConfig>().AuthLevel = "anonymous";
                return true;
            }

            //
            // disable all the methods in the ISolidRpcHost interface
            //
            if (c.Methods.First().DeclaringType == typeof(ISolidRpcHost))
            {
                return false;
            }
            return base.ConfigureAzureFunction(c);
        }
    }
}
