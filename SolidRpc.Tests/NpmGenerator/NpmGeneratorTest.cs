using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SolidProxy.GeneratorCastle;
using Microsoft.Extensions.Configuration;
using System.Threading;
using SolidRpc.Node.Services;
using System;
using SolidRpc.Node.Types;
using SolidRpc.Node.InternalServices;
using System.IO;

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
        [Test]
        public async Task TestRunNodeJsSimple()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcNode();

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
            sc.AddSolidRpcNode();

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
            sc.AddSolidRpcNode();

            var sp = sc.BuildServiceProvider();
            var nodeService = sp.GetRequiredService<INodeService>();

            var sep = Path.DirectorySeparatorChar;
            var res = await nodeService.ExecuteFileAsync(NodeModuleNpmResolver.GuidModuleId, null, $"npm{sep}bin{sep}npm-cli.js", new string[] { "--version" });

            Assert.AreEqual(0, res.ExitCode);
            Assert.AreEqual("10.5.0", res.Out.Trim());
            Assert.AreEqual("", res.Err);
        }
    }
}
