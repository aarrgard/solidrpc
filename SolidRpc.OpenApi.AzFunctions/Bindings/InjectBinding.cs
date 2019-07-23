using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.DependencyInjection;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    public class InjectBinding : IBinding
    {
        public InjectBinding(IServiceScopeFactory serviceScopeFactory, string name, Type type)
        {
            ServiceScopeFactory = serviceScopeFactory;
            ScopedServiceProviders = new ConcurrentDictionary<Guid, IServiceScope>();
            Name = name;
            Type = type;
        }

        public IServiceScopeFactory ServiceScopeFactory { get; }

        public string Name { get; }

        public Type Type { get; }

        public bool FromAttribute => true;

        public ConcurrentDictionary<Guid, IServiceScope> ScopedServiceProviders { get; }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            var scopedServiceProvider = ScopedServiceProviders.GetOrAdd(context.FunctionInstanceId, (_) => ServiceScopeFactory.CreateScope()).ServiceProvider; 
            return Task.FromResult<IValueProvider>(new InjectValueProvider(context.FunctionInstanceId, this, scopedServiceProvider));
        }

        public void CleanupFunction(Guid functionInstanceId)
        {
            if(ScopedServiceProviders.TryRemove(functionInstanceId, out IServiceScope sc))
            {
                sc.Dispose();
            }
        }

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var scopedServiceProvider = ScopedServiceProviders.GetOrAdd(context.FunctionInstanceId, (_) => ServiceScopeFactory.CreateScope()).ServiceProvider;
            return Task.FromResult<IValueProvider>(new InjectValueProvider(context.FunctionInstanceId, this, scopedServiceProvider));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor()
            {
                Name = Name,
            };
        }
    }
}