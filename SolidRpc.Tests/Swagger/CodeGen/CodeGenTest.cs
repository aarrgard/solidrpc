﻿using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.DotNetTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.CodeGen
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class CodeGenTest : WebHostMvcTest
    {
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
        /// Tests generating code from a swagger file
        /// </summary>
        [Test]
        public async Task TestCodeGenLocal()
        {
            string swaggerSpec;
            using (var ctx = await StartKestrelHostContextAsync())
            {
                var resp = await ctx.GetResponse("/swagger/v1/swagger.json");
                swaggerSpec = await AssertOk(resp);
           }

            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger", "CodeGen", "Local", "swagger.json");
            var fi = new FileInfo(path);
            using (var tw = fi.CreateText())
            {
                await tw.WriteAsync(swaggerSpec);
            }           
        }

        /// <summary>
        /// Tests generating code from a swagger file
        /// </summary>
        [Test]
        public void TestCodeGen()
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger", "CodeGen");
            var dir = new DirectoryInfo(path);
            Assert.IsTrue(dir.Exists);
            foreach(var subDir in dir.GetDirectories())
            {
                if (subDir.Name == "SecurityPermissionAttribute") continue;
                CreateCode(subDir, false);
            }
        }

        private void CreateCode(DirectoryInfo dir, bool onlyCompare)
        {
            var openApiFiles = dir.GetFiles("*.json");
            if(openApiFiles.Length != 1)
            {
                Assert.Fail("Cannot find json file in folder:" + dir.FullName);
            }
            try
            {
                Program.MainWithExeptions(new[] {
                "-openapi2code",
                "-d", dir.FullName,
                "-only-compare", onlyCompare.ToString(),
                "-ProjectNamespace", $"{GetType().Assembly.GetName().Name}.Swagger.CodeGen.{dir.Name}",
                openApiFiles[0].Name }).Wait();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
       }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestUrlParam()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestUrlParam).Substring(4));


                var moq = new Moq.Mock<UrlParam.Services.IUrlParam>();
                moq.Setup(o => o.ProxyIntegerInPath(It.Is<int>(val => val == 3), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(3));

                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<UrlParam.Services.IUrlParam>();

                var res = await proxy.ProxyIntegerInPath(3);
                Assert.AreEqual(3, res);
                Assert.AreEqual(1, moq.Invocations.Count);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestArrayParam()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestArrayParam).Substring(4));

                var moq = new Moq.Mock<ArrayParam.Services.IArrayParam>();
                moq.Setup(o => o.ProxyArrayInQuery(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult<IEnumerable<int>>(new[] { 5, 9 }));

                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<ArrayParam.Services.IArrayParam>();

                var res = await proxy.ProxyArrayInQuery(new int[] { 5, 9 });
                Assert.AreEqual(5, res.Skip(0).First());
                Assert.AreEqual(9, res.Skip(1).First());
                Assert.AreEqual(1, moq.Invocations.Count);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestRequiredOptionalParam()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestRequiredOptionalParam).Substring(4));

                var moq = new Moq.Mock<RequiredOptionalParam.Services.IRequiredOptionalParam>();

                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<RequiredOptionalParam.Services.IRequiredOptionalParam>();

                moq.Setup(o => o.OptionalInt(It.Is<int?>(i => i == 1), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult<int>(1));
                var res = await proxy.OptionalInt(1);
                Assert.AreEqual(1, res);

                moq.Setup(o => o.OptionalInt(It.Is<int?>(i => i == null), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult<int>(0));
                res = await proxy.OptionalInt(null);
                Assert.AreEqual(0, res);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestTwoOpsOneMeth()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestTwoOpsOneMeth).Substring(4));

                var moq = new Moq.Mock<TwoOpsOneMeth.Services.ITwoOpsOneMeth>();

                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<TwoOpsOneMeth.Services.ITwoOpsOneMeth>();

                moq.Setup(o => o.TwoOpsOneMeth(
                    It.Is<int>(i => i == 1), 
                    It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult<int>(1));
                var res = await proxy.TwoOpsOneMeth(1);
                Assert.AreEqual(1, res);

                var url = await ctx.ClientServiceProvider
                    .GetRequiredService<IInvoker<TwoOpsOneMeth.Services.ITwoOpsOneMeth>>()
                    .GetUriAsync(o => o.TwoOpsOneMeth(4711, CancellationToken.None));
                var httpClientFact = ctx.ClientServiceProvider
                    .GetRequiredService<IHttpClientFactory>();
                var client = httpClientFact.CreateClient();


                moq.Setup(o => o.TwoOpsOneMeth(
                     It.Is<int>(i => i == 4711),
                     It.IsAny<CancellationToken>()))
                     .Returns(Task.FromResult<int>(4711));
                var getReq = await client.GetAsync(url);
                Assert.AreEqual(HttpStatusCode.OK, getReq.StatusCode);
                Assert.AreEqual("4711", await getReq.Content.ReadAsStringAsync());

                moq.Setup(o => o.TwoOpsOneMeth(
                     It.Is<int>(i => i == 4711),
                     It.IsAny<CancellationToken>()))
                     .Returns(Task.FromResult<int>(4711));
                var postReq = await client.PostAsync(url, new StringContent(""));
                Assert.AreEqual(HttpStatusCode.OK, postReq.StatusCode);
                Assert.AreEqual("4711", await postReq.Content.ReadAsStringAsync());
                
                var deleteReq = await client.DeleteAsync(url);
                Assert.AreEqual(HttpStatusCode.NotFound, deleteReq.StatusCode);
            }
        }

        
    }
}
