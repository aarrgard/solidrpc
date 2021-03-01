using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.NpmGenerator.Services;
using SolidRpc.NpmGenerator.Types;
using SolidProxy.GeneratorCastle;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SolidRpc.Security.Services;
using System.Threading;
using SolidRpc.Node.Services;
using System;

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
        [Test, Ignore("Implement")]
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
            sc.AddSolidRpcBindings(typeof(ISolidRpcSecurity), null);
            sc.AddSolidRpcNpmGenerator();
            sc.AddSolidRpcOAuth2Local("http://localhost", _ => { });
            sc.AddSolidRpcSecurityBackend();
            sc.AddSolidRpcSecurityBackendMicrosoft((o, opts) =>
            {
            });
            sc.AddSolidRpcSecurityBackendGoogle((o, opts) =>
            {
            });
            sc.AddSolidRpcSecurityBackendFacebook((o, opts) =>
            {
            });

            var sp = sc.BuildServiceProvider();
            var npmGenerator = sp.GetRequiredService<INpmGenerator>();

            var ts = await npmGenerator.CreateTypesTs(typeof(ISolidRpcSecurity).Assembly.GetName().Name);
            //using (var fs = new FileInfo(@"C:\Development\github\solidrpc\SolidRpc.Tests\NpmGenerator\src\types.ts").CreateText())
            //{
            //    fs.Write(ts);
            //}
            var tsTemplate = GetManifestResourceAsString("TestCreateTypescript.ts");

            Assert.AreEqual(tsTemplate, ts);
        }


        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestRunNodeJsSimple()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcNpmGenerator();

            var sp = sc.BuildServiceProvider();
            var nodeService = sp.GetRequiredService<INodeService>();
            var version = await nodeService.GetNodeVersionAsync();
            Assert.IsTrue(version.StartsWith("v"));



            for (int i = 0; i < 100; i++)
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfter(10000);
                var str = $"testing{i}";
                var res = await nodeService.ExecuteScriptAsync(Guid.Empty, $"(async function() {{ console.log('test'); return '{str}'; }})() ", cts.Token);
                Assert.AreEqual(0, res.ExitCode);
                Assert.AreEqual($"\"{str}\"", res.Result);
                Assert.AreEqual("test", res.Out.Trim());
            }

            for (int i = 0; i < 100; i++)
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfter(10000);
                var str = $"testing{i}";
                var res = await nodeService.ExecuteScriptAsync(Guid.Empty, $"console.log('test'); '{str}';", cts.Token);
                Assert.AreEqual(0, res.ExitCode);
                Assert.AreEqual($"\"{str}\"", res.Result);
                Assert.AreEqual("test", res.Out.Trim());
            }
        }


        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestRunNodeJsPuppeteer()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcNpmGenerator();

            var sp = sc.BuildServiceProvider();
            var nodeService = sp.GetRequiredService<INodeService>();


            for (int i = 0; i < 5; i++)
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfter(60000);
                var res = await nodeService.ExecuteScriptAsync(NodeModulePuppeteerResolver.GuidModuleId, @"
const puppeteer = require('puppeteer');
(async () => {
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('https://google.se');
    await page.screenshot({ path: 'example.png' });

    await browser.close();
    return 'example.png';
})();", cts.Token);
                Assert.AreEqual(0, res.ExitCode);
                Assert.AreEqual($"\"example.png\"", res.Result);
                Assert.AreEqual("", res.Out.Trim());
            }

            //Assert.AreEqual("", res.Err);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test]
        public async Task TestRunNodeJsNpm()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcNpmGenerator();

            var sp = sc.BuildServiceProvider();
            var nodeService = sp.GetRequiredService<INodeService>();

            var res = await nodeService.ExecuteFileAsync(NodeModuleNpmResolver.GuidModuleId, null, "npm\\bin\\npm-cli.js", new string[] { "--version"});

            Assert.AreEqual(0, res.ExitCode);
            Assert.AreEqual("7.6.0", res.Out.Trim());
            Assert.AreEqual("", res.Err);
        }
    }
}
