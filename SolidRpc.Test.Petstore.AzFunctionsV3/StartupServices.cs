using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzFunctions;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.OpenApi.SwaggerUI.Services;
using SolidRpc.Test.Petstore.AzFunctionsV2;
using System.Linq;
using System.Threading;

[assembly: SolidRpcServiceCollection(typeof(StartupServices))]

namespace SolidRpc.Test.Petstore.AzFunctionsV2
{
    public class StartupServices : StartupSolidRpcServices
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            base.ConfigureServices(services);
            services.AddSolidRpcSwaggerUI(o => { }, ConfigureAzureFunction);
            //services.AddVitec(ConfigureAzureFunction);
            //services.AddSolidRpcSecurityBackend();
            services.AddAzFunctionTimer<ISolidRpcHost>(o => o.GetHostId(CancellationToken.None), "0 * * * * *");
        }

        protected override bool ConfigureAzureFunction(ISolidRpcOpenApiConfig c)
        {
            //
            // enable anonyous access to the swagger methods and static content.
            //
            var azConfig = c.GetAdviceConfig<ISolidAzureFunctionConfig>();

            if (!azConfig.Protocols.Any(o => o == "http")) azConfig.Protocols.Add("http");
            if (!azConfig.Protocols.Any(o => o == "queue")) azConfig.Protocols.Add("queue");

            if (c.Methods.First().DeclaringType.Assembly == typeof(ISwaggerUI).Assembly)
            {
                azConfig.HttpAuthLevel = "anonymous";
                return true;
            }
            if (c.Methods.First().DeclaringType == typeof(ISolidRpcContentHandler))
            {
                azConfig.HttpAuthLevel = "anonymous";
                return true;
            }

            return base.ConfigureAzureFunction(c);
        }
    }
}
