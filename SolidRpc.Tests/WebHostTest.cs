using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.OpenApi.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SolidRpc.Tests
{
    /// <summary>
    /// Represents a test that sets up a webhost.
    /// </summary>
    public abstract class WebHostTest : TestBase
    {
        /// <summary>
        /// A service interceptor
        /// </summary>
        public class ServiceInterceptor
        {
            /// <summary>
            /// Constructs a new instance
            /// </summary>
            /// <param name="methodInfo"></param>
            /// <param name="openApiConfiguration"></param>
            /// <param name="callback"></param>
            public ServiceInterceptor(MethodInfo methodInfo, string openApiConfiguration, Func<object[], object> callback)
            {
                MethodInfo = methodInfo;
                Callback = callback;
                OpenApiConfiguration = openApiConfiguration;
            }

            /// <summary>
            /// The method we are intercepting
            /// </summary>
            public MethodInfo MethodInfo { get; }

            /// <summary>
            /// The callback
            /// </summary>
            public Func<object[], object> Callback { get; }

            /// <summary>
            /// The open api configuration to use when binding the method.
            /// </summary>
            public string OpenApiConfiguration { get;  }
        }

        /// <summary>
        /// Represents a web host context
        /// </summary>
        public class TestHostContext : IStartup, IDisposable
        {
            private IServiceProvider _serviceProvider;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="webHostTest"></param>
            public TestHostContext(WebHostTest webHostTest)
            {
                WebHostTest = webHostTest;
                HttpClient = new HttpClient();
                ServiceInterceptors = new List<ServiceInterceptor>();
            }

            /// <summary>
            /// The service interceptors in this test.
            /// </summary>
            public IList<ServiceInterceptor> ServiceInterceptors { get; }

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
            public IServiceProvider ServiceProvider {
                get
                {
                    if(_serviceProvider == null)
                    {
                        var sc = new ServiceCollection();
                        WebHostTest.ConfigureClientServices(sc);
                        _serviceProvider = sc.BuildServiceProvider();

                    }
                    return _serviceProvider;
                }
            }

            /// <summary>
            /// Starts the conted
            /// </summary>
            /// <returns></returns>
            public async Task StartAsync()
            {
                WebHost = GetWebHost();
                await WebHost.StartAsync();

                var feature = WebHost.ServerFeatures.Get<IServerAddressesFeature>();
                foreach (var addr in feature.Addresses)
                {
                    BaseAddress = new Uri(addr);
                }

            }

            /// <summary>
            /// Returns the webbost. Does not start it.
            /// </summary>
            /// <returns></returns>
            protected IWebHost GetWebHost()
            {
                var builder = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(new string[0]);
                builder.ConfigureLogging(WebHostTest.ConfigureLogging);
                builder.ConfigureServices((sc) => {
                    sc.AddSingleton<IStartup>(this);
                });
                return builder.Build();
            }

            /// <summary>
            /// Disposes the context - stops the host.
            /// </summary>
            public void Dispose()
            {
                WebHost?.StopAsync().Wait();
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

            /// <summary>
            /// Adds an interceptor
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="expression"></param>
            /// <param name="openApiConfiguration"></param>
            /// <param name="callback"></param>
            public void CreateServerInterceptor<T>(Expression<Action<T>> expression, string openApiConfiguration, Func<object[], object> callback)
            {
                var methodInfo = ((MethodCallExpression)((LambdaExpression)expression).Body).Method;
                ServiceInterceptors.Add(new ServiceInterceptor(methodInfo, openApiConfiguration, callback) { });
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

            /// <summary>
            /// Configures the services
            /// </summary>
            /// <param name="services"></param>
            /// <returns></returns>
            public IServiceProvider ConfigureServices(IServiceCollection services)
            {
                var configBuilder = services.GetSolidConfigurationBuilder()
                    .SetGenerator<SolidProxyCastleGenerator>();
                ServiceInterceptors.ToList().ForEach(m =>
                {
                    if(!services.Any(s => s.ServiceType == m.MethodInfo.DeclaringType))
                    {
                        services.AddTransient(m.MethodInfo.DeclaringType, m.MethodInfo.DeclaringType);
                    }
                    var methodConf = configBuilder
                        .ConfigureInterfaceAssembly(m.MethodInfo.DeclaringType.Assembly)
                        .ConfigureInterface(m.MethodInfo.DeclaringType)
                        .ConfigureMethod(m.MethodInfo);
                    var interceptorConf = methodConf.ConfigureAdvice<IServiceInterceptorAdviceConfig>();
                    var serviceCalls = interceptorConf.ServiceCalls ?? new List<ServiceCall>();
                    serviceCalls.Add(new ServiceCall(m.MethodInfo, m.Callback));
                    interceptorConf.ServiceCalls = serviceCalls;

                    methodConf.ConfigureAdvice<ISolidRpcAspNetCoreConfig>().OpenApiConfiguration = m.OpenApiConfiguration;
                });
                configBuilder.AddAdvice(typeof(ServiceInterceptorAdvice<,,>));
                return WebHostTest.ConfigureServerServices(services);
            }

            /// <summary>
            /// Configures the host.
            /// </summary>
            /// <param name="app"></param>
            public void Configure(IApplicationBuilder app)
            {
                WebHostTest.Configure(app);
            }
        }

        /// <summary>
        /// Constructs a new host context
        /// </summary>
        /// <returns></returns>
        protected TestHostContext CreateTestHostContext()
        {
            var ctx = new TestHostContext(this);
            return ctx;
        }

        /// <summary>
        /// Constructs a new host context
        /// </summary>
        /// <returns></returns>
        protected async Task<TestHostContext> StartTestHostContextAsync()
        {
            var ctx = new TestHostContext(this);
            await ctx.StartAsync();
            return ctx;
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

        /// <summary>
        /// Configures the services hosted on the server
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public virtual IServiceProvider ConfigureServerServices(IServiceCollection services)
        {
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