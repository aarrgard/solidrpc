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

namespace SolidRpc.Tests.CodeGenerator
{
    /// <summary>
    /// Tests the node services
    /// </summary>
    public class TypeScriptTest : WebHostTest
    {
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
                var tsg = ctx.ClientServiceProvider.GetRequiredService<ITypescriptGenerator>();
                var ts1 = await tsg.CreateTypesTsForAssemblyAsync("SolidRpc");
                var ts2 = await tsg.CreateTypesTsForAssemblyAsync(assemblyName);

                var ns = ctx.ClientServiceProvider.GetRequiredService<INodeService>();
                var sep = Path.DirectorySeparatorChar;
                var nodeRes = await ns.ExecuteScriptAsync(new NodeExecutionInput()
                {
                    Js = $"typescript{sep}lib{sep}tsc.js",
                    Args = new string[0],
                    InputFiles = new[] {
                        new NodeExecutionFile()
                        {
                            FileName = $"solidrpc.ts",
                            Content = new MemoryStream(Encoding.UTF8.GetBytes(ts1))
                        },
                        new NodeExecutionFile()
                        {
                            FileName = $"{assemblyName}.ts",
                            Content = new MemoryStream(Encoding.UTF8.GetBytes(ts2))
                        }
                    },
                    ModuleId = NodeModuleRpcResolver.GuidModuleId
                });

                Assert.AreEqual(0, nodeRes.ExitCode);
           }
            
        }
    }
}
