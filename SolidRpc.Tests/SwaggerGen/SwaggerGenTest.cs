using NUnit.Framework;
using SolidRpc.Swagger.Generator;
using System.IO;

namespace SolidRpc.Tests.Swagger
{
    public class SwaggerGenTest : TestBase
    {
        [Test]
        public void TestPetStoreGenerator()
        {
            var swaggerSpec = new StreamReader(GetManifestResource("petstore.json")).ReadToEnd();
            var settings = new SwaggerCodeSettings()
            {
                SwaggerSpec = swaggerSpec,
                OutputPath = "C:\\Development\\github\\solidrpc\\SolidRpc.Tests\\tmp",
                //OutputPath = TestContext.CurrentContext.TestDirectory
                //RootNamespace = "SolidRpc.Tests.Swagger.Petstore"
            };

            SwaggerCodeGenerator.GenerateCode(settings);
        }
    }
}
