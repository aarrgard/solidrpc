using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.NpmGenerator.Services;
using SolidProxy.GeneratorCastle;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SolidRpc.Security.Services;
using SolidRpc.Abstractions.Services.Code;
using System;
using System.Threading;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Node.Services;
using SolidRpc.Node.Types;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace SolidRpc.Tests.CodeGenerator
{
    /// <summary>
    /// Tests the node services
    /// </summary>
    public class TypeScriptTest : WebHostTest
    {
        public class CompiledTs
        {
            public string Js { get; set; }
            public string DTs { get; set; }
        }
        public interface ITestInterface
        {
            Task<int> DoSomethingAsync(CancellationToken cancellation = default(CancellationToken));
        }
        public class TestInterfaceImpl : ITestInterface
        {
            public Task<int> DoSomethingAsync(CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(4711);
            }
        }

        public override void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            base.ConfigureClientServices(clientServices, baseAddress);
            var openApiSpec = clientServices.GetSolidRpcService<IOpenApiParser>()
                .CreateSpecification(typeof(ITestInterface), typeof(ITypescriptGenerator))
                .SetBaseAddress(baseAddress)
                .WriteAsJsonString();
            clientServices.AddSolidRpcBindings(typeof(ITestInterface), null, c => {
                c.OpenApiSpec = openApiSpec;
                return true;
            });
            clientServices.AddSolidRpcBindings(typeof(ITypescriptGenerator), null, c => {
                c.OpenApiSpec = openApiSpec;
                return true;
            });
            clientServices.AddSolidRpcNode();
        }

        public override void ConfigureServerServices(IServiceCollection serverServices)
        {
            base.ConfigureServerServices(serverServices);
            var openApiSpec = serverServices.GetSolidRpcService<IOpenApiParser>().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            serverServices.AddSolidRpcServices();
            serverServices.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestInterfaceImpl), c => { 
                c.OpenApiSpec = openApiSpec; 
                return true; 
            });
        }

        /// <summary>
        /// Tests the javascript invocation
        /// </summary>
        [Test]
        public async Task TestInvoke()
        {
            using (var ctx = await StartKestrelHostContextAsync())
            {
                // make sure that the c# bindings work
                var ti = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                var res = await ti.DoSomethingAsync();
                Assert.AreEqual(4711, res);

                // now we create typescript & compile
                var assemblyName = typeof(ITestInterface).Assembly.GetName().Name;
                var packages = new Dictionary<string, CompiledTs>();
                await CreatePackage(ctx.ClientServiceProvider, packages, "SolidRpc");
                await CreatePackage(ctx.ClientServiceProvider, packages, assemblyName);

                //var tsg = ctx.ClientServiceProvider.GetRequiredService<ITypescriptGenerator>();
                //var ts = await tsg.CreateTypesTsForAssemblyAsync(assemblyName);
                //await WritePackageFile("index.ts", ts);
                //var js = await CompileTsAsync(ctx.ClientServiceProvider, packages, ts);
            }

        }

        private async Task CreatePackage(IServiceProvider sp, Dictionary<string, CompiledTs> packages, string assemblyName)
        {
            var tsg = sp.GetRequiredService<ITypescriptGenerator>();
            var ts = await tsg.CreateTypesTsForAssemblyAsync(assemblyName);
            var js = await CompileTsAsync(sp, packages, ts);
            packages[assemblyName.ToLower()] = js;

            await WriteModuleFile(assemblyName.ToLower(), "index.js", js.Js);
            await WriteModuleFile(assemblyName.ToLower(), "index.d.ts", js.DTs);
            await WriteModuleFile(assemblyName.ToLower(), "index.ts", ts);
        }

        private Task WriteModuleFile(string packageName, string fileName, string fileContent)
        {
            return WritePackageFile($"node_modules\\{packageName}\\{fileName}", fileContent);
        }

        private async Task WritePackageFile(string fileName, string fileContent)
        {
            var fi = new FileInfo($"c:\\tmp\\code\\{fileName}");
            fi.Directory.Create();
            using (var fs = fi.CreateText())
            {
                await fs.WriteAsync(fileContent);
            }
        }

        private async Task<CompiledTs> CompileTsAsync(IServiceProvider sp, Dictionary<string, CompiledTs> packages, string ts)
        {
            var inputFiles = packages.Select(o => new NodeExecutionFile()
            {
                FileName = $"node_modules/{o.Key}/index.js",
                Content = new MemoryStream(Encoding.UTF8.GetBytes(o.Value.Js))
            }).Union(packages.Select(o => new NodeExecutionFile()
            {
                FileName = $"node_modules/{o.Key}/index.d.ts",
                Content = new MemoryStream(Encoding.UTF8.GetBytes(o.Value.DTs))
            })).Union(new[] {
                new NodeExecutionFile()
                {
                    FileName = $"ts.ts",
                    Content = new MemoryStream(Encoding.UTF8.GetBytes(ts))
                }
            }).ToList();

            var sep = Path.DirectorySeparatorChar;
            var ns = sp.GetRequiredService<INodeService>();
            var nodeRes = await ns.ExecuteScriptAsync(new NodeExecutionInput()
            {
                Js = $"typescript{sep}lib{sep}tsc.js",
                Args = new string[0],
                InputFiles = inputFiles,
                ModuleId = NodeModuleRpcResolver.GuidModuleId
            });

            if(nodeRes.ExitCode != 0)
            {
                throw new Exception("NodeError:" + nodeRes.Out);
            }
            return new CompiledTs()
            {
                Js = await GetContent(nodeRes.ResultFiles, "ts.js"),
                DTs = await GetContent(nodeRes.ResultFiles, "ts.d.ts")
            };
        }

        private async Task<string> GetContent(IEnumerable<NodeExecutionFile> resultFiles, string fileName)
        {

            string jsContent;
            var resultFile = resultFiles.Single(o => o.FileName == fileName);
            using (var sr = new StreamReader(resultFile.Content))
            {
                jsContent = await sr.ReadToEndAsync();
            }
            return jsContent;
        }
    }
}
