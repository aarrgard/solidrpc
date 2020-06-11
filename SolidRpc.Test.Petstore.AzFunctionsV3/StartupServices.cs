using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.OpenApi.Transport;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzFunctions;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.OpenApi.AzQueue.Services;
using SolidRpc.OpenApi.SwaggerUI.Services;
using SolidRpc.Test.Petstore.AzFunctionsV2;
using SolidRpc.Test.Petstore.AzFunctionsV3;
using System;
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

            services.AddAzTableQueue("AzureWebJobsStorage", "azfunctions", ConfigureAzureFunction);

            services.AddSolidRpcBindings(typeof(ITestInterface).Assembly, typeof(TestImplementation).Assembly, conf =>
            {
                conf.OpenApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();

                if(conf.Methods.First().Name == nameof(ITestInterface.MyFunc))
                {
                    conf.SetHttpTransport(InvocationStrategy.Forward);
                    conf.SetQueueTransport<AzTableHandler>(InvocationStrategy.Invoke, "AzureWebJobsStorage");
                }

                return ConfigureAzureFunction(conf);
            });
            services.AddSolidRpcServices(ConfigureAzureFunction);
            services.AddSolidRpcSwaggerUI(o => { }, ConfigureAzureFunction);
            //services.AddSolidRpcRateLimit(new Uri("https://eo-prd-ratelimit-func.azurewebsites.net/front/SolidRpc/Abstractions/"));
            //services.AddSolidRpcRateLimit();
            //services.AddSolidRpcRateLimitTableStorage(ConfigureAzureFunction);
            //services.AddVitec(ConfigureAzureFunction);
            //services.AddSolidRpcSecurityBackend();
            services.AddAzFunctionTimer<ISolidRpcHost>(o => o.GetHostId(CancellationToken.None), "0 * * * * *");
            services.AddAzFunctionTimer<IAzTableQueue>(o => o.DoScheduledScanAsync(CancellationToken.None), "0 * * * * *");

            services.GetSolidRpcContentStore().AddMapping("/A*", async sp =>
            {
                return await sp.GetRequiredService<IInvoker<ISolidRpcContentHandler>>().GetUriAsync(o => o.GetContent("A*", CancellationToken.None));
            });
            services.GetSolidRpcContentStore().AddMapping("/", async sp =>
            {
                return await sp.GetRequiredService<IInvoker<ISwaggerUI>>().GetUriAsync(o => o.GetIndexHtml(true, CancellationToken.None));
            }, true);
        }

        protected override bool ConfigureAzureFunction(ISolidRpcOpenApiConfig c)
        {
            //
            // enable anonyous access to the swagger methods and static content.
            //
            var azConfig = c.GetAdviceConfig<ISolidAzureFunctionConfig>();

            //c.GetAdviceConfig<ISolidRpcRateLimitConfig>().ResourceName = c.Methods.First().DeclaringType.Assembly.GetName().Name;

            //c.SetHttpTransport();
            //c.SetQueueTransport<QueueInvocationHandler>();
            //c.SetQueueTransportInboundHandler("azfunctions");

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

            var res = base.ConfigureAzureFunction(c);
            return true;
        }
    }
}
