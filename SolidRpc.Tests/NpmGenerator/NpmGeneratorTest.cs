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
using SolidRpc.OpenApi.Binder.Proxy;
using System.Threading;
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
            sc.AddSolidRpcBindings(typeof(ISolidRpcSecurity), null);
            sc.AddSolidRpcNpmGenerator();
            sc.AddSolidRpcOAuth2Local(new Uri("http://localhost"), _ => { });
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
            Assert.AreEqual("v14.15.4", version);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(10000); 
            var res = await nodeService.ExecuteJSAsync("console.log('test');", cts.Token);

            Assert.AreEqual(0, res.ExitCode);
            Assert.AreEqual("test", res.Out);
            //Assert.AreEqual("", res.Err);
        }

        /// <summary>
        /// Tests the type store
        /// </summary>
        [Test, Ignore("Implement")]
        public async Task TestRunNodeJsNpm()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.AddLogging(ConfigureLogging);
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcNpmGenerator();

            var sp = sc.BuildServiceProvider();
            var nodeService = sp.GetRequiredService<INodeService>();
            await nodeService.DownloadPackageAsync("npm", "6.14.8");

            var res = await nodeService.ExecuteJSAsync(@"var npm = require('npm');
npm.load(function(err) {
  // handle errors

  // install module ffi
  npm.commands.install(['ffi'], function(er, data) {
    // log errors or data
  });

  npm.on('log', function(message) {
    // log installation progress
    console.log(message);
  });
});");

            Assert.AreEqual(0, res.ExitCode);
            //Assert.AreEqual("test", res.Out);
            //Assert.AreEqual("", res.Err);
        }
    }
}
