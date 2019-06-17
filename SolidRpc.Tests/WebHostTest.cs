using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SolidRpc.Tests
{
    /// <summary>
    /// Represents a test that sets up a webhost.
    /// </summary>
    public abstract class WebHostTest : TestBase, IStartup
    {
        /// <summary>
        /// Represents a web host context
        /// </summary>
        public class TestHostContext : IDisposable
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="webHostTest"></param>
            public TestHostContext(WebHostTest webHostTest)
            {
                WebHostTest = webHostTest;
                HttpClient = new HttpClient();
                StartAsync().Wait();

                var sc = new ServiceCollection();
                webHostTest.ConfigureClientServices(sc);
                ServiceProvider = sc.BuildServiceProvider();
            }

            /// <summary>
            /// The web host test
            /// </summary>
            public WebHostTest WebHostTest { get; }

            /// <summary>
            /// The constructed host
            /// </summary>
            public IWebHost WebHost { get; private set; }

            /// <summary>
            /// The Http client
            /// </summary>
            public HttpClient HttpClient { get; }

            /// <summary>
            /// The test base address
            /// </summary>
            public Uri BaseAddress { get; private set; }

            /// <summary>
            /// The service provider for the test context.
            /// </summary>
            public ServiceProvider ServiceProvider { get; }

            /// <summary>
            /// Starts the conted
            /// </summary>
            /// <returns></returns>
            public async Task StartAsync()
            {
                WebHost = WebHostTest.GetWebHost();
                await WebHost.StartAsync();

                var feature = WebHost.ServerFeatures.Get<IServerAddressesFeature>();
                foreach (var addr in feature.Addresses)
                {
                    BaseAddress = new Uri(addr);
                }

            }

            /// <summary>
            /// Disposes the context - stops the host.
            /// </summary>
            public void Dispose()
            {
                WebHost.StopAsync().Wait();
                HttpClient?.Dispose();
            }

            /// <summary>
            /// Returns the response
            /// </summary>
            /// <param name="requestUri"></param>
            /// <returns></returns>
            public Task<HttpResponseMessage> GetResponse(string requestUri)
            {
                return GetResponse<object>(requestUri);
            }

            public void CreateServerProxy<T>(Expression<Action<T>> expression)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Returns the response.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="requestUri"></param>
            /// <param name="formValues"></param>
            /// <param name="headerValues"></param>
            /// <returns></returns>
            public Task<HttpResponseMessage> GetResponse<T>(string requestUri, IEnumerable<KeyValuePair<string, T>> formValues = null, IEnumerable<KeyValuePair<string, T>> headerValues = null)
            {
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
                return HttpClient.SendAsync(req);
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
            
            /// <summary>
            /// Posts a response.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="requestUri"></param>
            /// <param name="nvps"></param>
            /// <returns></returns>
            public Task<HttpResponseMessage> PostResponse<T>(string requestUri, IEnumerable<KeyValuePair<string, T>> nvps = null)
            {
                var content = new FormUrlEncodedContent(nvps.Select(o => new KeyValuePair<string, string>(o.Key, ToString(o.Value))));
                return HttpClient.PostAsync(new Uri(BaseAddress, requestUri), content);
            }

            /// <summary>
            /// Returns the response
            /// </summary>
            /// <param name="msg"></param>
            /// <returns></returns>
            public Task<HttpResponseMessage> SendAsync(HttpRequestMessage msg)
            {
                return HttpClient.SendAsync(msg);
            }
        }

        /// <summary>
        /// Constructs a new host context
        /// </summary>
        /// <returns></returns>
        protected TestHostContext CreateTestHostContext()
        {
            return new TestHostContext(this);
        }

        /// <summary>
        /// Returns the webbost. Does not start it.
        /// </summary>
        /// <returns></returns>
        protected IWebHost GetWebHost()
        {
            var builder = WebHost.CreateDefaultBuilder(new string[0]);
            builder.ConfigureLogging(ConfigureLogging);
            builder.ConfigureServices((sc) => {
                sc.AddSingleton<IStartup>(this);
            });
            return builder.Build();
        }

        /// <summary>
        /// Configures the logging.
        /// </summary>
        protected void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddConsole();
        }

        /// <summary>
        /// Asserts that the response is successful. Writes error message/page to disk.
        /// </summary>
        /// <param name="resp"></param>
        /// <returns></returns>
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

        IServiceProvider IStartup.ConfigureServices(IServiceCollection services) => ConfigureServerServices(services);

        /// <summary>
        /// Configures the services hosted on the server
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public virtual IServiceProvider ConfigureServerServices(IServiceCollection services)
        {
            services.GetSolidConfigurationBuilder();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Configures the services hosted on the client
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public virtual void ConfigureClientServices(IServiceCollection services)
        {

        }

        /// <summary>
        /// Configures the applicatio 
        /// </summary>
        /// <param name="app"></param>
        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => {
                    swaggerDoc.Host = httpReq.Host.Value;
                    swaggerDoc.Schemes = new string[] { httpReq.Scheme };
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseSolidRpcProxies();
        }
    }
}