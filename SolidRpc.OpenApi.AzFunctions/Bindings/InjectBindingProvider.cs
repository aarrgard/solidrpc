using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    public class InjectBindingProvider : IBindingProvider
    {
        public InjectBindingProvider(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        public IServiceScopeFactory ServiceScopeFactory { get; }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            return Task.FromResult<IBinding>(new InjectBinding(ServiceScopeFactory, context.Parameter.Name, context.Parameter.ParameterType));
        }
    }
}