using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Tests
{
    public class WebHostMvcTest : WebHostTest
    {
        public override void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvcWithDefaultRoute();
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().ConfigureApplicationPartManager(apm =>
            {
                var ap = new AssemblyPart(GetType().Assembly);
                apm.ApplicationParts.Add(ap);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            return services.BuildServiceProvider();
        }
    }
}