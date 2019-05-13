using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests
{
    public abstract class WebHostTest : IStartup
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

        public abstract IServiceProvider ConfigureServices(IServiceCollection services);

        public abstract void Configure(IApplicationBuilder app);
    }
}