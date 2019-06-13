using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.OpenApi.Generator;
using SolidRpc.OpenApi.Generator.Impl.Services;
using SolidRpc.OpenApi.Generator.Services;
using SolidRpc.OpenApi.Generator.Types;
using System.Threading.Tasks;
using System.IO;

namespace SolidRpc.Tests.Swagger
{
    /// <summary>
    /// Tests swagger generator functionality.
    /// </summary>
    public class SwaggerGenTest : WebHostMvcTest
    {
        public override void ConfigureClientServices(IServiceCollection services)
        {
            base.ConfigureClientServices(services);
            services.AddTransient<IOpenApiGenerator, OpenApiGenerator>();
        }

        /// <summary>
        /// Creates the local bindings
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestTestsGenerator()
        {
            using (var ctx = CreateTestHostContext())
            {
                var resp = await ctx.GetResponse("/swagger/v1/swagger.json");
                var swaggerSpec = await AssertOk(resp);
                var settings = new SettingsCodeGen()
                {
                    SwaggerSpec = swaggerSpec,
                    ProjectNamespace = "SolidRpc.Tests",
                    CodeNamespace = "Generated.Local"
                };

                var projPath = GetProjectFolder("SolidRpc.Tests");

                var gen = ctx.ServiceProvider.GetRequiredService<IOpenApiGenerator>();
                var project = await gen.CreateCodeFromOpenApiSpec(settings);
                var zip = await gen.CreateProjectZip(project);
                await projPath.WriteFileDataZip(zip);
            }
        }
    }
}
