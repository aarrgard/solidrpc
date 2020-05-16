using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.DotNetTool;
using SolidRpc.Tests.Swagger.SpecGen.HttpRequestArgs.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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

        public async Task RunTestInContext(Func<TestHostContext, Task> testFunc)
        {
            using (var ctx = CreateKestrelHostContext())
            {
                await testFunc(ctx);
            }
            using (var ctx = CreateHttpMessageHandlerContext())
            {
                await testFunc(ctx);
            }
        }
        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestEnumArgs()
        {
            return RunTestInContext(async ctx =>
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
            });
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestStringValuesArg()
        {
            return RunTestInContext(async ctx =>
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
            });
        }

        public class HttpRequestArgsProxy : HttpRequestArgs.Services.IHttpRequestArgs
        {
            public Task<HttpRequest> ReturnNullRequest(CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<HttpRequest>(null);
            }

            public Task<HttpRequestArgs.Types.HttpRequest> TestInvokeRequest(HttpRequestArgs.Types.HttpRequest req, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(req);
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

                var serverStub = new HttpRequestArgsProxy();
                ctx.AddServerAndClientService<HttpRequestArgs.Services.IHttpRequestArgs>(serverStub, config);

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<HttpRequestArgs.Services.IHttpRequestArgs>();

                var res = await proxy.TestInvokeRequest(null);
                Assert.AreEqual("/SolidRpc/Tests/Swagger/SpecGen/HttpRequestArgs/Services/IHttpRequestArgs/TestInvokeRequest", res.Uri.AbsolutePath);
                Assert.AreEqual("GET", res.Method);

                var uri = await ctx.ClientServiceProvider
                    .GetRequiredService<IInvoker<HttpRequestArgs.Services.IHttpRequestArgs>>()
                    .GetUriAsync(o => o.TestInvokeRequest(null, CancellationToken.None));

                var httpClientFact = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFact.CreateClient();

                var getResp = await httpClient.GetAsync(uri);
                Assert.AreEqual(HttpStatusCode.OK, getResp.StatusCode);
                var content = await getResp.Content.ReadAsStringAsync();
                Assert.IsTrue(content.Contains("\"Method\":\"GET\""));

                var putResp = await httpClient.PutAsync(uri, new StringContent("test"));
                Assert.AreEqual(HttpStatusCode.OK, putResp.StatusCode);
                content = await putResp.Content.ReadAsStringAsync();
                Assert.IsTrue(content.Contains("\"Method\":\"PUT\""));

                var postResp = await httpClient.PostAsync(uri, new StringContent("test"));
                Assert.AreEqual(HttpStatusCode.OK, postResp.StatusCode);
                content = await postResp.Content.ReadAsStringAsync();
                Assert.IsTrue(content.Contains("\"Method\":\"POST\""));

                var deleteResp = await httpClient.DeleteAsync(uri);
                Assert.AreEqual(HttpStatusCode.OK, deleteResp.StatusCode);
                content = await deleteResp.Content.ReadAsStringAsync();
                Assert.IsTrue(content.Contains("\"Method\":\"DELETE\""));

                //
                // Null request
                //
                var nullRequestUri = await ctx.ClientServiceProvider
                    .GetRequiredService<IInvoker<HttpRequestArgs.Services.IHttpRequestArgs>>()
                    .GetUriAsync(o => o.ReturnNullRequest(CancellationToken.None));

                var nullResp = await httpClient.GetAsync(nullRequestUri);
                Assert.AreEqual(HttpStatusCode.OK, nullResp.StatusCode);
                content = await nullResp.Content.ReadAsStringAsync();
                Assert.IsTrue(content.Contains("null"));

            };
        }
        
        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestDictionaryArg()
        {
            return RunTestInContext(async ctx =>
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
            });
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestFileUpload1()
        {
            return RunTestInContext(async ctx =>
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
            });
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

                moq.Setup(o => o.NullData(
                    It.IsAny<CancellationToken>()
                    )).Returns(Task.FromResult<FileUpload2.Types.FileData>(null));
                res = await proxy.NullData();
                Assert.IsNull(res);
            }
        }

        private FileUpload2.Types.FileData CreateUpload2Struct()
        {
            var enc = Encoding.UTF8;
            return new FileUpload2.Types.FileData()
            {
                Content = new MemoryStream(enc.GetBytes("{\"test\":\"test\"}")),
                CharSet = enc.HeaderName,
                FileName = "filename.txt",
                ContentType = "application/json"
            };
        }


        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestFileUpload3()
        {
            return RunTestInContext(async ctx =>
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
            });
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
        public Task TestFileUpload4()
        {
            return RunTestInContext(async ctx =>
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
            });
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
        public Task TestFileUpload5()
        {
            return RunTestInContext(async ctx =>
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
            });
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
        public Task TestOneComplexArg()
        {
            return RunTestInContext(async ctx =>
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
            });
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestMoreComplexArgs()
        {
            return RunTestInContext(async ctx =>
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
            });
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestNullableTypes()
        {
            return RunTestInContext(async ctx =>
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
            });
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestETagArg()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestETagArg).Substring(4));


                var moq = new Mock<ETagArg.Services.IETagArg>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<ETagArg.Services.IETagArg>();

                moq.Setup(o => o.GetEtagStruct(It.Is<ETagArg.Types.FileType>(a => a.ETag == "ETag"))).Returns(() =>
                {
                    return CreateFileStruct();
                });
                var res = proxy.GetEtagStruct(CreateFileStruct());
                CompareStructs(CreateFileStruct(), res);
            });
        }

        private ETagArg.Types.FileType CreateFileStruct()
        {
            return new ETagArg.Types.FileType()
            {
                Content = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 }),
                ETag = "ETag"
            };
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestLastModifiedArg()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestLastModifiedArg).Substring(4));


                var moq = new Mock<LastModifiedArg.Services.ILastModifiedArg>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<LastModifiedArg.Services.ILastModifiedArg>();
                var lastModified = DateTimeOffset.ParseExact("2020-04-24 21:13:44", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                moq.Setup(o => o.GetLastModifiedStruct(It.Is<LastModifiedArg.Types.FileType>(a => a.LastModified == lastModified))).Returns(() =>
                {
                    return CreateTestLastModifiedArgFileStruct(lastModified);
                });
                var res = proxy.GetLastModifiedStruct(CreateTestLastModifiedArgFileStruct(lastModified));
                CompareStructs(CreateTestLastModifiedArgFileStruct(lastModified), res);
            });
        }

        private LastModifiedArg.Types.FileType CreateTestLastModifiedArgFileStruct(DateTimeOffset lastModified)
        {
            return new LastModifiedArg.Types.FileType()
            {
                Content = new MemoryStream(new byte[] { 1,2,3,4,5}),
                LastModified = lastModified
            };
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestUrlEncodeArg()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestUrlEncodeArg).Substring(4));

                var moq = new Mock<UrlEncodeArg.Services.IUrlEncodeArg>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<UrlEncodeArg.Services.IUrlEncodeArg>();

                var stringCheckEmpty = "";
                var stringCheckFixed = "test";
                moq.Setup(o => o.ProxyStrings(It.Is<string>(a => a == stringCheckEmpty), It.Is<string>(a => a == stringCheckFixed))).Returns(() => stringCheckFixed);
                Assert.AreEqual(stringCheckFixed, proxy.ProxyStrings(stringCheckEmpty, stringCheckFixed));

                var stringCheck0 = "";
                moq.Setup(o => o.ProxyStrings(It.Is<string>(a => a == stringCheck0), It.Is<string>(a => a == stringCheck0))).Returns(() => stringCheck0);
                Assert.AreEqual(stringCheck0, proxy.ProxyStrings(stringCheck0, stringCheck0));

                var stringCheck1 = "one/one";
                moq.Setup(o => o.ProxyStrings(It.Is<string>(a => a == stringCheck1), It.Is<string>(a => a == stringCheck1))).Returns(() => stringCheck1);
                Assert.AreEqual(stringCheck1, proxy.ProxyStrings(stringCheck1, stringCheck1));

                var stringCheck2 = "one+one";
                moq.Setup(o => o.ProxyStrings(It.Is<string>(a => a == stringCheck2), It.Is<string>(a => a == stringCheck2))).Returns(() => stringCheck2);
                Assert.AreEqual(stringCheck2, proxy.ProxyStrings(stringCheck2, stringCheck2));

                var stringCheck3 = "one()";
                moq.Setup(o => o.ProxyStrings(It.Is<string>(a => a == stringCheck3), It.Is<string>(a => a == stringCheck3))).Returns(() => stringCheck3);
                Assert.AreEqual(stringCheck3, proxy.ProxyStrings(stringCheck3, stringCheck3));

                var stringCheck4 = "one[]";
                moq.Setup(o => o.ProxyStrings(It.Is<string>(a => a == stringCheck4), It.Is<string>(a => a == stringCheck4))).Returns(() => stringCheck4);
                Assert.AreEqual(stringCheck4, proxy.ProxyStrings(stringCheck4, stringCheck4));

                var stringCheck5 = "one{}";
                moq.Setup(o => o.ProxyStrings(It.Is<string>(a => a == stringCheck5), It.Is<string>(a => a == stringCheck5))).Returns(() => stringCheck5);
                Assert.AreEqual(stringCheck5, proxy.ProxyStrings(stringCheck5, stringCheck5));

                var stringCheck6 = "one<>";
                moq.Setup(o => o.ProxyStrings(It.Is<string>(a => a == stringCheck6), It.Is<string>(a => a == stringCheck6))).Returns(() => stringCheck6);
                Assert.AreEqual(stringCheck6, proxy.ProxyStrings(stringCheck6, stringCheck6));

                var stringCheck7 = "one%%";
                moq.Setup(o => o.ProxyStrings(It.Is<string>(a => a == stringCheck7), It.Is<string>(a => a == stringCheck7))).Returns(() => stringCheck7);
                Assert.AreEqual(stringCheck7, proxy.ProxyStrings(stringCheck7, stringCheck7));
            });
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestUrlAndQueryArgs()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestUrlAndQueryArgs).Substring(4));

                var moq = new Mock<TestUrlAndQueryArgs.Services.IUrlAndQueryArgs>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<TestUrlAndQueryArgs.Services.IUrlAndQueryArgs>();

                var stringCheck1 = "one/one";
                var stringCheck2 = "one+one";
                moq.Setup(o => o.DoSometingAsync(It.Is<string>(a => a == stringCheck1), It.Is<string>(a => a == stringCheck2))).Returns(() => Task.CompletedTask);
                await proxy.DoSometingAsync(stringCheck1, stringCheck2);
            });
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestDateTimeArg()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestDateTimeArg).Substring(4));

                var moq = new Mock<DateTimeArg.Services.IDateTimeArg>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<DateTimeArg.Services.IDateTimeArg>();

                var dt = DateTimeOffset.Now;
                moq.Setup(o => o.ProxyDateTimeOffset(It.Is<DateTimeOffset>(a => CheckDate(dt, a)))).Returns(() => dt);
                var res = proxy.ProxyDateTimeOffset(dt);

                // check other formats
                if(ctx is TestHostContextKestrel)
                {
                    var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<DateTimeArg.Services.IDateTimeArg>>();
                    var path = await invoker.GetUriAsync(o => o.ProxyDateTimeOffset(DateTimeOffset.MinValue));
                    var searchPattern = HttpUtility.UrlEncode(DateTimeOffset.MinValue.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture));

                    var clientFactory = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>();
                    var httpClient = clientFactory.CreateClient();

                    dt = DateTimeOffset.MinValue;
                    moq.Setup(o => o.ProxyDateTimeOffset(It.Is<DateTimeOffset>(a => CheckDate(dt, a)))).Returns(() => dt);
                    var strRes = await httpClient.GetStringAsync(path);
                    Assert.AreEqual("\"0001-01-01T00:00:00+00:00\"", strRes);

                    var strDate = "2020-05-04";
                    dt = DateTimeOffset.ParseExact(strDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    moq.Setup(o => o.ProxyDateTimeOffset(It.Is<DateTimeOffset>(a => CheckDate(dt, a)))).Returns(() => dt);
                    var newPath = path.ToString().Replace(searchPattern, strDate);
                    strRes = await httpClient.GetStringAsync(newPath);
                    Assert.AreEqual($"\"{dt.ToString("yyyy-MM-ddTHH:mm:sszzz")}\"", strRes);

                    strDate = "2020-05-04T12:12";
                    dt = DateTimeOffset.ParseExact(strDate, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    moq.Setup(o => o.ProxyDateTimeOffset(It.Is<DateTimeOffset>(a => CheckDate(dt, a)))).Returns(() => dt);
                    newPath = path.ToString().Replace(searchPattern, strDate);
                    strRes = await httpClient.GetStringAsync(newPath);
                    Assert.AreEqual($"\"{dt.ToString("yyyy-MM-ddTHH:mm:sszzz")}\"", strRes);

                }
            });
        }

        private bool CheckDate(DateTimeOffset wanted, DateTimeOffset sent)
        {
            return wanted.ToString() == sent.ToString();
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestRedirect()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestRedirect).Substring(4));

                var moq = new Mock<Redirect.Services.IRedirect>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<Redirect.Services.IRedirect>();

                var location = await ctx.ClientServiceProvider.GetRequiredService<IInvoker<Redirect.Services.IRedirect>>().GetUriAsync(o => o.Redirected());
                
                //moq.Setup(o => o.Redirect(It.Is<Redirect.Types.Redirect>(a => CompareStructs(a, CreateRedirect())))).Returns(() => CreateRedirect());
                moq.Setup(o => o.Redirect(It.IsAny<Redirect.Types.Redirect>())).Returns(() => CreateRedirect(location.ToString()));
                //moq.Setup(o => o.Redirected()).Returns(() => CreateRedirect(location.ToString()));
                CompareStructs(CreateRedirect(location.ToString()), proxy.Redirect(CreateRedirect(location.ToString())));
            });
        }

        private Redirect.Types.Redirect CreateRedirect(string location)
        {
            return new Redirect.Types.Redirect()
            {
                Location = location
            };
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestSameMethodNameNbrArgs()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestSameMethodNameNbrArgs).Substring(4));

                var moq = new Mock<SameMethodNameNbrArgs.Services.ISameMethodNameNbrArgs>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<SameMethodNameNbrArgs.Services.ISameMethodNameNbrArgs>();

                moq.Setup(o => o.ConsumeString(It.IsAny<string>()));
                proxy.ConsumeString("s1");

                moq.Setup(o => o.ConsumeString(It.IsAny<string>(), It.IsAny<string>()));
                proxy.ConsumeString("s1", "s2");
            });
        }
    }
}
