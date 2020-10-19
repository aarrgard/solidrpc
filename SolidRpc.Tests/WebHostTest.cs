using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SolidProxy.GeneratorCastle;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Proxy;
using SolidRpc.OpenApi.Binder.Invoker;
using SolidRpc.OpenApi.Binder.Proxy;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// Reads the api config
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected string ReadOpenApiConfiguration(string folderName)
        {
            var folder = GetSpecFolder(folderName);
            var jsonFile = folder.GetFiles("*.json").Single();
            using (var sr = jsonFile.OpenText())
            {
                return sr.ReadToEnd();
            }
        }

        protected virtual DirectoryInfo GetSpecFolder(string folderName)
        {
            throw new NotImplementedException();
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
            public override IServiceProvider ServerServiceProvider => WebHost.Services;

            public override void ConfigureClientServices(IServiceCollection clientServices)
            {
                base.ConfigureClientServices(clientServices);

                clientServices.AddHttpClient(Options.DefaultName).ConfigurePrimaryHttpMessageHandler(o => new HttpClientHandler()
                {
                    AllowAutoRedirect = false
                });
                ServerServiceProvider.GetRequiredService<IMethodBinderStore>()
                    .MethodBinders.ToList().ForEach(binder =>
                    {
                        clientServices.AddHttpClient(binder.OpenApiSpec.Title).ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler()
                        {
                            AllowAutoRedirect = false
                        });
                    });

            }
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
            private IServiceProvider _serverServiceProvider;
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
            public override IServiceProvider ServerServiceProvider => _serverServiceProvider;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="services"></param>
            /// <returns></returns>
            public override void ConfigureClientServices(IServiceCollection services)
            {
                //
                // configure server services and use them from a singleton registration.
                // 
                var serverServices = new ServiceCollection();
                AddBaseAddress(serverServices, BaseAddress, true);
                _serverServiceProvider = ConfigureServices(serverServices);

                _serverServiceProvider.GetRequiredService<IMethodBinderStore>()
                    .MethodBinders.ToList().ForEach(binder =>
                    {
                        services.AddHttpClient(binder.OpenApiSpec.Title).ConfigurePrimaryHttpMessageHandler(sp =>
                        {
                            return new SolidRpcHttpMessageHandler(
                                _serverServiceProvider,
                                _serverServiceProvider.GetRequiredService<HttpHandler>(),
                        _serverServiceProvider.GetRequiredService<IMethodInvoker>());
                    });
                    });
                base.ConfigureClientServices(services);
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
            private IServiceProvider _clientServiceProvider;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="webHostTest"></param>
            /// <param name="httpClient"></param>
            public TestHostContext(WebHostTest webHostTest, HttpClient httpClient)
            {
                WebHostTest = webHostTest;
                HttpClient = httpClient;
                ServerServicesCallback = (_) => { };
                ClientServicesCallback = (_) => { };
            }

            private Action<IServiceCollection> ServerServicesCallback { get; set; }
            private Action<IServiceCollection> ClientServicesCallback { get; set; }

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
            public IServiceProvider ClientServiceProvider {
                get
                {
                    if(_clientServiceProvider == null)
                    {
                        throw new Exception("Context not started.");

                    }
                    return _clientServiceProvider;
                }
            }

            /// <summary>
            /// Returns the server service provider
            /// </summary>
            public abstract IServiceProvider ServerServiceProvider { get; }

            /// <summary>
            /// Starts the conted
            /// </summary>
            /// <returns></returns>
            public virtual Task StartAsync()
            {
                var clientServices = new ServiceCollection();
                clientServices.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
                ConfigureClientServices(clientServices);
                _clientServiceProvider = clientServices.BuildServiceProvider();
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
                TaskConsoleLogger.Flush();
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
                if (typeof(DateTimeOffset) == typeof(T))
                {
                    return ((DateTimeOffset)(object)value).ToString("yyyy-MM-ddTHH:mm:sszzz");
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
            /// <param name="clientServices"></param>
            /// <returns></returns>
            public virtual void ConfigureClientServices(IServiceCollection clientServices)
            {
                AddBaseAddress(clientServices, BaseAddress, false);
                WebHostTest.ConfigureClientServices(clientServices, BaseAddress);
                ClientServicesCallback(clientServices);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="services"></param>
            /// <param name="baseAddress"></param>
            /// <param name="addServerUrls"></param>
            protected void AddBaseAddress(IServiceCollection services, Uri baseAddress, bool addServerUrls)
            {
                if (BaseAddress == null) throw new Exception("No base address set");
                var strBaseAddress = baseAddress.ToString();
                strBaseAddress = strBaseAddress.Substring(0, strBaseAddress.Length - 1);

                var config = new Dictionary<string,string>();
                config["SolidRpc:BaseUrl"] = strBaseAddress;
                if(addServerUrls)
                {
                    config["urls"] = strBaseAddress;
                }

                services.GetConfigurationBuilder(() => new ConfigurationBuilder(), c => new ChainedConfigurationSource() { Configuration = c })
                    .AddInMemoryCollection(config);
            }

            /// <summary>
            /// Configures the services
            /// </summary>
            /// <param name="serverServices"></param>
            /// <returns></returns>
            public IServiceProvider ConfigureServices(IServiceCollection serverServices)
            {
                serverServices.GetSolidConfigurationBuilder().SetGenerator<SolidProxyCastleGenerator>();
                WebHostTest.ConfigureServerServices(serverServices);
                ServerServicesCallback(serverServices);
                return serverServices.BuildServiceProvider();
            }

            private Uri GetBaseUrl(IServiceProvider serviceProvider, Uri baseUri, MethodInfo methodInfo)
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                var url = config["urls"].Split(',').First();
                return new Uri(url + baseUri.AbsolutePath);
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
            /// Adds a callback to add the server services
            /// </summary>
            /// <param name="sc"></param>
            public void AddServerService(Action<IServiceCollection> sc)
            {
                var oldCallback = ServerServicesCallback;
                ServerServicesCallback = (_) =>
                {
                    oldCallback(_);
                    sc(_);
                };
            }

            /// <summary>
            /// Adds a callback to add the server services
            /// </summary>
            /// <param name="sc"></param>
            public void AddClientService(Action<IServiceCollection> sc)
            {
                var oldCallback = ServerServicesCallback;
                ClientServicesCallback = (_) =>
                {
                    oldCallback(_);
                    sc(_);
                };
            }

            /// <summary>
            /// Adds the serer and client service
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="impl"></param>
            /// <param name="config"></param>
            public void AddServerAndClientService<T>(T impl, string config) where T : class
            {
                AddServerService(sc => sc.AddSolidRpcSingletonBindings(impl, c =>
                {
                    c.OpenApiSpec = config;
                    return true;
                }));

                AddClientService(clientServices =>
                {
                    clientServices.AddTransient<T, T>();
                    var conf = clientServices.GetSolidConfigurationBuilder()
                        .ConfigureInterface<T>()
                        .ConfigureAdvice<ISolidRpcOpenApiConfig>();
                    conf.OpenApiSpec = config;
                    conf.SetMethodAddressTransformer(GetBaseUrl);

                    clientServices.GetSolidConfigurationBuilder().AddAdviceDependency(typeof(LoggingAdvice<,,>), typeof(SolidRpcOpenApiInitAdvice<,,>));
                    clientServices.GetSolidConfigurationBuilder().AddAdvice(adviceType: typeof(SolidRpcOpenApiInitAdvice<,,>));
                    clientServices.GetSolidConfigurationBuilder().AddAdvice(typeof(LoggingAdvice<,,>), o => o.MethodInfo.DeclaringType == typeof(T));
                });
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
        public virtual void ConfigureServerServices(IServiceCollection services)
        {
            ConfigureConfiguration(services);
            services.AddLogging(ConfigureLogging);
            services.AddHttpClient();
            services.AddSolidRpcSingletonServices();
        }

        /// <summary>
        /// Configures the services hosted on the client
        /// </summary>
        /// <param name="clientServices"></param>
        /// <param name="baseAddress"></param>
        /// <returns></returns>
        public virtual void ConfigureClientServices(IServiceCollection clientServices, Uri baseAddress)
        {
            ConfigureConfiguration(clientServices);
            clientServices.AddLogging(ConfigureLogging);
            clientServices.AddHttpClient();
            clientServices.AddSolidRpcSingletonServices();
        }

        private void ConfigureConfiguration(IServiceCollection services)
        {
            services.GetConfigurationBuilder(() => new ConfigurationBuilder(), c => new ChainedConfigurationSource() { Configuration = c })
                .AddJsonFile("appsettings.local.json", true);
            services.BuildConfiguration(() => new ConfigurationBuilder());
        }

        /// <summary>
        /// Configures the application
        /// </summary>
        /// <param name="app"></param>
        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            if(app.ApplicationServices.GetService(typeof(ISwaggerProvider)) != null)
            {
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
            }

            app.UseSolidRpcProxies();
        }
    }
}