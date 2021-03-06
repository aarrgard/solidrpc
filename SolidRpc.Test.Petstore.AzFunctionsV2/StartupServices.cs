﻿using Microsoft.Extensions.DependencyInjection;
using SolidProxy.Core.Configuration.Builder;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AzFunctions;
using SolidRpc.OpenApi.AzFunctions.Bindings;
using SolidRpc.OpenApi.AzQueue.Invoker;
using SolidRpc.OpenApi.SwaggerUI.Services;
using SolidRpc.Test.Petstore.AzFunctionsV2;
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
            services.AddSolidRpcSwaggerUI(o => o.DefaultOpenApiSpec = "SolidRpc.Security", ConfigureAzureFunction);
            //services.AddSolidRpcSecurityFrontend((sp, c) => { }, ConfigureAzureFunction);
            //services.AddSolidRpcSecurityBackend((sp, c) => {
            //    c.OidcClientId = Guid.NewGuid().ToString();
            //    c.OidcClientSecret = Guid.NewGuid().ToString();
            //}, ConfigureAzureFunction);
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

            c.SetHttpTransport();
            c.SetQueueTransport<AzQueueHandler>();
            c.SetQueueTransportInboundHandler("azfunctions");

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
