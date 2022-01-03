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

namespace SolidRpc.Test.PetstoreWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(o => {
                o.SetMinimumLevel(LogLevel.Trace);
                o.AddConsole();
            });
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            services.AddHttpClient();
            services.AddSolidRpcServices(o => true);
            services.AddSolidRpcAzTableQueue("AzureWebJobsStorage", "generic");
            services.AddSolidRpcSwaggerUI(conf =>
            {
                conf.DefaultOpenApiSpec = "SolidRpc.OpenApi.SwaggerUI";
                conf.OAuthClientId = "swagger-ui";
                conf.OAuthClientSecret = "swagger-ui";
            }, conf =>
            {
                conf.DisableSecurity();
                //conf.SetOAuth2Security("https://identity.erikolsson.se");
                if(conf.Methods.Any(o => o.Name == nameof(ISwaggerUI.GetIndexHtml)))
                {
                    //conf.GetAdviceConfig<ISecurityOAuth2Config>().RedirectUnauthorizedIdentity = true;
                }
                return true;
            });
            services.AddSolidRpcOAuth2();

            services.AddPetstore();
            services.GetSolidRpcContentStore().AddMapping("/", async sp =>
            {
                var handler = sp.GetRequiredService<IInvoker<IPet>>();
                return await handler.GetUriAsync(o => o.GetPetById(100, CancellationToken.None));
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
