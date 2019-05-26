using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SolidRpc.Tests
{
    public abstract class WebHostTest : TestBase, IStartup
    {
        public class TestHostContext : IDisposable
        {
            public TestHostContext(IWebHost webHost)
            {
                WebHost = webHost;
                webHost.StartAsync();

                var feature = WebHost.ServerFeatures.Get<IServerAddressesFeature>();
                foreach (var addr in feature.Addresses)
                {
                    BaseAddress = new Uri(addr);
                }
            }

            public IWebHost WebHost { get; }
            public Uri BaseAddress { get; }

            public void Dispose()
            {
                WebHost.StopAsync().Wait();
            }

            public Task<HttpResponseMessage> GetResponse(string requestUri)
            {
                return GetResponse<object>(requestUri);
            }
            public Task<HttpResponseMessage> GetResponse<T>(string requestUri, IEnumerable<KeyValuePair<string, T>> formValues = null, IEnumerable<KeyValuePair<string, T>> headerValues = null)
            {
                var httpClient = new HttpClient();
                if(formValues != null)
                {
                    var query = string.Join("&", formValues.Select(o => $"{o.Key}={HttpUtility.UrlEncode(ToString(o.Value))}"));
                    requestUri = requestUri + "?" + query;
                }
                var req = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(BaseAddress, requestUri)
                };
                if(headerValues != null)
                {
                    foreach (var h in headerValues)
                    {
                        req.Headers.Add(h.Key, ToString(h.Value));
                    }
                }
                return httpClient.SendAsync(req);
            }

            private string ToString<T>(T value)
            {
                if (typeof(DateTime) == typeof(T))
                {
                    return ((DateTime)(object)value).ToString("yyyy-MM-ddTHH:mm:ss");
                }
                if (typeof(int) == typeof(T))
                {
                    return value.ToString();
                }
                throw new NotImplementedException();
            }

            public Task<HttpResponseMessage> PostResponse<T>(string requestUri, IEnumerable<KeyValuePair<string, T>> nvps = null)
            {
                var httpClient = new HttpClient();
                var content = new FormUrlEncodedContent(nvps.Select(o => new KeyValuePair<string, string>(o.Key, ToString(o.Value))));
                return httpClient.PostAsync(new Uri(BaseAddress, requestUri), content);
            }
            public Task<HttpResponseMessage> GetResponse(HttpRequestMessage msg)
            {
                var httpClient = new HttpClient();
                return httpClient.SendAsync(msg);
            }
        }
        protected IWebHost GetWebHost()
        {
            var builder = WebHost.CreateDefaultBuilder(new string[0]);
            builder.ConfigureLogging(o =>
            {
                o.SetMinimumLevel(LogLevel.Trace);
                o.AddConsole();
            });
            builder.ConfigureServices((sc) => {
                sc.AddSingleton<IStartup>(this);
            });
            return builder.Build();
        }

        protected async Task<string> AssertOk(HttpResponseMessage resp)
        {
            var content = await resp.Content.ReadAsStringAsync();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                var fi = new FileInfo("c:\\temp\\result.html");
                using (var fs = fi.Create())
                {
                    var buf = Encoding.UTF8.GetBytes(content);
                    await fs.WriteAsync(buf, 0, buf.Length);
                }
                Assert.Fail($"Status:{resp.StatusCode}");
            }
            return content;
        }

        public abstract IServiceProvider ConfigureServices(IServiceCollection services);

        public abstract void Configure(IApplicationBuilder app);
    }
}