using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.OpenApi.Binder.Proxy;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Services;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.CodeGen
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class VitecTest : WebHostTest
    {
        /// <summary>
        /// 
        /// </summary>
        public VitecTest()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        private string OpenApiSpec => ReadOpenApiConfiguration(nameof(TestVitec).Substring(4));

        /// <summary>
        /// Returns the spec folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected override DirectoryInfo GetSpecFolder(string folderName)
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger", "CodeGen", folderName);
            return new DirectoryInfo(path);
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestVitecUsingKestrel()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await TestVitec(ctx);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestVitecUsingHttpMessageHandler()
        {
            using (var ctx = CreateHttpMessageHandlerContext())
            {
                await TestVitec(ctx);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        public async Task TestVitec(TestHostContext ctx)
        {
            var fileMock = new Mock<IFile>();
            ctx.AddServerAndClientService(fileMock.Object, OpenApiSpec);

            await ctx.StartAsync();

            var ms = (Stream)new MemoryStream(new byte[50]);
            var file = ctx.ClientServiceProvider.GetRequiredService<IFile>();
            fileMock.Setup(o => o.FileGetFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(ms));
            
            var res = await file.FileGetFile("customerId", "fileId", CancellationToken.None);
            Assert.AreEqual(ms.Length, res.Length);
        }

        /// <summary>
        /// Tests all the bindings
        /// </summary>
        [Test]
        public void CheckBindings()
        {
            //
            // check bindings
            //
            var openApiSpec = ReadOpenApiConfiguration(nameof(TestVitec).Substring(4));
            var sc = new ServiceCollection();
            sc.AddLogging(ConfigureLogging);
            sc.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
            sc.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
            sc.AddSolidRpcServices(o => { o.AddRpcHostServices = true; });
            var sp = sc.BuildServiceProvider();
            var mbs = sp.GetRequiredService<IMethodBinderStore>();
            GetType().Assembly.GetTypes()
                .Where(o => o.Namespace == typeof(IBusinessIntelligence).Namespace)
                .SelectMany(o => o.GetMethods())
                .ToList().ForEach(m =>
                {
                    var binding = mbs.CreateMethodBinding(openApiSpec, false, m);
                    Log($"Checking: {m.Name}->{binding.Method} {binding.Path}");
                });
        }
    }
}
