using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Features;
using System.Threading.Tasks;
using SolidRpc.Abstractions.OpenApi.Proxy;
using System.Linq;
using SolidRpc.OpenApi.SwaggerUI.Services;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Test.Petstore.Services;
using System.Threading;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.OAuth2.Services;
using SolidRpc.Test.Petstore.Impl;

namespace SolidRpc.Test.PetstoreWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSolidRpcSingletonServices();
            var oauth2Iss = services.GetSolidRpcOAuth2LocalIssuer();
            var oauthClientId = SolidRpcOidcTestImpl.ClientId;
            var oauthClientSecret = SolidRpcOidcTestImpl.ClientSecret;
            //var oauth2Iss = "https://identity.realalliance.se";
            //var oauthClientId = "**";
            //var oauthClientSecret = "**";

            services.AddLogging(o => {
                o.SetMinimumLevel(LogLevel.Trace);
                o.AddConsole();
            });
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            services.AddHttpClient();
            services.AddSolidRpcOidcTestImpl();
            services.AddSolidRpcServices(conf =>
            {
                if (conf.Methods.First().DeclaringType == typeof(ISolidRpcOAuth2))
                {
                    SolidRpcOidcTestImpl.ClientAllowedPaths = new[] { "/*" };
                    SolidRpcOidcTestImpl.UserAllowedPaths = new[] { "/*" };
                    conf.SetOAuth2ClientSecurity(oauth2Iss, oauthClientId, oauthClientSecret);
                    conf.DisableSecurity();
                }
                return true;
            });
            services.AddSolidRpcAzTableQueue("AzureWebJobsStorage", "generic");
            services.AddSolidRpcSwaggerUI(conf =>
            {
                conf.DefaultOpenApiSpec = "SolidRpc.OpenApi.SwaggerUI";
                conf.OAuthClientId = oauthClientId;
                conf.OAuthClientSecret = oauthClientSecret;
            }, conf =>
            {
                conf.DisableSecurity();
                if (conf.Methods.Any(o => o.Name == nameof(ISwaggerUI.GetIndexHtml)))
                {
                    conf.SetOAuth2Security(oauth2Iss);
                    conf.GetAdviceConfig<ISecurityOAuth2Config>().RedirectUnauthorizedIdentity = true;
                }
                if (conf.Methods.Any(o => o.Name == nameof(ISwaggerUI.GetOpenApiSpec)))
                {
                    conf.SetOAuth2Security(oauth2Iss);
                    conf.GetAdviceConfig<ISecurityOAuth2Config>().RedirectUnauthorizedIdentity = true;
                }
                if (conf.Methods.Any(o => o.Name == nameof(ISwaggerUI.GetSwaggerInitializer)))
                {
                    conf.SetOAuth2Security(oauth2Iss);
                    conf.GetAdviceConfig<ISecurityOAuth2Config>().RedirectUnauthorizedIdentity = true;
                }
                return true;
            });
            services.AddSolidRpcOAuth2(a => {
                a.AddDefaultScopes("authorization_code", new[] { "openid", "solidrpc", "offline_access" });
            });
            //services.ConfigureSolidRpcOAuth2(oauth2Iss, a => {
            //    a.AddDefaultScopes("authorization_code", new[] { "offline_access" });
            //});
            services.AddSolidRpcOAuth2Local(a =>
            {
                a.CreateSigningKey();
            });

            services.AddPetstore();
            //services.GetSolidRpcContentStore().AddMapping("/", async sp =>
            //{
            //    var handler = sp.GetRequiredService<IInvoker<ISwaggerUI>>();
            //    return await handler.GetUriAsync(o => o.GetIndexHtml(true, CancellationToken.None));
            //});
            services.AddSolidRpcWellKnownRootRewrite();


            //services.AddSolidRpcSecurityFrontend((sp, conf) =>
            //{
            //    //conf.Authority = "https://login.microsoftonline.com/common/v2.0";
            //    //conf.ClientId = "615993a8-66b3-40ce-a165-96a81edd3677";
            //});
            //services.AddSolidRpcSecurityBackend();
            //services.AddSolidRpcSecurityBackendFacebook((sp, conf) =>
            //{
            //    sp.ConfigureOptions(conf);
            //});
            //services.AddSolidRpcSecurityBackendGoogle((sp, conf) =>
            //{
            //    sp.ConfigureOptions(conf);
            //});
            //services.AddSolidRpcSecurityBackendMicrosoft((sp, conf) =>
            //{
            //    sp.ConfigureOptions(conf);
            //});
            //services.AddPetstore();
            //services.AddVitec();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSolidRpcProxies((context) =>
            {
                context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = null;
                return Task.CompletedTask;
            });
        }
    }
}
