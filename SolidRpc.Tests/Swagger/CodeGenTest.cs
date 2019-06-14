using NUnit.Framework;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.DotNetTool;
using SolidRpc.OpenApi.Model;
using SolidRpc.OpenApi.Model.V2;
using SolidRpc.Test.Petstore.Services;
using SolidRpc.Test.Petstore.Types;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace SolidRpc.Tests.Swagger
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class CodeGenTest : TestBase
    {
        /// <summary>
        /// Tests generating code from a swagger file
        /// </summary>
        [Test]
        public void TestCodeGen()
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "CodeGen");
            var dir = new DirectoryInfo(path);
            Assert.IsTrue(dir.Exists);
            foreach(var subDir in dir.GetDirectories())
            {
                CreateCode(subDir, true);
            }
        }

        private void CreateCode(DirectoryInfo dir, bool onlyCompare)
        {
            var openApiFiles = dir.GetFiles("*.json");
            if(openApiFiles.Length != 1)
            {
                Assert.Fail("Cannot find json file in folder:" + dir.FullName);
            }
            Program.MainWithExeptions(new[] {
                "-openapi2code",
                "-d", dir.FullName,
                "-only-compare", onlyCompare.ToString(),
                "-ProjectNamespace", $"{GetType().Assembly.GetName().Name}.Swagger.CodeGen.{dir.Name}",
                openApiFiles[0].Name }).Wait();
        }
    }
}
