using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using SolidRpc.Abstractions.Services.Code;
using System;
using System.Threading;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Node.Services;
using SolidRpc.Node.Types;
using System.IO;
using System.Text;
using System.Collections.Generic;
using SolidRpc.Abstractions.Serialization;
using System.Runtime.Serialization;

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

        public class ComplexType
        {
            [DataMember(Name = "int")]
            public int Integer { get; set; }

            [DataMember(Name = "str")]
            public string String { get; set; }
        }

        public interface ITestInterface
        {
            Task<bool> ProxyBooleanAsync(bool x, CancellationToken cancellation = default(CancellationToken));
            Task<byte> ProxyByteAsync(byte x, CancellationToken cancellation = default(CancellationToken));
            Task<short> ProxyShortAsync(short x, CancellationToken cancellation = default(CancellationToken));
            Task<int> ProxyIntegerAsync(int x, CancellationToken cancellation = default(CancellationToken));
            Task<string> ProxyStringAsync(string x, CancellationToken cancellation = default(CancellationToken));
            Task<decimal> ProxyDecimalAsync(decimal x, CancellationToken cancellation = default(CancellationToken));
            Task<float> ProxyFloatAsync(float x, CancellationToken cancellation = default(CancellationToken));
            Task<double> ProxyDoubleAsync(double x, CancellationToken cancellation = default(CancellationToken));
            Task<Guid> ProxyGuidAsync(Guid x, CancellationToken cancellation = default(CancellationToken));
            Task<Uri> ProxyUriAsync(Uri x, CancellationToken cancellation = default(CancellationToken));
            Task<ComplexType> ProxyComplexTypeAsync(ComplexType x, CancellationToken cancellation = default(CancellationToken));

            Task<bool?> ProxyOBooleanAsync(bool? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<byte?> ProxyOByteAsync(byte? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<short?> ProxyOShortAsync(short? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<int?> ProxyOIntegerAsync(int? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<string> ProxyOStringAsync(string x = null, CancellationToken cancellation = default(CancellationToken));
            Task<decimal?> ProxyODecimalAsync(decimal? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<float?> ProxyOFloatAsync(float? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<double?> ProxyODoubleAsync(double? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<Guid?> ProxyOGuidAsync(Guid? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<Uri> ProxyOUriAsync(Uri x = null, CancellationToken cancellation = default(CancellationToken));
        }
        public class TestInterfaceImpl : ITestInterface
        {
            public Task<bool> ProxyBooleanAsync(bool x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<byte> ProxyByteAsync(byte x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<ComplexType> ProxyComplexTypeAsync(ComplexType x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<decimal> ProxyDecimalAsync(decimal x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<double> ProxyDoubleAsync(double x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<float> ProxyFloatAsync(float x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<Guid> ProxyGuidAsync(Guid x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<int> ProxyIntegerAsync(int x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<bool?> ProxyOBooleanAsync(bool? x = null, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<byte?> ProxyOByteAsync(byte? x = null, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<decimal?> ProxyODecimalAsync(decimal? x = null, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<double?> ProxyODoubleAsync(double? x = null, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<float?> ProxyOFloatAsync(float? x = null, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<Guid?> ProxyOGuidAsync(Guid? x = null, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<int?> ProxyOIntegerAsync(int? x = null, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<short?> ProxyOShortAsync(short? x = null, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<string> ProxyOStringAsync(string x = null, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<Uri> ProxyOUriAsync(Uri x = null, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<short> ProxyShortAsync(short x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<string> ProxyStringAsync(string x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<Uri> ProxyUriAsync(Uri x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
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
                Assert.AreEqual(4711, await ti.ProxyIntegerAsync(4711));
                Assert.AreEqual("str", await ti.ProxyStringAsync("str"));
                Assert.AreEqual(1.43m, await ti.ProxyDecimalAsync(1.43m));

                // now we create typescript & compile
                var assemblyName = typeof(ITestInterface).Assembly.GetName().Name;
                var packages = new Dictionary<string, CompiledTs>();
                await CreatePackage(ctx.ClientServiceProvider, packages, "SolidRpc");
                await CreatePackage(ctx.ClientServiceProvider, packages, assemblyName);

                //var tsg = ctx.ClientServiceProvider.GetRequiredService<ITypescriptGenerator>();
                //var ts = await tsg.CreateTypesTsForAssemblyAsync(assemblyName);
                /*
                var ts = GetAssemblyResource($"{nameof(TestInvoke)}.ts");
                await WritePackageFile("tsconfig.json", NodeModuleRpcResolver.tsconfig);
                await WritePackageFile("index.ts", ts);
                var js = await CompileTsAsync(ctx.ClientServiceProvider, packages, ts);
                await WritePackageFile("index.js", js.Js);
                */
                var guid = Guid.NewGuid();
                var uri = new Uri("ws://test.ws/ws");
                var ct = new ComplexType() { String = "test string", Integer = 123 };
                ctx.ClientServiceProvider.GetRequiredService<ISerializerFactory>().SerializeToString(out string strCt, ct);

                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyBooleanAsync), (bool)true, "true");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyBooleanAsync), (bool)false, "false");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyByteAsync), (byte)17, "17");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyShortAsync), (short)11, "11");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyIntegerAsync), (int)4711, "4711");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyStringAsync), (string)"My string", "\"My string\"");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyDecimalAsync), (decimal)1.43m, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyFloatAsync), (float)1.43f, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyDoubleAsync), (double)1.43d, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyGuidAsync), guid, $"\"{guid}\"");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyUriAsync), uri, $"\"{uri}\"");

                await RunTestScriptNoArgConvAsync<ComplexType>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyComplexTypeAsync), $"new x.TypeScriptTest.ComplexType({strCt})", "{\"Integer\":123,\"String\":\"test string\"}");

                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOBooleanAsync), (bool)true, "true");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOBooleanAsync), (bool)false, "false");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOByteAsync), (byte)17, "17");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOShortAsync), (short)11, "11");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOIntegerAsync), (int)4711, "4711");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOStringAsync), (string)"My string", "\"My string\"");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyODecimalAsync), (decimal)1.43m, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOFloatAsync), (float)1.43f, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyODoubleAsync), (double)1.43d, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOGuidAsync), guid, $"\"{guid}\"");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOUriAsync), uri, $"\"{uri}\"");
            }

        }

        private async Task RunTestScriptAsync<T>(IServiceProvider sp, Dictionary<string, CompiledTs> packages, string methodName, T input, string jsonText)
        {
            var sf = sp.GetRequiredService<ISerializerFactory>();
            sf.SerializeToString(out string jsInput, input);
            var res = await RunTestScriptNoArgConvAsync<T>(sp, packages, methodName, jsInput, jsonText);
            Assert.AreEqual(input, res);
        }

        private async Task<T> RunTestScriptNoArgConvAsync<T>(IServiceProvider sp, Dictionary<string, CompiledTs> packages, string methodName, string jsInput, string jsonText)
        {

            var ns = sp.GetRequiredService<INodeService>();
            var js = $@"const x = require(""solidrpc.tests""); (async function(){{
  return await x.TypeScriptTest.TestInterfaceInstance.{methodName}({jsInput}).toPromise();
}})();";
            var nodeRes = await ns.ExecuteScriptAsync(new NodeExecutionInput()
            {
                Js = js,
                InputFiles = CreateInputFiles(packages),
                ModuleId = NodeModuleRpcResolver.GuidModuleId
            });

            if(nodeRes.ExitCode != 0)
            {
                throw new Exception(nodeRes.Err);
            }
            Assert.AreEqual(jsonText, nodeRes.Result);

            var sf = sp.GetRequiredService<ISerializerFactory>();
            sf.DeserializeFromString<T>(nodeRes.Result, out T res);
            return res;
        }

        private string GetAssemblyResource(string name)
        {
            var a = GetType().Assembly;
            var resourceName = a.GetManifestResourceNames().Single(o => o.EndsWith(name));
            using(var sr = new StreamReader(a.GetManifestResourceStream(resourceName)))
            {
                return sr.ReadToEnd();
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
            var inputFiles = CreateInputFiles(packages).Union(new[] {
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

        private IEnumerable<NodeExecutionFile> CreateInputFiles(Dictionary<string, CompiledTs> packages)
        {
            return packages.Select(o => new NodeExecutionFile()
            {
                FileName = $"node_modules/{o.Key}/index.js",
                Content = new MemoryStream(Encoding.UTF8.GetBytes(o.Value.Js))
            }).Union(packages.Select(o => new NodeExecutionFile()
            {
                FileName = $"node_modules/{o.Key}/index.d.ts",
                Content = new MemoryStream(Encoding.UTF8.GetBytes(o.Value.DTs))
            })).ToList();
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
