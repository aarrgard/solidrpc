using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace SolidRpc.Tests
{
    /// <summary>
    /// Base test for mvc applications
    /// </summary>
    public class WebHostMvcTest : WebHostTest
    {
        /// <summary>
        /// Configures the application
        /// </summary>
        /// <param name="app"></param>
        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Configures the services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override void ConfigureServerServices(IServiceCollection services)
        {
            base.ConfigureServerServices(services);
            services.AddMvc().ConfigureApplicationPartManager(apm =>
            {
                var ap = new AssemblyPart(GetType().Assembly);
                apm.ApplicationParts.Add(ap);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "My API",
                    Version = "v1"
                });

                c.OperationFilter<IFormFileOperationFilter>(); //Register File Upload Operation Filter

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }
    }
}