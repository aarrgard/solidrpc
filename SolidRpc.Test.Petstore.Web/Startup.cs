﻿using Microsoft.AspNetCore.Builder;
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

namespace SolidRpc.Test.PetstoreWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var oauth2Iss = "https://localhost:5001/";
            //var oauth2Iss = "https://identity.erikolsson.se";
            services.AddLogging(o => {
                o.SetMinimumLevel(LogLevel.Trace);
                o.AddConsole();
            });
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            services.AddHttpClient();
            services.AddSolidRpcServices(conf =>
            {
                if (conf.Methods.First().DeclaringType == typeof(ISolidRpcOAuth2))
                {
                    conf.SetOAuth2ClientSecurity(oauth2Iss, "eo-prd-vitec-web", "0a5fa78b-da2b-401a-89b5-a5239bc32ac3");
                    conf.DisableSecurity();
                }
                return true;
            });
            services.AddSolidRpcAzTableQueue("AzureWebJobsStorage", "generic");
            services.AddSolidRpcSwaggerUI(conf =>
            {
                conf.DefaultOpenApiSpec = "SolidRpc.OpenApi.SwaggerUI";
                conf.OAuthClientId = "swagger-ui";
                conf.OAuthClientSecret = "swagger-ui";
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
                return true;
            });
            services.AddSolidRpcOAuth2(a => {
                a.AddDefaultScopes("authorization_code", new[] { "openid", "solidrpc"/*, "offline_access"*/ });
            });
            //services.ConfigureSolidRpcOAuth2(oauth2Iss, a => {
            //    a.AddDefaultScopes("authorization_code", new[] { "offline_access" });
            //});
            services.AddSolidRpcOAuth2Local(oauth2Iss, a =>
            {
                a.CreateSigningKey();
            });

            services.AddPetstore();
            services.GetSolidRpcContentStore().AddMapping("/", async sp =>
            {
                var handler = sp.GetRequiredService<IInvoker<IPet>>();
                return await handler.GetUriAsync(o => o.GetPetById(100, CancellationToken.None));
            });
            services.GetSolidRpcContentStore().AddMapping("/.well-known/openid-configuration", async sp =>
            {
                var handler = sp.GetRequiredService<IInvoker<ISolidRpcOidc>>();
                return await handler.GetUriAsync(o => o.GetDiscoveryDocumentAsync(CancellationToken.None));
            });
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
