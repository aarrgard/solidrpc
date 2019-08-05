using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    /// <summary>
    /// 
    /// </summary>
    public class InjectBindingProvider : IBindingProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configProvider"></param>
        public InjectBindingProvider(InjectExtensionConfigProvider configProvider)
        {
            ConfigProvider = configProvider;
        }

        /// <summary>
        /// The configuration provider
        /// </summary>
        public InjectExtensionConfigProvider ConfigProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceScopeFactory ServiceScopeFactory => ConfigProvider.ServiceScopeFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            context.Parameter.Member.DeclaringType.Assembly.GetCustomAttributes(true).OfType<SolidRpcServiceCollectionAttribute>();
            return Task.FromResult<IBinding>(new InjectBinding(this, context.Parameter.Name, context.Parameter.ParameterType));
        }
    }
}