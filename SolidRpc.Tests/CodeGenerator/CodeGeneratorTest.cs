using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SolidRpc.Abstractions.Services.Code;
using SolidRpc.Abstractions.Services;

namespace SolidRpc.Tests.CodeGenerator
{
    /// <summary>
    /// Tests the node services
    /// </summary>
    public class CodeGeneratorTest : TestBase
    {
        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestCreateCodeNamespace()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcServices();
            sc.AddSolidRpcNode();

            var sp = sc.BuildServiceProvider();
            var codeGenerator = sp.GetRequiredService<ICodeNamespaceGenerator>();

            var assemblyName = typeof(INpmGenerator).Assembly.GetName().Name;
            var codeNamespace = await codeGenerator.CreateCodeNamespace(assemblyName);
            var iNpmGenerator = codeNamespace
                .Namespaces.Single(o => o.Name == "SolidRpc")
                .Namespaces.Single(o => o.Name == "Abstractions")
                .Namespaces.Single(o => o.Name == "Services")
                .Namespaces.Single(o => o.Name == "Code")
                .Interfaces.Single(o => o.Name == "INpmGenerator");
            Assert.IsNotNull(iNpmGenerator);

            var mCreateCodeNamespace = iNpmGenerator.Methods.Single(o => o.Name == nameof(INpmGenerator.CreateNpmPackage));
            Assert.AreEqual("assemblyNames", mCreateCodeNamespace.Arguments.First().Name);
            Assert.AreEqual(new string[] { "string", "[]" }, mCreateCodeNamespace.Arguments.First().ArgType);
            Assert.AreEqual(new string[] { "SolidRpc", "Abstractions", "Types", "Code", "NpmPackage", "[]" }, mCreateCodeNamespace.ReturnType);

            var tsGenerator = sp.GetRequiredService<ITypescriptGenerator>();
            var ts = await tsGenerator.CreateTypesTsForAssemblyAsync(assemblyName);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestCreateTypescript()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddHttpClient();
            sc.AddSolidRpcServices();
            sc.AddSolidRpcNode();

            var sp = sc.BuildServiceProvider();
            var tsGenerator = sp.GetRequiredService<ITypescriptGenerator>();

            var ts = await tsGenerator.CreateTypesTsForAssemblyAsync(typeof(ISolidRpcOidc).Assembly.GetName().Name);
            //using (var fs = new FileInfo(@"C:\Development\github\solidrpc\SolidRpc.Tests\NpmGenerator\src\types.ts").CreateText())
            //{
            //    fs.Write(ts);
            //}
            var tsTemplate = GetManifestResourceAsString("TestCreateTypescript.ts");

            Assert.AreEqual(tsTemplate.Replace("\r\n","\n"), ts.Replace("\r\n", "\n"));
        }
    }
}
