using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.OpenApi.DotNetTool;
using System;
using System.Collections.Generic;
using System.IO;
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
        [Test,Ignore("Works on Windows")]
        public void TestCodeGen()
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger", "CodeGen");
            var dir = new DirectoryInfo(path);
            Assert.IsTrue(dir.Exists);
            foreach(var subDir in dir.GetDirectories())
            {
                if (subDir.Name == "SecurityPermissionAttribute") continue;
                CreateCode(subDir, true);
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

                // await proxy.DeletePet(api_key, pet.Id);
                ctx.CreateServerInterceptor<UrlParam.Services.IUrlParam>(
                    o => o.ProxyIntegerInPath(1, CancellationToken.None),
                    config,
                    args =>
                    {
                        CompareStructs(3, args[0]);
                        CompareStructs(CancellationToken.None, args[1]);
                        return Task.FromResult((int)args[0]);
                    });


                ctx.AddOpenApiProxy<UrlParam.Services.IUrlParam>(config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<UrlParam.Services.IUrlParam>();

                await proxy.ProxyIntegerInPath(3);
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

                // await proxy.DeletePet(api_key, pet.Id);
                ctx.CreateServerInterceptor<ArrayParam.Services.IArrayParam>(
                    o => o.ProxyArrayInQuery(null, CancellationToken.None),
                    config,
                    args =>
                    {
                        CompareStructs(new int[] { 5, 9 }, args[0]);
                        CompareStructs(CancellationToken.None, args[1]);
                        return Task.FromResult<IEnumerable<int>>((int[])args[0]);
                    });


                ctx.AddOpenApiProxy<ArrayParam.Services.IArrayParam>(config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<ArrayParam.Services.IArrayParam>();

                await proxy.ProxyArrayInQuery(new int[] { 5, 9});
            }
        }
    }
}
