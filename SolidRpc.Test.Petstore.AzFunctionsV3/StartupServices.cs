﻿using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Binder;
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
            var baseAddress = services.GetSolidRpcService<IMethodAddressTransformer>().BaseAddress;

            //services.AddSolidRpcApplicationInsights(OpenApi.ApplicationInsights.LogSettings.ErrorScopes, "MS_FunctionInvocationId");

            services.AddSolidRpcOAuth2();
            services.AddSolidRpcAzTableQueue("AzureWebJobsStorage", "azfunctions", ConfigureAzureFunction);

            services.AddSolidRpcBindings(typeof(ITestInterface).Assembly, typeof(TestImplementation).Assembly, conf =>
            {
                conf.OpenApiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();

                conf.SetOauth2Security(baseAddress.ToString(), "clientid", "secret");
                if(conf.Methods.First().Name == nameof(ITestInterface.MyFunc))
                {
                    conf.SetHttpTransport(InvocationStrategy.Forward);
                    conf.SetQueueTransport<AzTableHandler>(InvocationStrategy.Invoke, "AzureWebJobsStorage");
                }

                return ConfigureAzureFunction(conf);
            });
            services.AddSolidRpcServices(ConfigureAzureFunction);
            services.AddSolidRpcSwaggerUI(o => { 
                o.OAuthClientId = "testar";
                o.OAuthClientSecret = "secret";
            }, ConfigureAzureFunction);
            services.AddSolidRpcNpmGenerator(ConfigureAzureFunction);
            //services.AddSolidRpcRateLimit(new Uri("https://eo-prd-ratelimit-func.azurewebsites.net/front/SolidRpc/Abstractions/"));
            //services.AddSolidRpcRateLimit();
            //services.AddSolidRpcRateLimitTableStorage(ConfigureAzureFunction);
            //services.AddVitec(ConfigureAzureFunction);
            services.AddSolidRpcOAuth2Local(baseAddress.ToString(), conf => { conf.CreateSigningKey(); });
            services.AddSolidRpcSecurityBackend((sp, conf) => { }, ConfigureAzureFunction);
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

        protected override bool ConfigureAzureFunction(ISolidRpcOpenApiConfig conf)
        {
            //
            // enable anonyous access to the swagger methods and static content.
            //

            //c.GetAdviceConfig<ISolidRpcRateLimitConfig>().ResourceName = c.Methods.First().DeclaringType.Assembly.GetName().Name;

            //c.SetHttpTransport();
            //c.SetQueueTransport<QueueInvocationHandler>();
            //c.SetQueueTransportInboundHandler("azfunctions");

            var method = conf.Methods.First();
            if (method.DeclaringType.Assembly == typeof(ISwaggerUI).Assembly)
            {
                conf.DisableSecurity();
                return true;
            }
            if (method.DeclaringType == typeof(ISolidRpcContentHandler))
            {
                conf.DisableSecurity();
                return true;
            }
            if (method.DeclaringType == typeof(ISolidRpcHost))
            {
                conf.SetOauth2Security("https://identity.erikolsson.se", "test", "test");
                return true;
            }
            if (method.DeclaringType == typeof(IAzTableQueue))
            {
                switch (method.Name)
                {
                    case nameof(IAzTableQueue.ProcessTestMessage):
                        conf.ProxyTransportType = AzTableHandler.TransportType;
                        conf.SetHttpTransport(InvocationStrategy.Forward);
                        conf.SetQueueTransport<AzTableHandler>(InvocationStrategy.Invoke, "AzureWebJobsStorage");
                        break;
                }
            }
            var res = base.ConfigureAzureFunction(conf);
            return true;
        }
    }
}
