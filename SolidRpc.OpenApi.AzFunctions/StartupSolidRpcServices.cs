using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AspNetCore.Services;
using SolidRpc.OpenApi.AzFunctions.Services;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SolidRpc.Test.Petstore.AzFunctions
{
    /// <summary>
    /// Base class to configure the soldi rpc services
    /// </summary>
    public class StartupSolidRpcServices 
    {

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            if(!services.Any(o => o.ServiceType == typeof(IConfiguration)))
            {
                var cb = new ConfigurationBuilder();
                cb.AddEnvironmentVariables();
                services.AddSingleton<IConfiguration>(cb.Build());
            }
            if(!services.Any(o => o.ServiceType == typeof(ILogger)))
            {
                services.AddLogging(o => {
                    o.SetMinimumLevel(LogLevel.Trace);
                    o.AddProvider(new TraceWriterLoggerProvider());
                });
            }
            services.AddSingleton<IContentTypeProvider>(new FileExtensionContentTypeProvider());
            services.AddSolidRpcSingletonServices();

            services.AddSingleton<ISolidRpcHost, SolidRpcHostAzFunctions>();
            var openApiParser = services.GetSolidRpcOpenApiParser();
            var solidRpcHostSpec = openApiParser.CreateSpecification(typeof(ISolidRpcHost)).WriteAsJsonString();
            services.AddSolidRpcBindings(typeof(ISolidRpcHost), typeof(SolidRpcHostAzFunctions), solidRpcHostSpec);

            services.AddAzFunctionHttp<ISolidRpcStaticContent>(o => o.GetStaticContent(null, CancellationToken.None), () => new SolidRpcStaticContent());
        }
    }
}
