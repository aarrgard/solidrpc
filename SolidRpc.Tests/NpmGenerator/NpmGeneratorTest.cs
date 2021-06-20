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
using SolidRpc.Node.Types;
using SolidRpc.Abstractions.Services.Code;

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
                var res = await nodeService.ExecuteScriptAsync(new NodeExecutionInput()
                {
                    ModuleId = Guid.Empty,
                    Js = $"(async function() {{ console.log('test'); return '{str}'; }})() "
                }, cts.Token);
                Assert.AreEqual(0, res.ExitCode);
                Assert.AreEqual($"\"{str}\"", res.Result);
                Assert.AreEqual("test", res.Out.Trim());
            }

            for (int i = 0; i < 100; i++)
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfter(10000);
                var str = $"testing{i}";
                var res = await nodeService.ExecuteScriptAsync(new NodeExecutionInput()
                {
                    ModuleId = Guid.Empty,
                    Js = $"console.log('test'); '{str}';"
                }, cts.Token);
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
                cts.CancelAfter(120000);
                var res = await nodeService.ExecuteScriptAsync(new NodeExecutionInput()
                {
                    ModuleId = NodeModulePuppeteerResolver.GuidModuleId,
                    Js = @"
const puppeteer = require('puppeteer');
(async () => {
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    await page.goto('https://google.se');
    await page.screenshot({ path: 'example.png' });

    await browser.close();
    return 'example.png';
})();" }, cts.Token);
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
            Assert.AreEqual("7.13.0", res.Out.Trim());
            Assert.AreEqual("", res.Err);
        }
    }
}
