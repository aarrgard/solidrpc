using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Model;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services.Code;
using SolidRpc.Abstractions.Types.Code;
using SolidRpc.Node.Services;
using SolidRpc.Node.Types;
using SolidRpc.OpenApi.SwaggerUI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            [DataMember(Name = "sarr")]
            public IEnumerable<string> StringArr { get; set; }
        }

        public interface ITestInterface
        {
            Task<bool> ProxyBooleanAsync(bool x, CancellationToken cancellation = default(CancellationToken));
            Task<byte> ProxyByteAsync(byte x, CancellationToken cancellation = default(CancellationToken));
            Task<short> ProxyShortAsync(short x, CancellationToken cancellation = default(CancellationToken));
            Task<int> ProxyIntegerAsync(int x, CancellationToken cancellation = default(CancellationToken));
            Task<long> ProxyLongAsync(long x, CancellationToken cancellation = default(CancellationToken));
            Task<string> ProxyStringAsync(string x, CancellationToken cancellation = default(CancellationToken));
            Task<decimal> ProxyDecimalAsync(decimal x, CancellationToken cancellation = default(CancellationToken));
            Task<float> ProxyFloatAsync(float x, CancellationToken cancellation = default(CancellationToken));
            Task<double> ProxyDoubleAsync(double x, CancellationToken cancellation = default(CancellationToken));
            Task<DateTime> ProxyDateTimeAsync(DateTime x, CancellationToken cancellation = default(CancellationToken));
            Task<DateTimeOffset> ProxyDateTimeOffsetAsync(DateTimeOffset x, CancellationToken cancellation = default(CancellationToken));
            Task<Guid> ProxyGuidAsync(Guid x, CancellationToken cancellation = default(CancellationToken));
            Task<Uri> ProxyUriAsync(Uri x, CancellationToken cancellation = default(CancellationToken));
            Task<ComplexType> ProxyComplexTypeAsync(ComplexType x, CancellationToken cancellation = default(CancellationToken));
            Task<IDictionary<string,string>> ProxyDictionaryAsync(IDictionary<string, string> x, CancellationToken cancellation = default(CancellationToken));

            Task<bool?> ProxyOBooleanAsync(bool? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<byte?> ProxyOByteAsync(byte? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<short?> ProxyOShortAsync(short? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<int?> ProxyOIntegerAsync(int? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<long?> ProxyOLongAsync(long? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<string> ProxyOStringAsync(string x = null, CancellationToken cancellation = default(CancellationToken));
            Task<decimal?> ProxyODecimalAsync(decimal? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<float?> ProxyOFloatAsync(float? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<double?> ProxyODoubleAsync(double? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<DateTime?> ProxyODateTimeAsync(DateTime? x = null, CancellationToken cancellation = default(CancellationToken));
            Task<DateTimeOffset?> ProxyODateTimeOffsetAsync(DateTimeOffset? x = null, CancellationToken cancellation = default(CancellationToken));
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

            public Task<DateTime> ProxyDateTimeAsync(DateTime x, CancellationToken cancellation = default)
            {
                return Task.FromResult(x);
            }

            public Task<DateTimeOffset> ProxyDateTimeOffsetAsync(DateTimeOffset x, CancellationToken cancellation = default)
            {
                return Task.FromResult(x);
            }

            public Task<decimal> ProxyDecimalAsync(decimal x, CancellationToken cancellation = default(CancellationToken))
            {
                return Task.FromResult(x);
            }

            public Task<IDictionary<string, string>> ProxyDictionaryAsync(IDictionary<string, string> x, CancellationToken cancellation = default)
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

            public Task<long> ProxyLongAsync(long x, CancellationToken cancellation = default)
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

            public Task<DateTime?> ProxyODateTimeAsync(DateTime? x, CancellationToken cancellation = default)
            {
                return Task.FromResult(x);
            }

            public Task<DateTimeOffset?> ProxyODateTimeOffsetAsync(DateTimeOffset? x, CancellationToken cancellation = default)
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

            public Task<long?> ProxyOLongAsync(long? x = 0, CancellationToken cancellation = default)
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
                c.SetSecurityKey("Authorization", "Bearer mykey");
                return true;
            });
            clientServices.AddSolidRpcServices(o => true);
            clientServices.AddSolidRpcNode();
            clientServices.AddSolidRpcSwaggerUI();
        }

        public override void ConfigureServerServices(IServiceCollection serverServices)
        {
            base.ConfigureServerServices(serverServices);
            var openApiSpec = serverServices.GetSolidRpcService<IOpenApiParser>().CreateSpecification(typeof(ITestInterface)).WriteAsJsonString();
            serverServices.AddSolidRpcServices();
            serverServices.AddSolidRpcBindings(typeof(ITestInterface), typeof(TestInterfaceImpl), c => { 
                c.OpenApiSpec = openApiSpec;
                c.SetSecurityKey("Authorization", "Bearer mykey");
                return true; 
            });
        }

        /// <summary>
        /// Tests the javascript invocation
        /// </summary>
        [Test]
        public async Task TestCompile()
        {
            using (var ctx = await StartKestrelHostContextAsync())
            {
                //await CreatePackage(ctx.ClientServiceProvider, "SolidRpcJs");
                await CreatePackage(ctx.ClientServiceProvider, "SolidRpcNode");
                await CreatePackage(ctx.ClientServiceProvider, typeof(ISwaggerUI).Assembly.GetName().Name);
                await CreatePackage(ctx.ClientServiceProvider, typeof(ITypescriptGenerator).Assembly.GetName().Name);
            }
        }

        /// <summary>
        /// Tests the javascript invocation
        /// </summary>
        [Test]
        public async Task TestCompileEoBankId()
        {
            using (var ctx = CreateKestrelHostContext(ss => {}, cs =>
            {
                cs.AddSolidRpcBindings(typeof(EO.BankId.Services.IBankId));
                cs.AddSolidRpcBindings(typeof(EO.BankId.IpApi.Services.IIpApi));
            }))
            {
                await ctx.StartAsync();
                await CreatePackage(ctx.ClientServiceProvider, typeof(EO.BankId.Services.IBankId).Assembly.GetName().Name);
                await CreatePackage(ctx.ClientServiceProvider, typeof(EO.BankId.IpApi.Services.IIpApi).Assembly.GetName().Name);
            }
        }

        /// <summary>
        /// Tests the javascript invocation
        /// </summary>
        [Test]
        public async Task TestInvoke()
        {
            using (var ctx = await StartKestrelHostContextAsync())
            {
                var guid = Guid.NewGuid();
                var uri = new Uri("ws://test.ws/ws");
                var ct = new ComplexType() { String = "test string", Integer = 123, StringArr = new[] { "test" } };
                var dict = new Dictionary<string, string>() { { "key", "value"} };
                var dt = DateTime.Now;
                dt = dt.AddTicks(-(dt.Ticks % TimeSpan.TicksPerSecond));
                var dtu = dt.ToUniversalTime();

                var dto = DateTimeOffset.Now;
                dto = dto.AddTicks(-(dto.Ticks % TimeSpan.TicksPerSecond));
                var dtou = dto.ToUniversalTime();

                // make sure that the c# bindings work
                var ti = ctx.ClientServiceProvider.GetRequiredService<ITestInterface>();
                Assert.AreEqual(4711, await ti.ProxyIntegerAsync(4711));
                Assert.AreEqual("str", await ti.ProxyStringAsync("str"));
                Assert.AreEqual(1.43m, await ti.ProxyDecimalAsync(1.43m));
                Assert.AreEqual(dt, await ti.ProxyDateTimeAsync(dt));
                Assert.AreEqual(dto, await ti.ProxyDateTimeOffsetAsync(dto));

                // now we create typescript & compile
                var assemblyName = typeof(ITestInterface).Assembly.GetName().Name;
                var ts = await ctx.ClientServiceProvider.GetRequiredService<ITypescriptGenerator>().CreateTypesTsForAssemblyAsync(assemblyName);
                //await CreatePackage(ctx.ClientServiceProvider, "SolidRpc");
                var packages = await CreatePackage(ctx.ClientServiceProvider, assemblyName);

                ctx.ClientServiceProvider.GetRequiredService<ISerializerFactory>().SerializeToString(out string strCt, ct);

                // invoke without args
                await RunTestScriptNoArgConvAsync<bool?>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOBooleanAsync), "", "null");
                await RunTestScriptNoArgConvAsync<byte?>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOByteAsync), "", "null");
                await RunTestScriptNoArgConvAsync<short?>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOShortAsync), "", "null");
                await RunTestScriptNoArgConvAsync<int?>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOIntegerAsync), "", "null");
                await RunTestScriptNoArgConvAsync<long?>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOLongAsync), "", "null");
                await RunTestScriptNoArgConvAsync<string>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOStringAsync), "", "null");
                await RunTestScriptNoArgConvAsync<decimal?>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyODecimalAsync), "", "null");
                await RunTestScriptNoArgConvAsync<float?>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOFloatAsync), "", "null");
                await RunTestScriptNoArgConvAsync<double?>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyODecimalAsync), "", "null");
                await RunTestScriptNoArgConvAsync<DateTime?>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyODateTimeAsync), "", "null");
                await RunTestScriptNoArgConvAsync<DateTimeOffset?>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyODateTimeOffsetAsync), "", "null");
                await RunTestScriptNoArgConvAsync<Guid?>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOGuidAsync), "", "null");
                await RunTestScriptNoArgConvAsync<Uri>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOUriAsync), "", "null");

                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyBooleanAsync), (bool)true, "true");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyBooleanAsync), (bool)false, "false");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyByteAsync), (byte)17, "17");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyShortAsync), (short)11, "11");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyIntegerAsync), (int)4711, "4711");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyLongAsync), (long)4711, "4711");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyStringAsync), (string)"My string", "\"My string\"");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyDecimalAsync), (decimal)1.43m, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyFloatAsync), (float)1.43f, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyDoubleAsync), (double)1.43d, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyDateTimeAsync), dt, $"\"{dtu:yyyy-MM-ddTHH:mm:ss.000Z}\"", (i,r) => Assert.AreEqual(dtu, r));
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyDateTimeOffsetAsync), dto, $"\"{dtou:yyyy-MM-ddTHH:mm:ss.000Z}\"", (i, r) => Assert.AreEqual(dtou, r));
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyGuidAsync), guid, $"\"{guid}\"");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyUriAsync), uri, $"\"{uri}\"");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyDictionaryAsync), dict, $"{{\"key\":\"value\"}}");

                await RunTestScriptNoArgConvAsync<ComplexType>(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyComplexTypeAsync), $"new x.TypeScriptTest.ComplexType({strCt})", "{\"Integer\":123,\"String\":\"test string\",\"StringArr\":[\"test\"]}");

                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOBooleanAsync), (bool)true, "true");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOBooleanAsync), (bool)false, "false");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOByteAsync), (byte)17, "17");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOShortAsync), (short)11, "11");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOIntegerAsync), (int)4711, "4711");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOLongAsync), (long)4711, "4711");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOStringAsync), (string)"My string", "\"My string\"");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyODecimalAsync), (decimal)1.43m, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOFloatAsync), (float)1.43f, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyODoubleAsync), (double)1.43d, "1.43");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyODateTimeAsync), dt, $"\"{dtu:yyyy-MM-ddTHH:mm:ss.000Z}\"", (i, r) => Assert.AreEqual(dtu, r));
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyODateTimeOffsetAsync), dto, $"\"{dtou:yyyy-MM-ddTHH:mm:ss.000Z}\"", (i, r) => Assert.AreEqual(dtou, r));
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOGuidAsync), guid, $"\"{guid}\"");
                await RunTestScriptAsync(ctx.ClientServiceProvider, packages, nameof(ITestInterface.ProxyOUriAsync), uri, $"\"{uri}\"");

            }

        }

        private async Task RunTestScriptAsync<T>(IServiceProvider sp, IEnumerable<NpmPackage> packages, string methodName, T input, string jsonText, Action<T,T> comparer = null)
        {
            if (comparer == null) comparer = (i, r) => Assert.AreEqual(i, r);
            var sf = sp.GetRequiredService<ISerializerFactory>();
            sf.SerializeToString(out string jsInput, input);
            var res = await RunTestScriptNoArgConvAsync<T>(sp, packages, methodName, jsInput, jsonText);
            comparer(input, res);
        }

        private async Task<T> RunTestScriptNoArgConvAsync<T>(IServiceProvider sp, IEnumerable<NpmPackage> packages, string methodName, string jsInput, string jsonText)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(new TimeSpan(0, 0, 30));

            var ns = sp.GetRequiredService<INodeService>();
            var js = $@"
const x = require(""solidrpc-tests"");
const y = require(""solidrpcjs"");
y.SolidRpcJs.resetPreFlight();
y.SolidRpcJs.addPreFlight((req, cont) => {{ 
    req.headers['Authorization'] = 'Bearer mykey';

    console.log(req.method + ':ing data @ ' + req.uri);
    if (req.query) {{ Object.keys(req.query).forEach(k => console.log(' Query  - ' + k + ':' + req.query[k])); }}
    if (req.headers) {{ Object.keys(req.headers).forEach(k => console.log(' Header - ' + k + ':' + req.headers[k])); }}
    if (req.data) {{ console.log('Data - ' + req.data); }}

    cont();
}});
(async function(){{
  console.log('Invoking...');
  let res = await x.TypeScriptTest.ITestInterface.{methodName}({jsInput}).invoke().toPromise();
  console.log('...done');
  return res;
}})();";
            var sep = Path.DirectorySeparatorChar;
            var inputFiles = packages.SelectMany(p => p.Files.Select(f => new { p.Name, File = f }))
                .Select(o => new NodeExecutionFile()
                {
                    FileName = $"node_modules{sep}{o.Name}{sep}{o.File.FilePath}",
                    Content = new MemoryStream(Encoding.UTF8.GetBytes(o.File.Content))
                }).ToList();
            var nodeRes = await ns.ExecuteScriptAsync(new NodeExecutionInput()
            {
                Js = js,
                InputFiles = inputFiles,
                ModuleId = NodeModuleRpcResolver.GuidModuleId
            }, cts.Token);
            if (!string.IsNullOrEmpty(nodeRes.Out)) Console.Write("NodeOut:" + nodeRes.Out);
            if (!string.IsNullOrEmpty(nodeRes.Err) && nodeRes.ExitCode == 0) Console.Write("NodeErr:" + nodeRes.Err);

            if (nodeRes.ExitCode != 0)
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

        private async Task<IEnumerable<NpmPackage>> CreatePackage(IServiceProvider sp, string assemblyName)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(new TimeSpan(0, 1, 0));

            var ng = sp.GetRequiredService<INpmGenerator>();
            var packages = await ng.CreateNpmPackage(new[] { assemblyName }, cts.Token);

            foreach(var package in packages)
            {
                foreach(var file in package.Files)
                {
                    await WriteModuleFile(package.Name, file.FilePath, file.Content);
                }
            }
            return packages;
        }

        private Task WriteModuleFile(string packageName, string fileName, string fileContent)
        {
            return WritePackageFile($"generated\\{packageName}\\{fileName}", fileContent);
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
    }
}
