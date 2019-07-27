using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidRpc.OpenApi.DotNetTool;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGen
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class SpecGenTest : SwaggerTestBase
    {
        /// <summary>
        /// Returns the spec folder
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected override DirectoryInfo GetSpecFolder(string folderName)
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "SpecGen");
            path = Path.Combine(path, folderName);
            return new DirectoryInfo(path);
        }

        /// <summary>
        /// Tests generating swagger file from code
        /// </summary>
        [Test]
        public void TestSpecGen()
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "SpecGen");
            var dir = new DirectoryInfo(path);
            Assert.IsTrue(dir.Exists);
            foreach(var subDir in dir.GetDirectories())
            {
                CreateSpec(subDir.Name, true);
            }
        }

        private void CreateSpec(string folderName, bool onlyCompare)
        {
            try
            {
                var dir = GetSpecGenFolder(folderName);
                var openApiFile = $"{dir.Name}.json";

                Program.MainWithExeptions(new[] {
                "-code2openapi",
                "-d", dir.FullName,
                "-only-compare", onlyCompare.ToString(),
                "-BasePath", $".{GetType().Assembly.GetName().Name}.Swagger.SpecGen.{dir.Name}".Replace('.','/'),
                "-ProjectNamespace", $"{GetType().Assembly.GetName().Name}.Swagger.SpecGen.{dir.Name}",
                openApiFile}).Wait();
            }
            catch(Exception e)
            {
                Console.Out.WriteLine(e);
                throw;
            }
       }

        private DirectoryInfo GetSpecGenFolder(string folderName)
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "Swagger");
            path = Path.Combine(path, "SpecGen");
            path = Path.Combine(path, folderName);
            var dir = new DirectoryInfo(path);
            if(!dir.Exists)
            {
                throw new ArgumentException("Cannot find path:" + folderName);
            }
            return dir;
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestFileUpload1()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestFileUpload1).Substring(4));
                ctx.CreateServerInterceptor<FileUpload1.Services.IFileUpload>(
                    o => o.UploadFile(null, null, null, CancellationToken.None),
                    config,
                    args =>
                    {
                        Assert.AreEqual(4, args.Length);
                        Assert.AreEqual(new byte[] { 0, 1, 2, 3 }, ((MemoryStream)args[0]).ToArray());
                        Assert.AreEqual("filename.txt", args[1]);
                        Assert.AreEqual("application/pdf", args[2]);
                        Assert.IsNotNull((CancellationToken)args[3]);
                        return Task.CompletedTask;
                    });
                ctx.AddOpenApiProxy<FileUpload1.Services.IFileUpload>(config);
                await ctx.StartAsync();
                var proxy = ctx.ServiceProvider.GetRequiredService<FileUpload1.Services.IFileUpload>();
                await proxy.UploadFile(new MemoryStream(new byte[] { 0, 1, 2, 3 }), "filename.txt", "application/pdf");
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestOneComplexArg()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestOneComplexArg).Substring(4));
                ctx.CreateServerInterceptor<OneComplexArg.Services.IOneComplexArg>(
                    o => o.GetComplexType(null, null),
                    config,
                    args =>
                    {
                        Assert.AreEqual(2, args.Length);
                        Assert.AreEqual("test", args[0]);
                        Assert.IsNotNull((OneComplexArg.Types.ComplexType1)args[1]);
                        return (OneComplexArg.Types.ComplexType1)args[1];
                    });
                ctx.AddOpenApiProxy<OneComplexArg.Services.IOneComplexArg>(config);
                await ctx.StartAsync();
                var proxy = ctx.ServiceProvider.GetRequiredService<OneComplexArg.Services.IOneComplexArg>();
                var res = proxy.GetComplexType("test", new OneComplexArg.Types.ComplexType1());
            }
        }
    }
}
