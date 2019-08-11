using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using SolidRpc.Abstractions.Services;
using SolidRpc.OpenApi.AspNetCore.Services;
using SolidRpc.OpenApi.AzFunctions.Services;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using SolidRpc.OpenApi.Binder;

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
            var azFuncHandler = services.GetAzFunctionHandler();

            if (!services.Any(o => o.ServiceType == typeof(IConfiguration)))
            {
                var cb = new ConfigurationBuilder();
                cb.AddEnvironmentVariables();
                cb.AddInMemoryCollection(new Dictionary<string, string>()
                {
                    { ConfigurationBaseUriTransformer.ConfigPathPrefix, azFuncHandler.HttpRouteFrontendPrefix}
                });
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
            services.AddSolidRpcServices(o =>
            {
                o.AddRpcHostServices = true;
                o.AddStaticContentServices = true;
            });
            services.AddSingleton<ISolidRpcHost, SolidRpcHostAzFunctions>();
        }
    }
}
