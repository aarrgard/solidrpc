using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    [Extension("Inject")]
    public class InjectExtensionConfigProvider : IExtensionConfigProvider
    {
        public InjectExtensionConfigProvider(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        public IServiceScopeFactory ServiceScopeFactory { get; }

        public void Initialize(ExtensionConfigContext context)
        {
            context.AddBindingRule<InjectAttribute>().Bind(new InjectBindingProvider(ServiceScopeFactory));
        }
    }
}
