using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Proxy;
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
        /// A kestrel test context
        /// </summary>
        public class TestHostContextKestrel : TestHostContext, IStartup
        {
            private static int s_hostPort = 5000;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="webHostTest"></param>
            public TestHostContextKestrel(WebHostTest webHostTest) : base(webHostTest, new HttpClient())
            {
            }

            /// <summary>
            /// The constructed host
            /// </summary>
            public IWebHost WebHost { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override async Task StartAsync()
            {
                WebHost = GetWebHost();
                await WebHost.StartAsync();

                var feature = WebHost.ServerFeatures.Get<IServerAddressesFeature>();
                foreach (var addr in feature.Addresses)
                {
                    ServerStarted(new Uri(addr)); 
                    BaseAddress = new Uri(addr);
                }

                await base.StartAsync();
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
                builder.UseUrls($"http://localhost:{s_hostPort++}");
                return builder.Build();
            }

            /// <summary>
            /// 
            /// </summary>
            public override void Dispose()
            {
                base.Dispose();
                WebHost?.StopAsync().Wait();
                WebHost?.Dispose();
            }

        }


        /// <summary>
        /// A kestrel test context
        /// </summary>
        public class TestHostContextHttpMessageHandler : TestHostContext
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="webHostTest"></param>
            public TestHostContextHttpMessageHandler(WebHostTest webHostTest) : base(webHostTest, new HttpClient())
            {
                BaseAddress = new Uri("https://localhost/");
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="services"></param>
            /// <returns></returns>
            public override IServiceProvider ConfigureClientServices(IServiceCollection services)
            {
                //
                // configure server services and use them from a singleton registration.
                // 
                var serverServices = new ServiceCollection();
                var serverProvider = ConfigureServices(serverServices);

                services.AddSingleton<HttpMessageHandler>(new SolidRpcHttpMessageHandler(serverProvider.GetRequiredService<IMethodInvoker>()));
                return base.ConfigureClientServices(services);
            }

        }

        /// <summary>
        /// Represents a web host context
        /// </summary>
        public abstract class TestHostContext : IDisposable
        {
            /// <summary>
            /// Event raised when the server is started
            /// </summary>
            public event Action<Uri> ServerStartedEvent;
            private IServiceProvider _serviceProvider;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="webHostTest"></param>
            /// <param name="httpClient"></param>
            public TestHostContext(WebHostTest webHostTest, HttpClient httpClient)
            {
                WebHostTest = webHostTest;
                HttpClient = httpClient;
                ServiceInterceptors = new List<ServiceInterceptor>();
                ClientServices = new ServiceCollection();
                ClientServices.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();

            }

            /// <summary>
            /// The service interceptors in this test.
            /// </summary>
            public IList<ServiceInterceptor> ServiceInterceptors { get; }

            /// <summary>
            /// The client services
            /// </summary>
            public ServiceCollection ClientServices { get; }

            /// <summary>
            /// The web host test
            /// </summary>
            public WebHostTest WebHostTest { get; }

            /// <summary>
            /// The Http client
            /// </summary>
            public HttpClient HttpClient { get; }

            /// <summary>
            /// The test base address
            /// </summary>
            public Uri BaseAddress { get; protected set; }

            /// <summary>
            /// The service provider for the test context.
            /// </summary>
            public IServiceProvider ServiceProvider {
                get
                {
                    if(_serviceProvider == null)
                    {
                        throw new Exception("Context not started.");

                    }
                    return _serviceProvider;
                }
            }

            /// <summary>
            /// Starts the conted
            /// </summary>
            /// <returns></returns>
            public virtual Task StartAsync()
            {
                _serviceProvider = ConfigureClientServices(ClientServices);
                return Task.CompletedTask;
            }

            /// <summary>
            /// Invoked when the server has started.
            /// </summary>
            /// <param name="uri"></param>
            protected void ServerStarted(Uri uri)
            {
                ServerStartedEvent?.Invoke(uri);
            }

            /// <summary>
            /// Disposes the context - stops the host.
            /// </summary>
            public virtual void Dispose()
            {
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
            public virtual IServiceProvider ConfigureClientServices(IServiceCollection services)
            {
                WebHostTest.ConfigureClientServices(services);
                services.AddSingleton<IMethodBinderStore, MethodBinderStore>();
                return services.BuildServiceProvider();
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
                services.AddSolidRpcSingletonServices();
                ServiceInterceptors.ToList().ForEach(m =>
                {
                    services.AddTransient(m.MethodInfo.DeclaringType);
                    var methodConf = services.AddSolidRpcBinding(m.MethodInfo, GetBaseUrl, m.OpenApiConfiguration);

                    var interceptorConf = methodConf.ConfigureAdvice<IServiceInterceptorAdviceConfig>();
                    var serviceCalls = interceptorConf.ServiceCalls ?? new List<ServiceCall>();
                    serviceCalls.Add(new ServiceCall(m.MethodInfo, m.Callback));
                    interceptorConf.ServiceCalls = serviceCalls;

                });
                configBuilder.AddAdvice(typeof(ServiceInterceptorAdvice<,,>));
                return WebHostTest.ConfigureServerServices(services);
            }

            private Uri GetBaseUrl(IServiceProvider serviceProvider, Uri baseUri)
            {
                return new Uri(BaseAddress.ToString() + baseUri.AbsolutePath.Substring(1));
            }

            /// <summary>
            /// Configures the host.
            /// </summary>
            /// <param name="app"></param>
            public void Configure(IApplicationBuilder app)
            {
                WebHostTest.Configure(app);
            }

            /// <summary>
            /// Adds an openapi proxy
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="openApiConfiguration"></param>
            public void AddOpenApiProxy<T>(string openApiConfiguration) where T : class
            {
                ClientServices.AddTransient<T, T>();
                var conf = ClientServices.GetSolidConfigurationBuilder()
                    .ConfigureInterface<T>()
                    .ConfigureAdvice<ISolidRpcProxyConfig>();
                conf.OpenApiConfiguration = openApiConfiguration;
                conf.BaseUriTransformer = GetBaseUrl;

                ClientServices.GetSolidConfigurationBuilder().AddAdvice(typeof(LoggingAdvice<,,>), o => o.MethodInfo.DeclaringType == typeof(T));
                ClientServices.GetSolidConfigurationBuilder().AddAdvice(typeof(SolidRpcProxyAdvice<,,>));
            }
        }

        /// <summary>
        /// Constructs a new host context
        /// </summary>
        /// <returns></returns>
        protected TestHostContext CreateHttpMessageHandlerContext()
        {
            var ctx = new TestHostContextHttpMessageHandler(this);
            return ctx;
        }

        /// <summary>
        /// Constructs a new host context
        /// </summary>
        /// <returns></returns>
        protected TestHostContext CreateKestrelHostContext()
        {
            var ctx = new TestHostContextKestrel(this);
            return ctx;
        }

        /// <summary>
        /// Constructs a new host context
        /// </summary>
        /// <returns></returns>
        protected async Task<TestHostContext> StartKestrelHostContextAsync()
        {
            var ctx = new TestHostContextKestrel(this);
            await ctx.StartAsync();
            return ctx;
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
            services.AddLogging(ConfigureLogging);
            services.AddSolidRpcSingletonServices();
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Configures the services hosted on the client
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public virtual void ConfigureClientServices(IServiceCollection services)
        {
            services.AddLogging(ConfigureLogging);
            services.AddSolidRpcSingletonServices();
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