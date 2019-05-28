using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Proxy;
using SolidRpc.Swagger.Model.V2;
using SolidRpc.Tests.Generated.Local.Services;

namespace SolidRpc.Tests.MvcProxyTest
{
    /// <summary>
    /// Tests sending data back and forth between client and server.
    /// </summary>
    public class MvcProxyTest : WebHostMvcTest
    {
        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyBoolean()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyBooleanInQuery)}?b=true");
                Assert.AreEqual("true", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(true, await sp.ProxyBooleanInQuery(true));
                Assert.AreEqual(false, await sp.ProxyBooleanInQuery(false));
            }
        }

        /// <summary>
        /// Sends a byte back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyByte()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyByteInQuery)}?b=10");
                Assert.AreEqual("10", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual((byte)10, await sp.ProxyByteInQuery(10));
                Assert.AreEqual((byte)11, await sp.ProxyByteInQuery(11));
            }
        }

        /// <summary>
        /// Sends a short back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyShort()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyShortInQuery)}?s=10");
                Assert.AreEqual("10", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual((short)10, await sp.ProxyShortInQuery(10));
                Assert.AreEqual((short)11, await sp.ProxyShortInQuery(11));
            }
        }

        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyInt()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var vals = new int[] { 10 };
                var nvps = vals.Select(o => new KeyValuePair<string, int>("i", o));
                var resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyIntInQuery)}", nvps);
                Assert.AreEqual("10", await AssertOk(resp));
                resp = await ctx.PostResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyIntInForm)}", nvps);
                Assert.AreEqual("10", await AssertOk(resp));
                resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyIntInRoute)}/10");
                Assert.AreEqual("10", await AssertOk(resp));

                // The .net core mvc does not bind it - its in the header collection on the server !!!
                //resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyIntInHeader)}", null, nvps);
                //Assert.AreEqual("10", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(10, await sp.ProxyIntInQuery(10));
                Assert.AreEqual(11, await sp.ProxyIntInForm(11));
                Assert.AreEqual(13, await sp.ProxyIntInRoute(13));
                //Assert.AreEqual(14, await sp.ProxyIntInHeader(14));
            }
        }

        /// <summary>
        /// Sends an integer array-array back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test, Ignore("implement binder")]
        public async Task TestProxyIntArrArr()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse("/MvcProxyTest/ProxyIntArrArr?iarr=10");
                Assert.AreEqual("[[10]]", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                var intArrArr = new int[][] { new int[] { 10 } };
                Assert.AreEqual(intArrArr, await sp.ProxyIntArrArrInQuery(intArrArr));
            }
        }

        /// <summary>
        /// Sends a long back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyLong()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyLongInQuery)}?l=10");
                Assert.AreEqual("10", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(10L, await sp.ProxyLongInQuery(10));
                Assert.AreEqual(11L, await sp.ProxyLongInQuery(11));
            }
        }


        /// <summary>
        /// Sends a datetime back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyFloat()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var f = 1.5f;
                var resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyFloatInQuery)}?f={f.ToString(CultureInfo.InvariantCulture)}");
                Assert.AreEqual($"{f.ToString(CultureInfo.InvariantCulture)}", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(f, await sp.ProxyFloatInQuery(f));
            }
        }


        /// <summary>
        /// Sends a datetime back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyDouble()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var d = 3.5d;
                var resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyDoubleInQuery)}?d={d.ToString(CultureInfo.InvariantCulture)}");
                Assert.AreEqual($"{d.ToString(CultureInfo.InvariantCulture)}", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(d, await sp.ProxyDoubleInQuery(d));
            }
        }

        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyString()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyStringInQuery)}?s=testar");
                Assert.AreEqual("\"testar\"", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual("testar", await sp.ProxyStringInQuery("testar"));
                Assert.AreEqual("testar2", await sp.ProxyStringInQuery("testar2"));
            }
        }

        /// <summary>
        /// Sends an integer back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyGuid()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var guid = Guid.NewGuid();
                var resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyGuidInQuery)}?g={guid.ToString()}");
                Assert.AreEqual($"\"{guid.ToString()}\"", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(guid, await sp.ProxyGuidInQuery(guid));
            }
        }

        /// <summary>
        /// Sends a datetime back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyDateTime()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var dateTime = new DateTime(2019, 05, 25, 17, 32, 44);
                var resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyDateTimeInQuery)}?d={dateTime.ToString("yyy-MM-ddTHH:mm:ss")}");
                Assert.AreEqual($"\"{dateTime.ToString("yyy-MM-ddTHH:mm:ss")}\"", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(dateTime, await sp.ProxyDateTimeInQuery(dateTime));
            }
        }

        /// <summary>
        /// Sends a datetime array back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyDateTimeArr()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var dateTimeArr = new DateTime[] {
                    new DateTime(2019, 05, 25, 17, 32, 44),
                    new DateTime(2019, 05, 25, 17, 32, 47),
                };
                var nvps = dateTimeArr.Select(o => new KeyValuePair<string, DateTime>("dArr", o));
                var resp = await ctx.GetResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyDateTimeArrayInQuery)}", nvps);
                Assert.AreEqual($"[\"{dateTimeArr[0].ToString("yyy-MM-ddTHH:mm:ss")}\",\"{dateTimeArr[1].ToString("yyy-MM-ddTHH:mm:ss")}\"]", await AssertOk(resp));
                resp = await ctx.PostResponse($"/MvcProxyTest/{nameof(MvcProxyTestController.ProxyDateTimeArrayInForm)}", nvps);
                Assert.AreEqual($"[\"{dateTimeArr[0].ToString("yyy-MM-ddTHH:mm:ss")}\",\"{dateTimeArr[1].ToString("yyy-MM-ddTHH:mm:ss")}\"]", await AssertOk(resp));

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(dateTimeArr, await sp.ProxyDateTimeArrayInQuery(dateTimeArr));
                Assert.AreEqual(dateTimeArr, await sp.ProxyDateTimeArrayInForm(dateTimeArr));
            }
        }

        private NameValueCollection CreateNameValueCollection<T>(params KeyValuePair<string, T>[] values)
        {
            var nvc = new NameValueCollection();
            if (typeof(T) == typeof(DateTime))
            {
                values.ToList().ForEach(o => nvc.Add(o.Key, ((DateTime)(object)o.Value).ToString("yyy-MM-ddTHH:mm:ss")));
            }
            return nvc;
        }

        /// <summary>
        /// Sends a stream back and forth between client and server
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task TestProxyStream()
        {
            using (var ctx = new TestHostContext(GetWebHost()))
            {
                var payload = new byte[] { 10, 20 };
                var msg = new HttpRequestMessage();
                msg.Method = HttpMethod.Post;
                msg.RequestUri = new Uri(ctx.BaseAddress, $"/MvcProxyTest/ProxyStream");
                var streamContent = new StreamContent(new MemoryStream(payload));
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "ff", FileName="test.xml" };
                var multipart = new MultipartFormDataContent();
                multipart.Add(streamContent);
                msg.Content = multipart;

                var resp = await ctx.GetResponse(msg);
                Assert.AreEqual("application/octet-stream", resp.Content.Headers.ContentType.MediaType);
                Assert.AreEqual(payload, await resp.Content.ReadAsByteArrayAsync());

                var sp = await CreateServiceProxy<IMvcProxyTest>(ctx);
                Assert.AreEqual(payload, ReadStream(await sp.ProxyStream(new MemoryStream(payload))));
            }
        }

        private byte[] ReadStream(Stream stream)
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        private async Task<T> CreateServiceProxy<T>(TestHostContext ctx) where T:class
        {
            var resp = await ctx.GetResponse("/swagger/v1/swagger.json");
            var swaggerConfiguration = await AssertOk(resp);

            var sc = new ServiceCollection();
            sc.AddTransient<T,T>();
            var proxyConf = sc.GetSolidConfigurationBuilder()
                .SetGenerator<SolidProxyCastleGenerator>()
                .ConfigureInterface<T>()
                .ConfigureAdvice<ISolidRpcProxyConfig>();
            proxyConf.SwaggerConfiguration = swaggerConfiguration;

            sc.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcProxyAdvice<,,>));

            var sp = sc.BuildServiceProvider();
            return sp.GetRequiredService<T>();
        }
    }
}