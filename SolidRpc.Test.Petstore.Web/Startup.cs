using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Test.Petstore.Impl;
using SolidRpc.Test.Petstore.Services;
using System.Linq;

namespace SolidRpc.Test.PetstoreWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPet, PetImpl>();

            services.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            services.AddSolidRpcBindings(typeof(IPet).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSolidRpcProxies();

            app.UseSwaggerUI(swaggerConf =>
            {
                app.ApplicationServices.GetRequiredService<IMethodBinderStore>()
                    .MethodBinders.ToList().ForEach(mb => {
                        var endpointUri = app.UseOpenApiConfig(mb);
                        swaggerConf.SwaggerEndpoint(endpointUri, $"{mb.Assembly.GetName().Name}");
                    });
            });
        }
    }
}
