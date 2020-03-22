using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
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
                //if (subDir.Name != "FileUpload4") continue;
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
        public async Task TestEnumArgs()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestEnumArgs).Substring(4));

                var moq = new Mock<EnumArgs.Services.IEnumArgs>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<EnumArgs.Services.IEnumArgs>();

                moq.Setup(o => o.GetEnumTypeAsync(
                    It.IsAny<EnumArgs.Types.TestEnum>(),
                    It.IsAny<CancellationToken>()
                    )).Returns(Task.FromResult(EnumArgs.Types.TestEnum.Two));

                var res = await proxy.GetEnumTypeAsync(EnumArgs.Types.TestEnum.Two);
                Assert.AreEqual(EnumArgs.Types.TestEnum.Two, res);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestStringValuesArg()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestStringValuesArg).Substring(4));

                var moq = new Mock<StringValuesArg.Services.IStringValuesArg>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<StringValuesArg.Services.IStringValuesArg>();

                var stringValues = new StringValues(new[] { "test1", "test2" });
                var complexType = new StringValuesArg.Types.ComplexType() { StringValues = stringValues };
                moq.Setup(o => o.GetStringValueArgs(
                    It.Is<StringValues>(a => a == stringValues),
                    It.Is<StringValuesArg.Types.ComplexType>(a => CompareStructs(complexType, a))
                    )).Returns(complexType);

                var res = proxy.GetStringValueArgs(stringValues, complexType);
                CompareStructs(complexType, res);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestHttpRequestArgs()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestHttpRequestArgs).Substring(4));

                var moq = new Mock<HttpRequestArgs.Services.IHttpRequestArgs>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<HttpRequestArgs.Services.IHttpRequestArgs>();

                var stringValues = new StringValues(new[] { "test1", "test2" });
                var req = new HttpRequestArgs.Types.HttpRequest() { 
                    Uri = new Uri("http://dummy.site/test") 
                };
                moq.Setup(o => o.TestInvokeRequest(
                    It.IsAny<HttpRequestArgs.Types.HttpRequest>(),
                    It.IsAny<CancellationToken>()
                    )).Returns(Task.FromResult(req));

                var res = await proxy.TestInvokeRequest(null);
                CompareStructs(req, res);
            }
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestDictionaryArg()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestDictionaryArg).Substring(4));

                var moq = new Mock<DictionaryArg.Services.IDictionaryArg>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<DictionaryArg.Services.IDictionaryArg>();

                var dictArg = new Dictionary<string, DictionaryArg.Types.ComplexType>()
                {
                    { "test1", new DictionaryArg.Types.ComplexType() {
                        ComplexTypes = new Dictionary<string, DictionaryArg.Types.ComplexType>()
                        {
                            { "test3", new DictionaryArg.Types.ComplexType() }
                        }
                    }},
                    { "test2", new DictionaryArg.Types.ComplexType() }
                };

                moq.Setup(o => o.GetDictionaryValues(
                    It.Is<IDictionary<string, DictionaryArg.Types.ComplexType>>(a => CompareStructs(dictArg, a))
                    )).Returns(dictArg);

                var res = proxy.GetDictionaryValues(dictArg);
                CompareStructs(dictArg, res);
            }
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
                    It.Is<Stream>(a => CompareStructs(new MemoryStream(new byte[] { 0, 1, 2, 3 }), a)),
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
                Content = new MemoryStream(new byte[] { 0, 1, 2, 3 }),
                FileName = "filename.txt",
                ContentType = "application/pdf"
            };
        }


        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestFileUpload3()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestFileUpload3).Substring(4));

                var moq = new Mock<FileUpload3.Services.IFileUpload>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();

                moq.Setup(o => o.UploadFile(
                    It.IsAny<string>(),
                    It.Is<FileUpload3.Types.FileData>(a => CompareStructs(a, CreateUpload3Struct())),
                    It.IsAny<CancellationToken>()
                    )).Returns(Task.FromResult(CreateUpload3Struct()));

                var proxy = ctx.ClientServiceProvider.GetRequiredService<FileUpload3.Services.IFileUpload>();
                var res = await proxy.UploadFile("", CreateUpload3Struct());

                CompareStructs(CreateUpload3Struct(), res);
            }
        }

        private FileUpload3.Types.FileData CreateUpload3Struct()
        {
            return new FileUpload3.Types.FileData()
            {
                Content = new MemoryStream(new byte[] { 0, 1, 2, 3 }),
                FileName = "filename.txt",
                ContentType = "application/pdf"
            };
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestFileUpload4()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestFileUpload4).Substring(4));

                var moq = new Mock<FileUpload4.Services.IFileUpload>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();

                var complexType = new FileUpload4.Types.ComplexType() { StringData = "MyData" };
                moq.Setup(o => o.UploadFile(
                    It.Is<FileUpload4.Types.ComplexType>(c => CompareStructs(c, complexType)),
                    It.Is<FileUpload4.Types.FileData>(a => CompareStructs(a, CreateUpload4Struct())),
                    It.IsAny<CancellationToken>()
                    )).Returns(Task.FromResult(CreateUpload4Struct()));

                var proxy = ctx.ClientServiceProvider.GetRequiredService<FileUpload4.Services.IFileUpload>();
                var res = await proxy.UploadFile(complexType, CreateUpload4Struct());

                CompareStructs(CreateUpload4Struct(), res);
            }
        }

        private FileUpload4.Types.FileData CreateUpload4Struct()
        {
            return new FileUpload4.Types.FileData()
            {
                Content = new MemoryStream(new byte[] { 0, 1, 2, 3 }),
                FileName = "filename.txt",
                ContentType = "application/pdf"
            };
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public async Task TestFileUpload5()
        {
            using (var ctx = CreateKestrelHostContext())
            {
                var config = ReadOpenApiConfiguration(nameof(TestFileUpload5).Substring(4));

                var moq = new Mock<FileUpload5.Services.IFileUpload>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();

                var complexType1 = new FileUpload5.Types.ComplexType() { StringData = "MyData" };
                var complexType2 = new FileUpload5.Types.ComplexType() { StringData = "MyData" };
                moq.Setup(o => o.UploadFile(
                    It.Is<FileUpload5.Types.ComplexType>(c => CompareStructs(c, complexType1)),
                    It.Is<FileUpload5.Types.ComplexType>(c => CompareStructs(c, complexType2)),
                    It.Is<FileUpload5.Types.FileData>(a => CompareStructs(a, CreateUpload5Struct())),
                    It.IsAny<CancellationToken>()
                    )).Returns(Task.FromResult(CreateUpload5Struct()));

                var proxy = ctx.ClientServiceProvider.GetRequiredService<FileUpload5.Services.IFileUpload>();
                var res = await proxy.UploadFile(complexType1, complexType2, CreateUpload5Struct());

                CompareStructs(CreateUpload5Struct(), res);
            }
        }

        private FileUpload5.Types.FileData CreateUpload5Struct()
        {
            return new FileUpload5.Types.FileData()
            {
                Content = new MemoryStream(new byte[] { 0, 1, 2, 3 }),
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
        [Test]
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
