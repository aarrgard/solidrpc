using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.DependencyInjection;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    /// <summary>
    /// The inject binding.
    /// </summary>
    public class InjectBinding : IBinding
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public InjectBinding(InjectBindingProvider injectBindingProvider, string name, Type type)
        {
            InjectBindingProvider = injectBindingProvider;
            ScopedServiceProviders = new ConcurrentDictionary<Guid, IServiceScope>();
            Name = name;
            Type = type;
        }

        /// <summary>
        /// 
        /// </summary>
        public InjectBindingProvider InjectBindingProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        public ConcurrentDictionary<Guid, IServiceScope> ScopedServiceProviders { get; }

        /// <summary>
        /// Returns the service scope factory
        /// </summary>
        public IServiceScopeFactory ServiceScopeFactory => InjectBindingProvider.ServiceScopeFactory;

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool FromAttribute => true;
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            var scopedServiceProvider = ScopedServiceProviders.GetOrAdd(context.FunctionInstanceId, (_) => ServiceScopeFactory.CreateScope()).ServiceProvider; 
            return Task.FromResult<IValueProvider>(new InjectValueProvider(context.FunctionInstanceId, this, scopedServiceProvider));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var scopedServiceProvider = ScopedServiceProviders.GetOrAdd(context.FunctionInstanceId, (_) => ServiceScopeFactory.CreateScope()).ServiceProvider;
            return Task.FromResult<IValueProvider>(new InjectValueProvider(context.FunctionInstanceId, this, scopedServiceProvider));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor()
            {
                Name = Name,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionInstanceId"></param>
        public void CleanupFunction(Guid functionInstanceId)
        {
            if (ScopedServiceProviders.TryRemove(functionInstanceId, out IServiceScope sc))
            {
                sc.Dispose();
            }
        }
    }
}