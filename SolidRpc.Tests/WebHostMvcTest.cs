using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
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
            app.UseDeveloperExceptionPage();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => {
                    swaggerDoc.Host = httpReq.Host.Value;
                    swaggerDoc.Schemes = new string[] { httpReq.Scheme };
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvcWithDefaultRoute();
        }

        /// <summary>
        /// Configures the services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public override IServiceProvider ConfigureServerServices(IServiceCollection services)
        {
            services.AddMvc().ConfigureApplicationPartManager(apm =>
            {
                var ap = new AssemblyPart(GetType().Assembly);
                apm.ApplicationParts.Add(ap);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Title = "My API",
                    Version = "v1"
                });

                c.OperationFilter<IFormFileOperationFilter>(); //Register File Upload Operation Filter

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services.BuildServiceProvider();
        }
    }
}