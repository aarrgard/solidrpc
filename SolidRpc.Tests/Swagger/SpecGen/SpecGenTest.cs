using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.Abstractions.Serialization;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.Binder.Http;
using SolidRpc.OpenApi.Binder.Invoker;
using SolidRpc.OpenApi.DotNetTool;
using SolidRpc.OpenApi.SwaggerUI.Services;
using SolidRpc.Tests.Swagger.SpecGen.HttpRequestArgs.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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
        /// Tests generating swagger file from code
        /// </summary>
        [Test]
        public void TestCreateOpenApiSpec()
        {
            CreateOpenApiSpec(typeof(ISolidRpcOAuth2).Assembly);
            CreateOpenApiSpec(typeof(ISwaggerUI).Assembly);
            //CreateOpenApiSpec("C:\\Development\\eo_devops\\sparse\\Vitec\\services\\Vitec\\EO.Vitec", "EO.Vitec");
        }

        private void CreateOpenApiSpec(Assembly assembly)
        {
            var path = GetProjectFolder(GetType().Assembly.GetName().Name).FullName;
            path = Path.Combine(path, "..");
            path = Path.Combine(path, assembly.GetName().Name);
            CreateOpenApiSpec(path, assembly.GetName().Name);
        }

        private void CreateOpenApiSpec(string path, string name)
        {
            var dir = new DirectoryInfo(path);
            Assert.IsTrue(dir.Exists);
            Program.MainWithExeptions(new[] {
                "-code2openapi",
                "-d", path,
                "-BasePath", $".{name}".Replace(".","/"),
                $"{name}.json"}).Wait();
        }

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
            foreach (var subDir in dir.GetDirectories())
            {
                //if (subDir.Name != "ByteArrArgs") continue;
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
        /// Runs the test function in a hosting context
        /// </summary>
        /// <param name="testFunc"></param>
        /// <returns></returns>
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

                var invoc = ctx.ClientServiceProvider.GetRequiredService<IInvoker<EnumArgs.Services.IEnumArgs>>();
                var uri = await invoc.GetUriAsync(o => o.GetEnumTypeAsync(EnumArgs.Types.TestEnum.Two, CancellationToken.None));
                Assert.AreEqual("/SolidRpc/Tests/Swagger/SpecGen/EnumArgs/Services/IEnumArgs/GetEnumTypeAsync/Two", uri.AbsolutePath);

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
                    It.Is<StringValues>(a => CompareStructs(stringValues, a)),
                    It.Is<StringValuesArg.Types.ComplexType>(a => CompareStructs(complexType, a))
                    )).Returns(complexType);

                var res = proxy.GetStringValueArgs(stringValues, complexType);
                CompareStructs(complexType, res);
            });
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestStringArrArgs()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestStringArrArgs).Substring(4));

                var moq = new Mock<StringArrArgs.Services.IStringArrArgs>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<StringArrArgs.Services.IStringArrArgs>();

                // array with elements
                var stringValues = new[] { "test1", "test2" }.AsEnumerable();
                moq.Setup(o => o.ProxyStringArray(
                    It.Is<IEnumerable<string>>(a => CompareTypedStructs(stringValues, a))
                    )).Returns(stringValues);

                var res = proxy.ProxyStringArray(stringValues);
                CompareTypedStructs(stringValues, res);

                // empty array 
                stringValues = new string[0];
                moq.Setup(o => o.ProxyStringArray(
                    It.Is<IEnumerable<string>>(a => CompareTypedStructs(stringValues, a))
                    )).Returns(stringValues);

                res = proxy.ProxyStringArray(stringValues);
                CompareTypedStructs(stringValues, res);
            });
        }

        /// <summary>
        /// A dummy implemtation
        /// </summary>
        public class HttpRequestArgsProxy : HttpRequestArgs.Services.IHttpRequestArgs
        {
            /// <summary>
            /// Implements the stub
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public Task<HttpRequest> ReturnNullRequest(CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<HttpRequest>(null);
            }
            /// <summary>
            /// Implements the stub
            /// </summary>
            /// <param name="req"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
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
                FileName = "filename.txt[]",
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
        public Task TestComplexAndSimpleArgs()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestComplexAndSimpleArgs).Substring(4));

                var moq = new Mock<ComplexAndSimpleArgs.Services.IComplexAndSimpleArgs>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<ComplexAndSimpleArgs.Services.IComplexAndSimpleArgs>();

                // Guid
                var g = Guid.NewGuid();
                moq.Setup(o => o.GetGuid(
                    It.Is<Guid>(a => CompareStructs(g, a))
                    )).Returns(g);
                var gres = proxy.GetGuid(g);
                CompareStructs(g, gres);

                // decimal
                var d = 10.3m;
                moq.Setup(o => o.GetDecimal(
                    It.Is<Decimal>(a => CompareStructs(d, a))
                    )).Returns(d);
                var dres = proxy.GetDecimal(d);
                CompareStructs(d, dres);

                // integer
                var i = 11;
                moq.Setup(o => o.GetInteger(
                    It.Is<int>(a => CompareStructs(i, a))
                    )).Returns(i);
                var ires = proxy.GetInteger(i);
                CompareStructs(i, ires);

                // url
                var uri = new Uri("wss://test.ws/testing");
                moq.Setup(o => o.GetUri(
                    It.Is<Uri>(a => CompareStructs(uri, a))
                    )).Returns(uri);
                var urires = proxy.GetUri(uri);
                CompareStructs(uri, urires);

                // complex inheritance
                var complexType = new ComplexAndSimpleArgs.Types.ComplexType2() {
                    CT1 = new ComplexAndSimpleArgs.Types.ComplexType1(),
                    CT2 = new ComplexAndSimpleArgs.Types.ComplexType2() 
                };
                moq.Setup(o => o.GetSimpleAndComplexType(
                    It.Is<string>(s => s == "test"),
                    It.Is<ComplexAndSimpleArgs.Types.ComplexType2>(a => CompareStructs(complexType, a))
                    )).Returns(complexType);
                var ct1 = proxy.GetSimpleAndComplexType("test", complexType);
                CompareStructs(complexType, ct1);
            });
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestOperatorOverrides()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestOperatorOverrides).Substring(4));

                var moq = new Mock<OperatorOverrides.Services.IOperatorOverrides>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                moq.Setup(o => o.GetComplexType(
                    It.Is<OperatorOverrides.Types.ComplexType>(a => true)
                    )).Returns(new OperatorOverrides.Types.ComplexType());

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<OperatorOverrides.Services.IOperatorOverrides>();
                var res = proxy.GetComplexType(new OperatorOverrides.Types.ComplexType());
                Assert.AreEqual(new OperatorOverrides.Types.ComplexType(), res);
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

                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<ETagArg.Services.IETagArg>>();
                res = invoker.InvokeAsync(o => o.GetEtagStruct(CreateFileStruct()), opt => opt.SetTransport(HttpHandler.TransportType).AddPostInvokeCallback(resp =>
                {
                    return Task.CompletedTask;
                }));
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
                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<UrlEncodeArg.Services.IUrlEncodeArg>>();
                
                var stringChecks = new[]
                {
                    "",
                    "test",
                    "one/one",
                    "one+one",
                    "one()",
                    "one[]",
                    "one<>",
                    "one%%",
                    "one one"
                };

                foreach(var check in stringChecks)
                {
                    moq.Setup(o => o.ProxyStrings(It.Is<string>(a => a == check), It.Is<string>(a => a == check))).Returns(() => check);
                    Assert.AreEqual(check, proxy.ProxyStrings(check, check));
                    var uri = await invoker.GetUriAsync(o => o.ProxyStrings(check, check));
                    Assert.IsTrue(uri.PathAndQuery.EndsWith($"/{HttpUtility.UrlEncode(check)}/{HttpUtility.UrlEncode(check)}"));
                }

                //
                // byte array
                //
                var arr = new byte[1000];
                for(int i = 0; i < arr.Length; i++)
                {
                    arr[i] = (byte)i;
                }

                moq.Setup(o => o.ProxyByteArray(It.Is<byte[]>(a => CompareStructs(a,arr)))).Returns(() => arr);
                Assert.IsTrue(CompareStructs(arr, proxy.ProxyByteArray(arr)));

                //
                // raw post
                //
                if(ctx is TestHostContextKestrel)
                {
                    // standard base64 encoded
                    var b64 = "A991R2IAAAAALABodHRwczovL2xvY2FsaG9zdDo1MDAxL1NvbGlkUnBjL0Fic3RyYWN0aW9uczcAQnJva2VySW1hZ2U6ODljMWVmNjctNWY0Yy00MGE3LTkxODktYWRiNzE2MjlmMzM0OnIxMDB4MAABcvnFdKkOCb5wa57Ov1F8FYT8d1sgN1Z%2fGE74o93zIKWyyMe2i%2fKbKy2f%2fQ032YLv4Nl3HcpXHaeApsrmPRPtXQBVmTeHVgjVfaz2FQbe06vV9MSmKsgmxZs1tCAlO6kzsOsN7e9cTYL5TzQyAiFqWN7hXEICWdiI9mBk%2f%2fZ0LO1n32TVXyrMclEBqMU%2fLkrGoOhvTsWL353iOLeMtzoLz+4dYOeezZSz2Cyc9jEWhBHGsHn+ly%2foqTb8CzJfPobQPQSjmF225Q+BA0v3eGCaem0VKfP2Pav59WabTECl38DSlyaKzHcpTbh+q9F9hqrxP%2fGq2A6JOyT7EzgPTPRhUw==";
                    arr = Base64Url.Decode(HttpUtility.UrlDecode(b64));
                    var newArr = Base64Url.Encode(arr);
                    var url = (await invoker.GetUriAsync(o => o.ProxyByteArray(new byte[0]))).ToString();
                    var resp = await ctx.HttpClient.GetAsync($"{url}{b64}");
                    Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);

                    // base64url encoded - "/"&"+" are replaced with "_"&"-" - no padding.
                    b64 = "A991R2IAAAAALABodHRwczovL2xvY2FsaG9zdDo1MDAxL1NvbGlkUnBjL0Fic3RyYWN0aW9uczcAQnJva2VySW1hZ2U6ODljMWVmNjctNWY0Yy00MGE3LTkxODktYWRiNzE2MjlmMzM0OnIxMDB4MAABcvnFdKkOCb5wa57Ov1F8FYT8d1sgN1Z_GE74o93zIKWyyMe2i_KbKy2f_Q032YLv4Nl3HcpXHaeApsrmPRPtXQBVmTeHVgjVfaz2FQbe06vV9MSmKsgmxZs1tCAlO6kzsOsN7e9cTYL5TzQyAiFqWN7hXEICWdiI9mBk__Z0LO1n32TVXyrMclEBqMU_LkrGoOhvTsWL353iOLeMtzoLz4dYOeezZSz2Cyc9jEWhBHGsHnly_oqTb8CzJfPobQPQSjmF225QBA0v3eGCaem0VKfP2Pav59WabTECl38DSlyaKzHcpTbhq9F9hqrxP_Gq2A6JOyT7EzgPTPRhUw";
                    arr = Base64Url.Decode(HttpUtility.UrlDecode(b64));
                    moq.Setup(o => o.ProxyByteArray(It.Is<byte[]>(a => CompareStructs(a, arr)))).Returns(() => arr);
                    url = (await invoker.GetUriAsync(o => o.ProxyByteArray(new byte[0]))).ToString();
                    resp = await ctx.HttpClient.GetAsync($"{url}{b64}");
                    Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
                }
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

                var moq = new Mock<UrlAndQueryArgs.Services.IUrlAndQueryArgs>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<UrlAndQueryArgs.Services.IUrlAndQueryArgs>();

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
        public Task TestByteArrArgs()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestByteArrArgs).Substring(4));

                var moq = new Mock<ByteArrArgs.Services.IByteArrArgs>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var barr = new byte[] { 1,2,3 };
                var proxy = ctx.ClientServiceProvider.GetRequiredService<ByteArrArgs.Services.IByteArrArgs>();
                moq.Setup(o => o.ProxyByteArray(It.Is<byte[]>(a => CompareStructs(barr, a)))).Returns(() => barr);
                var res = proxy.ProxyByteArray(barr);
                Assert.IsTrue(CompareStructs(barr, res));
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

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestOpenApiConfig()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestOpenApiConfig).Substring(4));

                var moq = new Mock<OpenApiConfig.Services.IOpenApiConfig>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<OpenApiConfig.Services.IOpenApiConfig>();

                moq.Setup(o => o.ProxyStrings(It.Is<string>(a => a == "s1"), It.Is<string>(a => a == "s2"), It.Is<string>(a => a == "s3"))).Returns(() => "s4");
                var res = proxy.ProxyStrings("s1", "s2", "s3");

                Assert.AreEqual("s4", res);

                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<OpenApiConfig.Services.IOpenApiConfig>>();
                var uri = await invoker.GetUriAsync(o => o.ProxyStrings("s1", "s2", "s3"));
                Assert.AreEqual("/SolidRpc/Tests/Swagger/SpecGen/OpenApiConfig/Services/IOpenApiConfig/ProxyStrings/s3?s2=s2", uri.PathAndQuery.ToString());
            });
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestOAuth2()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestOAuth2).Substring(4));

                var moq = new Mock<OAuth2.Services.IOAuth2>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);
                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<OAuth2.Services.IOAuth2>();

                moq.Setup(o => o.Discovery(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(new OAuth2.Types.OpenIDConnectDiscovery() { Issuer = "My issuer" }));
                moq.Setup(o => o.Token(It.Is<string>(a => a == "grant_type"), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(new OAuth2.Types.TokenResponse() { AccessToken = "AccessToken" }));
                //
                // use proxy(may be GET or POST)
                //
                var res = await proxy.Token("grant_type");
                Assert.AreEqual("AccessToken", res.AccessToken);

                //
                // use GET method
                //
                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<OAuth2.Services.IOAuth2>>();
                var uri = await invoker.GetUriAsync(o => o.Token("grant_type", null, null, null, null, null, null, null, null, null, CancellationToken.None));
                Assert.AreEqual("/SolidRpc/Tests/Swagger/SpecGen/OAuth2/Services/IOAuth2/Token?grant_type=grant_type", uri.PathAndQuery.ToString());

                var httpClient = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("OAuth2");
                var strUri = uri.ToString();
                var getResp = await httpClient.GetStringAsync(strUri);
                Assert.AreEqual("{\"access_token\":\"AccessToken\"}", getResp);

                //
                // use POST method
                //
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "grant_type", "grant_type" }
                });
                var postResp = await httpClient.PostAsync(strUri.Substring(0, strUri.IndexOf('?')), content);
                var postStrResp = await postResp.Content.ReadAsStringAsync();
                Assert.AreEqual("{\"access_token\":\"AccessToken\"}", postStrResp);
                
                // test discovery
                uri = await invoker.GetUriAsync(o => o.Discovery(CancellationToken.None));
                Assert.AreEqual("/SolidRpc/Tests/Swagger/SpecGen/OAuth2/.well-known/openid-configuration", uri.PathAndQuery.ToString());
                strUri = uri.ToString();
                getResp = await httpClient.GetStringAsync(strUri);
                Assert.AreEqual("{\"issuer\":\"My issuer\"}", getResp);
            });
        }

        /// <summary>
        /// Tests invoking the generated proxy.
        /// </summary>
        [Test]
        public Task TestFormAsStructArg()
        {
            return RunTestInContext(async ctx =>
            {
                var config = ReadOpenApiConfiguration(nameof(TestFormAsStructArg).Substring(4));

                var moq = new Mock<FormAsStructArg.Services.IFormAsStructArg>(MockBehavior.Strict);
                ctx.AddServerAndClientService(moq.Object, config);

                await ctx.StartAsync();
                var proxy = ctx.ClientServiceProvider.GetRequiredService<FormAsStructArg.Services.IFormAsStructArg>();

                var formData = new FormAsStructArg.Types.FormData() { StringValue = "test", GuidValue = Guid.NewGuid(), IntValue = 123 };
                moq.Setup(o => o.GetFormData(
                    It.Is<FormAsStructArg.Types.FormData>(a => CompareStructs(formData, a))
                    )).Returns(formData);

                var res = proxy.GetFormData(formData);
                CompareStructs(formData, res);

                //
                // proxying strings should still work
                //
                var farr = new[] { 
                    new FormAsStructArg.Types.FormData() { 
                        StringValue = "struct1"
                    },
                    new FormAsStructArg.Types.FormData() {
                        StringValue = "struct2"
                    }
                };
                moq.Setup(o => o.ProxyForms(
                    It.Is<string>(a => a == "dummy"),
                    It.Is<IEnumerable<FormAsStructArg.Types.FormData>>(a => CompareStructs(typeof(IEnumerable<FormAsStructArg.Types.FormData>), farr, a))
                    )).Returns(farr);
                var sarrres = proxy.ProxyForms("dummy", farr);
                CompareStructs(typeof(IEnumerable<FormAsStructArg.Types.FormData>), farr, sarrres);

                moq.Setup(o => o.ProxyForms(
                     It.Is<string>(a => a == "dummy"),
                     It.Is<IEnumerable<FormAsStructArg.Types.FormData>>(a => a == null)
                     )).Returns((IEnumerable<FormAsStructArg.Types.FormData>)null);

                sarrres = proxy.ProxyForms("dummy", null);

                var invoker = ctx.ClientServiceProvider.GetRequiredService<IInvoker<FormAsStructArg.Services.IFormAsStructArg>>();
                var uri = await invoker.GetUriAsync(o => o.GetFormData(formData));
                var httpClient = ctx.ClientServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("FormAsStructArg");
                
                // all params
                var dict = new Dictionary<string, string>()
                {
                    { nameof(FormAsStructArg.Types.FormData.StringValue), formData.StringValue },
                    { nameof(FormAsStructArg.Types.FormData.IntValue), formData.IntValue.ToString() },
                    { nameof(FormAsStructArg.Types.FormData.GuidValue), formData.GuidValue.ToString() }
                };
                HttpContent content = new FormUrlEncodedContent(dict);

                var httpRes = await httpClient.PostAsync(uri, content);
                Assert.AreEqual(HttpStatusCode.OK, httpRes.StatusCode);
                ctx.ClientServiceProvider.GetRequiredService<ISerializerFactory>().DeserializeFromString(await httpRes.Content.ReadAsStringAsync(), out res);
                CompareStructs(formData, res);

                // only supply the string value
                formData.GuidValue = Guid.Empty;
                formData.IntValue = 0;
                dict = new Dictionary<string, string>()
                {
                    { nameof(FormAsStructArg.Types.FormData.StringValue), formData.StringValue },
                    { "ExtraValue", "dummy" }
                };

                content = new FormUrlEncodedContent(dict);

                httpRes = await httpClient.PostAsync(uri, content);
                Assert.AreEqual(HttpStatusCode.OK, httpRes.StatusCode);
                ctx.ClientServiceProvider.GetRequiredService<ISerializerFactory>().DeserializeFromString(await httpRes.Content.ReadAsStringAsync(), out res);
                CompareStructs(formData, res);

                // use multipart data to post 
                content = new MultipartFormDataContent();
                var sc = new StringContent(formData.StringValue);
                sc.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                sc.Headers.ContentDisposition.Name = nameof(FormAsStructArg.Types.FormData.StringValue);
                ((MultipartFormDataContent)content).Add(sc);

                httpRes = await httpClient.PostAsync(uri, content);
                Assert.AreEqual(HttpStatusCode.OK, httpRes.StatusCode);
                ctx.ClientServiceProvider.GetRequiredService<ISerializerFactory>().DeserializeFromString(await httpRes.Content.ReadAsStringAsync(), out res);
                CompareStructs(formData, res);

            });
        }
    }
}
