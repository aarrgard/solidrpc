using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
                var httpClient = new HttpClient();
                return httpClient.GetAsync(new Uri(BaseAddress, requestUri));
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