using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    /// <summary>
    /// 
    /// </summary>
    [Extension("Inject")]
    public class InjectExtensionConfigProvider : IExtensionConfigProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public InjectExtensionConfigProvider(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        public IServiceScopeFactory ServiceScopeFactory { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<InjectAttribute>().Bind(new InjectBindingProvider(this));
        }
    }
}
