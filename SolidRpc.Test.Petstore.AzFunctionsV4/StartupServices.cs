using Microsoft.Extensions.Configuration;
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
using SolidRpc.OpenApi.OAuth2.Services;
using SolidRpc.OpenApi.SwaggerUI.Services;
using SolidRpc.Test.Petstore.AzFunctionsV2;
using SolidRpc.Test.Petstore.AzFunctionsV4;
using System;
using System.Linq;
using System.Reflection;
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

            services.GetSolidRpcContentStore().AddPrefixRewrite("/front", "");
            services.GetSolidRpcContentStore().AddPrefixRewrite("/api", "");

            services.AddSolidRpcOidcTestImpl();
            base.ConfigureServices(services, c => ConfigureAzureFunction(services, c));

            services.AddSolidRpcApplicationInsights(OpenApi.ApplicationInsights.LogSettings.ErrorScopes, "MS_FunctionInvocationId");
            services.AddSolidRpcOAuth2(conf =>
            {
                conf.AddDefaultScopes("authorization_code", new[] { "openid", "solidrpc", "offline_access" });
            });
            services.AddSolidRpcAzTableQueue("AzureWebJobsStorage", "azfunctions", c => ConfigureAzureFunction(services, c));

            services.AddSolidRpcSwaggerUI(o => {
                o.OAuthClientId = SolidRpcOidcTestImpl.ClientId;
                o.OAuthClientSecret = SolidRpcOidcTestImpl.ClientSecret;
            }, c => ConfigureAzureFunction(services, c));
            services.AddSolidRpcNode(c => ConfigureAzureFunction(services, c));
            //services.AddSolidRpcRateLimit(new Uri("https://eo-prd-ratelimit-func.azurewebsites.net/front/SolidRpc/Abstractions/"));
            //services.AddSolidRpcRateLimit();
            //services.AddSolidRpcRateLimitTableStorage(ConfigureAzureFunction);
            //services.AddVitec(ConfigureAzureFunction);
            services.AddSolidRpcOAuth2Local(conf => { conf.CreateSigningKey(); });
            //services.AddAzFunctionTimer<ISolidRpcHost>(o => o.GetHostId(CancellationToken.None), "0 * * * * *");
            services.AddAzFunctionTimer<IAzTableQueue>(o => o.DoScheduledScanAsync(CancellationToken.None), "0 * * * * *");
            //services.AddAzFunctionTimer<ITestInterfaceDel>(o => o.RunNodeService(CancellationToken.None), "0 * * * * *");

            services.GetSolidRpcContentStore().AddMapping("/A*", async sp =>
            {
                return await sp.GetRequiredService<IInvoker<ISolidRpcContentHandler>>().GetUriAsync(o => o.GetContent("A*", CancellationToken.None));
            });

            //services.GetSolidRpcContentStore().AddContent(typeof(SwaggerUI).Assembly, "www", "/");
            //services.GetSolidRpcContentStore().SetNotFoundRewrite("/SolidRpc/OpenApi/SwaggerUI/Services/ISwaggerUI/GetIndexHtml");
            //services.GetSolidRpcContentStore().AddMapping("/", async sp =>
            //{
            //    return await sp.GetRequiredService<IInvoker<IHttpFunc>>().GetUriAsync(o => o.Https(null, CancellationToken.None));
            //});
            services.GetSolidRpcContentStore().AddMapping("/index.html", async sp =>
            {
                return await sp.GetRequiredService<IInvoker<IHttpFunc>>().GetUriAsync(o => o.Https(null, CancellationToken.None));
            });
            services.GetSolidRpcContentStore().AddPrefixRewrite("/Https/", "/SolidRpc/Test/Petstore/AzFunctionsV4/IHttpFunc/Https?ops=");

            //services.AddSolidRpcRemoteBindings<INotification>(c => ConfigureAzureFunction(services, c));

            var apiSpec = services.GetSolidRpcOpenApiParser().CreateSpecification(typeof(IHttpFunc)).WriteAsJsonString();
            services.AddSolidRpcBindings(typeof(IHttpFunc), typeof(HttpFuncImpl), c =>
            {
                c.OpenApiSpec = apiSpec;
                c.DisableSecurity();
                return true;
            });
        }

        protected bool ConfigureAzureFunction(IServiceCollection services, ISolidRpcOpenApiConfig conf)
        {

            //conf.SetOAuth2ClientSecurity(services.GetSolidRpcOAuth2LocalIssuer(), "swagger-ui", "swagger-ui");
            conf.SetOAuth2ClientSecurity(services.GetSolidRpcOAuth2LocalIssuer(), SolidRpcOidcTestImpl.ClientId, SolidRpcOidcTestImpl.ClientSecret);
            //conf.SetOAuth2ClientSecurity("https://identity.erikolsson.se", "eo-prd-o365-func", "1053f26b-80ce-443c-8529-6dfa2c5d23b6");
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
            //if (method.DeclaringType == typeof(INotification))
            //{
            //    ConfigureAzTableHandlerRemote(conf, method, method.DeclaringType, method.Name);
            //}
            var res = base.ConfigureAzureFunction(conf);
            return true;
        }

        private void ConfigureAzTableHandlerRemote(ISolidRpcOpenApiConfig config, MethodInfo method, Type type, string methodName)
        {
            if (method.DeclaringType != type) return;
            if (method.Name != methodName) return;
            config.ProxyTransportType = AzTableHandler.TransportType;
            config.SetInvokerTransport<IAzTableTransport, IHttpTransport>();
            config.ConfigureTransport<IAzTableTransport>().SetConnectionName("AzureWebJobsStorage");
        }

    }
}
