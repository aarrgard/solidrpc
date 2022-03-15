using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
using System;
using System.Linq;
using System.Threading;

[assembly: SolidRpcServiceCollection(typeof(StartupServices))]

namespace SolidRpc.Test.Petstore.AzFunctionsV2
{
    public class StartupServices : StartupSolidRpcServices
    {
        public override void ConfigureServices(IServiceCollection services, Func<ISolidRpcOpenApiConfig, bool> configurator = null)
        {
            var settings = services.GetSolidRpcService<IConfiguration>();
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();

            services.AddSolidRpcOidcTestImpl();
            base.ConfigureServices(services, c => ConfigureAzureFunction(services, c));

            services.AddSolidRpcApplicationInsights(OpenApi.ApplicationInsights.LogSettings.ErrorScopes, "MS_FunctionInvocationId");

            services.AddSolidRpcOAuth2(conf =>
            {
                conf.AddDefaultScopes("authorization_code", new[] { "openid", "solidrpc", "offline_access" });
            });
            services.AddSolidRpcAzTableQueue("AzureWebJobsStorage", "azfunctions", ConfigureAzureFunction);

            services.AddSolidRpcSwaggerUI(o => {
                o.OAuthClientId = "swagger-ui";
                o.OAuthClientSecret = "swagger-ui";
            }, c => ConfigureAzureFunction(services, c));
            services.AddSolidRpcNode(c => ConfigureAzureFunction(services, c));
            //services.AddSolidRpcRateLimit(new Uri("https://eo-prd-ratelimit-func.azurewebsites.net/front/SolidRpc/Abstractions/"));
            //services.AddSolidRpcRateLimit();
            //services.AddSolidRpcRateLimitTableStorage(ConfigureAzureFunction);
            //services.AddVitec(ConfigureAzureFunction);
            services.AddSolidRpcOAuth2Local(GetOAuth2Issuer(services), conf => { conf.CreateSigningKey(); });
            //services.AddAzFunctionTimer<ISolidRpcHost>(o => o.GetHostId(CancellationToken.None), "0 * * * * *");
            services.AddAzFunctionTimer<IAzTableQueue>(o => o.DoScheduledScanAsync(CancellationToken.None), "0 * * * * *");
            //services.AddAzFunctionTimer<ITestInterfaceDel>(o => o.RunNodeService(CancellationToken.None), "0 * * * * *");

            services.GetSolidRpcContentStore().AddMapping("/A*", async sp =>
            {
                return await sp.GetRequiredService<IInvoker<ISolidRpcContentHandler>>().GetUriAsync(o => o.GetContent("A*", CancellationToken.None));
            });

            services.GetSolidRpcContentStore().AddContent(typeof(SwaggerUI).Assembly, "www", "/images");
            services.GetSolidRpcContentStore().AddMapping("/", async sp =>
            {
                return await sp.GetRequiredService<IInvoker<ISwaggerUI>>().GetUriAsync(o => o.GetIndexHtml(true, CancellationToken.None));
            }, true);
        }

        protected bool ConfigureAzureFunction(IServiceCollection services, ISolidRpcOpenApiConfig conf)
        {

            conf.SetOAuth2ClientSecurity(GetOAuth2Issuer(services), "swagger-ui", "swagger-ui");

            var method = conf.Methods.First();
            if (method.DeclaringType == typeof(ISwaggerUI))
            {
                //conf.DisableSecurity();
                //return true;
                switch (method.Name)
                {
                    case nameof(ISwaggerUI.GetOauth2RedirectHtml):
                        conf.DisableSecurity();
                        break;
                    default:
                        conf.GetAdviceConfig<ISecurityOAuth2Config>().RedirectUnauthorizedIdentity = true;
                        break;
                }
            }
            if (method.DeclaringType == typeof(ISolidRpcContentHandler))
            {
                conf.DisableSecurity();
                return true;
            }
            if (method.DeclaringType == typeof(ISolidRpcOAuth2))
            {
                conf.DisableSecurity();
                return true;
            }
            if (method.DeclaringType == typeof(ISolidRpcOidc))
            {
                conf.DisableSecurity();
                return true;
            }
            if (method.DeclaringType == typeof(IAzTableQueue))
            {
                switch (method.Name)
                {
                    case nameof(IAzTableQueue.ProcessTestMessage):
                        conf.SetProxyTransportType<IAzTableTransport>();
                        conf.SetInvokerTransport<IHttpTransport, IAzTableTransport>();
                        var t = conf.ConfigureTransport<IAzTableTransport>().SetConnectionName("AzureWebJobsStorage");
                        t.MessagePriority = 3;
                        break;
                }
            }
            var res = base.ConfigureAzureFunction(conf);
            return true;
        }

        private string GetOAuth2Issuer(IServiceCollection services)
        {

            var baseAddress = services.GetSolidRpcService<IMethodAddressTransformer>().BaseAddress.ToString();
            if (baseAddress.EndsWith("/")) baseAddress = baseAddress.Substring(0, baseAddress.Length - 1);
            var oauth2Issuer = $"{baseAddress}/SolidRpc/Abstractions";
            return oauth2Issuer;
        }
    }
}
