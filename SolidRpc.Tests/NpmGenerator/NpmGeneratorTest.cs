using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.NpmGenerator.Services;
using SolidRpc.NpmGenerator.Types;
using SolidProxy.GeneratorCastle;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;
using SolidRpc.Security.Services;

namespace SolidRpc.Tests.NpmGenerator
{
    /// <summary>
    /// Tests the node services
    /// </summary>
    public class NpmGeneratorTest : TestBase
    {
        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test, Ignore("Problem with path in tests - works on webserver.")]
        public async Task TestNodeServices()
        {
            var sc = new ServiceCollection();
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcNpmGenerator();

            var sp = sc.BuildServiceProvider();
            var npmGenerator = sp.GetRequiredService<INpmGenerator>();

            var npmPackage = new NpmPackage()
            {
                Files = new[]
                {
                    new NpmPackageFile()
                    {
                        FilePath = "package.json",
                        Content = @"{
    ""name"": ""test"",
    ""version"": ""1.0.0"",
    ""devDependencies"": {
        ""webpack"": ""4.39.2""
    },
    ""scripts"": {
        ""build"": ""webpack --mode production""
    }
}"
                    },
                    new NpmPackageFile()
                    {
                        FilePath = "tsconfig.json",
                        Content = @"
{
  ""compilerOptions"": {
    ""target"": ""es5"",       
    ""module"": ""commonjs"",  
    ""outDir"": ""./dist"",    
  },
  ""include"": [""./src/**/*""],
  ""exclude"": [
    ""node_modules""
  ]
}"
                    }
                }
            };

            var compiledPackage = await npmGenerator.RunNpm("webpack", npmPackage);

            // make sure that all the supplied files are returned.
            var files = new List<NpmPackageFile>(compiledPackage.Files);
            foreach (var f in npmPackage.Files)
            {
                Assert.AreEqual(1, files.RemoveAll(o => o.FilePath == f.FilePath));
            }


            //            var nodeServices = sp.GetRequiredService<INodeServices>();
            //            var res = await nodeServices.InvokeAsync<string>("NodeServices\\main.js", @"
            //declare function greet(greeting: string): void;
            //let x=5;
            //");
        }

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
            sc.AddSolidRpcNpmGenerator();

            var sp = sc.BuildServiceProvider();
            var npmGenerator = sp.GetRequiredService<INpmGenerator>();

            var codeNamespace = await npmGenerator.CreateCodeNamespace(typeof(INpmGenerator).Assembly.GetName().Name);
            var iNpmGenerator = codeNamespace
                .Namespaces.Single(o => o.Name == "SolidRpc")
                .Namespaces.Single(o => o.Name == "NpmGenerator")
                .Namespaces.Single(o => o.Name == "Services")
                .Interfaces.Single(o => o.Name == "INpmGenerator");
            Assert.IsNotNull(iNpmGenerator);

            var mCreateCodeNamespace = iNpmGenerator.Methods.Single(o => o.Name == nameof(INpmGenerator.CreateCodeNamespace));
            Assert.AreEqual("assemblyName", mCreateCodeNamespace.Arguments.First().Name);
            Assert.AreEqual(new string[] { "string" }, mCreateCodeNamespace.Arguments.First().ArgType);
            Assert.AreEqual(new string[] { "SolidRpc", "NpmGenerator", "Types", "CodeNamespace" }, mCreateCodeNamespace.ReturnType);
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
            sc.AddSolidRpcNpmGenerator();
            sc.AddSolidRpcSecurity();
            sc.AddSolidRpcSecurityMicrosoft((o, opts) =>
            {
            });
            sc.AddSolidRpcSecurityGoogle((o, opts) =>
            {
            });
            sc.AddSolidRpcSecurityFacebook((o, opts) =>
            {
            });

            var sp = sc.BuildServiceProvider();
            var npmGenerator = sp.GetRequiredService<INpmGenerator>();

            var ts = await npmGenerator.CreateTypesTs(typeof(ISolidRpcSecurity).Assembly.GetName().Name);
            using (var fs = new FileInfo(@"C:\Development\github\solidrpc\SolidRpc.Tests\NpmGenerator\src\types.ts").CreateText())
            {
                fs.Write(ts);
            }
            var tsTemplate = GetManifestResourceAsString("TestCreateTypescript.ts");

            Assert.AreEqual(tsTemplate, ts);
        }
    }
}
