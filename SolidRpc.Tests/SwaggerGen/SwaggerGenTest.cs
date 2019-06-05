using NUnit.Framework;
using SolidRpc.OpenApi.Generator;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger
{
    /// <summary>
    /// Tests swagger generator functionality.
    /// </summary>
    public class SwaggerGenTest : WebHostMvcTest
    {
        /// <summary>
        /// Creates the local bindings
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestTestsGenerator()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse("/swagger/v1/swagger.json");
                var swaggerSpec = await AssertOk(resp);
                var settings = new OpenApiCodeSettings()
                {
                    SwaggerSpec = swaggerSpec,
                    OutputPath = GetProjectFolder("SolidRpc.Tests").FullName,
                    ProjectNamespace = "SolidRpc.Tests",
                    CodeNamespace = "Generated.Local"
                };

                OpenApiCodeGenerator.GenerateCode(settings);
            }
        }
    }
}
