using Microsoft.Extensions.Logging;
using SolidProxy.Core.Proxy;
using SolidRpc.OpenApi.Binder;
using SolidRpc.OpenApi.Model;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Proxy
{
    public class SolidRpcProxyAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public SolidRpcProxyAdvice(ILogger<SolidRpcProxyAdvice<TObject, TMethod, TAdvice>> logger, IServiceProvider serviceProvider) {
            Logger = logger;
            HttpMessageHandler = (HttpMessageHandler)serviceProvider.GetService(typeof(HttpMessageHandler));
        }

        public HttpMessageHandler HttpMessageHandler { get; }

        private ILogger Logger { get; }

        public IMethodInfo MethodInfo { get; private set; }

        public void Configure(ISolidRpcProxyConfig config)
        {
            if(config.OpenApiConfiguration == null)
            {
                // locate config base on assembly name
                var assembly = typeof(TObject).Assembly;
                var assemblyName = assembly.GetName().Name;
                var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(o => o.EndsWith($".{assemblyName}.json"));
                if(resourceName == null)
                {
                    throw new Exception($"Solid proxy advice config does not contain a swagger spec for {typeof(TObject).FullName}.");
                }
                using (var s = assembly.GetManifestResourceStream(resourceName))
                {
                    using (var sr = new StreamReader(s))
                    {
                        config.OpenApiConfiguration = sr.ReadToEnd();
                    }
                }
            }
            // use the swagger binder to setup the invocation
            var swaggerConf = OpenApiParser.ParseSwaggerSpec(config.OpenApiConfiguration);
            if(swaggerConf == null)
            {
                throw new Exception($"Cannot parse swagger configuration({config}).");
            }
            if(config.RootAddress != null)
            {
                swaggerConf.SetSchemeAndHostAndPort(config.RootAddress);
            }
            MethodInfo = swaggerConf.GetMethodBinder().GetMethodInfo(config.InvocationConfiguration.MethodInfo);
        }

        private HttpClient CreateHttpClient()
        {
            if(HttpMessageHandler != null)
            {
                return new HttpClient(HttpMessageHandler);
            }
            else
            {
                return new HttpClient();
            }
        }

        public async Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            using (var httpClient = CreateHttpClient())
            {
                var httpReq = new HttpRequest();
                await MethodInfo.BindArgumentsAsync(httpReq, invocation.Arguments);

                Logger.LogTrace($"Sending data to {httpReq.Scheme}://{httpReq.HostAndPort}{httpReq.Path}");

                var httpClientReq = new HttpRequestMessage();
                httpReq.CopyTo(httpClientReq);

                var httpClientResponse = await httpClient.SendAsync(httpClientReq);
                var httpResp = new HttpResponse();
                await httpResp.CopyFrom(httpClientResponse);

                return MethodInfo.ExtractResponse<TAdvice>(httpResp);
            }
        }
    }
}
