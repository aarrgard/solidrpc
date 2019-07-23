using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    public class InjectValueProvider : IValueProvider, IValueBinder
    {
        public InjectValueProvider(Guid functionInstanceId, InjectBinding injectBinding, IServiceProvider scopedServiceProvider)
        {
            FunctionInstanceId = functionInstanceId;
            InjectBinding = injectBinding;
            ScopedServiceProvider = scopedServiceProvider;
        }

        public Guid FunctionInstanceId { get; }
        public InjectBinding InjectBinding { get; }
        public IServiceProvider ScopedServiceProvider { get; }

        public Type Type => InjectBinding.Type;

        public Task<object> GetValueAsync()
        {
            return Task.FromResult(ScopedServiceProvider.GetService(Type));
        }

        public Task SetValueAsync(object value, CancellationToken cancellationToken)
        {
            InjectBinding.CleanupFunction(FunctionInstanceId);
            return Task.CompletedTask;
        }

        public string ToInvokeString()
        {
            return null;
        }
    }
}