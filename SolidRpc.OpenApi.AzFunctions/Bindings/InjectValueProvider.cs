using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace SolidRpc.OpenApi.AzFunctions.Bindings
{
    /// <summary>
    /// 
    /// </summary>
    public class InjectValueProvider : IValueProvider, IValueBinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionInstanceId"></param>
        /// <param name="injectBinding"></param>
        /// <param name="scopedServiceProvider"></param>
        public InjectValueProvider(Guid functionInstanceId, InjectBinding injectBinding, IServiceProvider scopedServiceProvider)
        {
            FunctionInstanceId = functionInstanceId;
            InjectBinding = injectBinding;
            ScopedServiceProvider = scopedServiceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid FunctionInstanceId { get; }

        /// <summary>
        /// 
        /// </summary>
        public InjectBinding InjectBinding { get; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ScopedServiceProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type Type => InjectBinding.Type;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<object> GetValueAsync()
        {
            return Task.FromResult(ScopedServiceProvider.GetService(Type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetValueAsync(object value, CancellationToken cancellationToken)
        {
            InjectBinding.CleanupFunction(FunctionInstanceId);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToInvokeString()
        {
            return null;
        }
    }
}