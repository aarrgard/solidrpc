using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SolidRpc.OpenApi.DotNetTool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGen
{
    /// <summary>
    /// Tests swagger functionality.
    /// </summary>
    public class SpecGenTest : WebHostTest
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
                CreateSpec(subDir.Name, false);
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

                var moq = new Mock<FileUpload1.Services.IFileUpload>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<FileUpload1.Services.IFileUpload>();

                moq.Setup(o => o.UploadFile(
                    It.Is<Stream>(a => CompareStructs(a, new MemoryStream(new byte[] { 0, 1, 2, 3 }))),
                    It.Is<string>(a => a == "filename.txt"),
                    It.Is<string>(a => a == "application/pdf"),
                    It.IsAny<CancellationToken>()
                    )).Returns(Task.CompletedTask);

                await proxy.UploadFile(
                    new MemoryStream(new byte[] { 0, 1, 2, 3 }), 
                    "filename.txt", 
                    "application/pdf");

                Assert.AreEqual(1, moq.Invocations.Count);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestFileUpload2()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestFileUpload2).Substring(4));

                var moq = new Mock<FileUpload2.Services.IFileUpload>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();

                moq.Setup(o => o.UploadFile(
                    It.Is<FileUpload2.Types.FileData>(a => CompareStructs(a, CreateUpload2Struct())),
                    It.IsAny<CancellationToken>()
                    )).Returns(Task.FromResult(CreateUpload2Struct()));


                var proxy = ctx.ClientServiceProvider.GetRequiredService<FileUpload2.Services.IFileUpload>();
                var res = await proxy.UploadFile(CreateUpload2Struct());

                CompareStructs(CreateUpload2Struct(), res);
            }
        }

        private FileUpload2.Types.FileData CreateUpload2Struct()
        {
            return new FileUpload2.Types.FileData()
            {
                FileContent = new MemoryStream(new byte[] { 0, 1, 2, 3 }),
                FileName = "filename.txt",
                ContentType = "application/pdf"
            };     
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

                var moq = new Mock<OneComplexArg.Services.IOneComplexArg>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                moq.Setup(o => o.GetComplexType(
                    It.Is<OneComplexArg.Types.ComplexType1>(a => CompareStructs(a, new OneComplexArg.Types.ComplexType1()))
                    )).Returns(new OneComplexArg.Types.ComplexType1());

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<OneComplexArg.Services.IOneComplexArg>();
                var res = proxy.GetComplexType(new OneComplexArg.Types.ComplexType1());
                CompareStructs(new OneComplexArg.Types.ComplexType1(), res);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test, Ignore("Finish up!")]
        public async Task TestMoreComplexArgs()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestMoreComplexArgs).Substring(4));

                var moq = new Mock<MoreComplexArgs.Services.IMoreComplexArgs>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                var ct1Arr = (IEnumerable<MoreComplexArgs.Types.ComplexType1>)new[] { new MoreComplexArgs.Types.ComplexType1() {
                    CT2 = new MoreComplexArgs.Types.ComplexType2()
                } };
                var ct2Arr = (IEnumerable<MoreComplexArgs.Types.ComplexType2>)new[] { new MoreComplexArgs.Types.ComplexType2() {
                    CT1 = new MoreComplexArgs.Types.ComplexType1()
                } };

                moq.Setup(o => o.GetComplexType(
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.Is<IEnumerable<MoreComplexArgs.Types.ComplexType1>>(a => CompareTypedStructs(ct1Arr, a)),
                    It.Is<IEnumerable<MoreComplexArgs.Types.ComplexType2>>(a => CompareTypedStructs(ct2Arr, a))
                    )).Returns("test");

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<MoreComplexArgs.Services.IMoreComplexArgs>();
                var res = proxy.GetComplexType(
                    "test",
                    DateTime.Now,
                    ct1Arr,
                    ct2Arr
                    );
                Assert.AreEqual("test", res);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestNullableTypes()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestNullableTypes).Substring(4));

                var moq = new Mock<NullableTypes.Services.INullableTypes>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                //ctx.CreateServerInterceptor<NullableTypes.Services.INullableTypes>(
                //    o => o.GetComplexType(null),
                //    config,
                //    args =>
                //    {
                //        Assert.AreEqual(1, args.Length);
                //        Assert.IsNotNull((NullableTypes.Types.ComplexType)args[0]);
                //        return (NullableTypes.Types.ComplexType)args[0];
                //    });
                //ctx.AddOpenApiProxy<NullableTypes.Services.INullableTypes>(config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<NullableTypes.Services.INullableTypes>();

                moq.Setup(o => o.GetComplexType(It.Is<NullableTypes.Types.ComplexType>(a => a.NullableInt == null))).Returns(() =>
                {
                    return new NullableTypes.Types.ComplexType();
                });
                var res = proxy.GetComplexType(new NullableTypes.Types.ComplexType());
                Assert.IsNull(res.NullableInt);
                Assert.IsNull(res.NullableLong);

                moq.Setup(o => o.GetComplexType(It.Is<NullableTypes.Types.ComplexType>(a => a.NullableInt.HasValue))).Returns(() =>
                {
                    return new NullableTypes.Types.ComplexType()
                    {
                        NullableInt = 1,
                        NullableLong = 1
                    };
                });
                res = proxy.GetComplexType(new NullableTypes.Types.ComplexType()
                {
                    NullableInt = 1,
                    NullableLong = 1
                });
                Assert.IsNotNull(res.NullableInt);
                Assert.IsNotNull(res.NullableLong);
            }
        }
    }
}
