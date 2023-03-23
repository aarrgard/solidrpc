using System;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    /// <summary>
    /// 
    /// </summary>
    public class InjectExtensionConfigProvider : IExtensionConfigProvider
    {
        private readonly object s_mutex = new object();
        private IServiceScopeFactory s_serviceScopeFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<InjectAttribute>().Bind(new InjectBindingProvider(this));
        }

        /// <summary>
        /// Returns the service scope factory.
        /// </summary>
        public IServiceScopeFactory ServiceScopeFactory
        {
            get
            {
                if (s_serviceScopeFactory == null)
                {
                    lock (s_mutex)
                    {
                        if (s_serviceScopeFactory == null)
                        {
                            s_serviceScopeFactory = CreateServiceScopeFactory();
                        }
                    }
                }
                return s_serviceScopeFactory;
            }
        }

        private IServiceScopeFactory CreateServiceScopeFactory()
        {
            var services = new ServiceCollection();
            SolidRpcServiceCollectionAttribute.ConfigureServices(services);
            var sp = services.BuildServiceProvider();
            return sp.GetRequiredService<IServiceScopeFactory>();
        }
    }
}
