using Microsoft.Extensions.DependencyInjection;
using SolidProxy.Core.Configuration.Builder;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Proxy;
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
            services.AddSolidRpcSwaggerUI(o => o.DefaultOpenApiSpec = "SolidRpc.Security", ConfigureAzureFunction);
            services.AddSolidRpcSecurityFrontend((sp, c) => { }, ConfigureAzureFunction);
            services.AddSolidRpcSecurityBackend((sp, c) => {
                c.OidcClientId = Guid.NewGuid().ToString();
                c.OidcClientSecret = Guid.NewGuid().ToString();
            }, ConfigureAzureFunction);
            //services.AddPetstore();
            //services.AddSolidRpcSecurityBackend();
        }

        protected override void ConfigureAzureFunction(ISolidMethodConfigurationBuilder c)
        {
            if(c.MethodInfo.DeclaringType.Assembly == typeof(SolidRpc.OpenApi.SwaggerUI.Services.ISwaggerUI).Assembly)
            {
                c.ConfigureAdvice<ISolidAzureFunctionConfig>().AuthLevel = "anonymous";
            }
            else
            {
                base.ConfigureAzureFunction(c);
            }
        }
    }
}
