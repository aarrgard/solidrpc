using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using Microsoft.Extensions.Logging;

namespace SolidRpc.Test.PetstoreWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(o => o.SetMinimumLevel(LogLevel.Trace));
            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            services.AddSolidRpcServices(o =>
            {
                o.AddRpcHostServices = true;
                o.AddStaticContentServices = true;
            });
            services.AddSolidRpcSwaggerUI(o => {
                o.DefaultOpenApiSpec = "SolidRpc.Security.ISolidRpcSecurity";
            });
            services.AddSolidRpcNpmGenerator();
            services.AddSolidRpcSecurityFrontend();
            services.AddSolidRpcSecurityBackend();
            //services.AddSolidRpcSecurityBackendFacebook((sp, conf) =>
            //{
            //    sp.ConfigureOptions(conf);
            //});
            //services.AddSolidRpcSecurityBackendGoogle((sp, conf) =>
            //{
            //    sp.ConfigureOptions(conf);
            //});
            services.AddSolidRpcSecurityBackendMicrosoft((sp, conf) =>
            {
                sp.ConfigureOptions(conf);
            });
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

            app.UseSolidRpcProxies();
        }
    }
}
