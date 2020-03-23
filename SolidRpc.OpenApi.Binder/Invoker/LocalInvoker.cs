using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Invoker;
using SolidRpc.OpenApi.Binder.Invoker;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

[assembly: SolidRpc.Abstractions.SolidRpcAbstractionProvider(typeof(ILocalInvoker<>), typeof(LocalInvoker<>), ServiceLifetime.Scoped)]
namespace SolidRpc.OpenApi.Binder.Invoker
{
    /// <summary>
    /// Invoker that uses a local implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LocalInvoker<T> : Invoker<T>, ILocalInvoker<T> where T : class
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="methodBinderStore"></param>
        public LocalInvoker(ILogger<LocalInvoker<T>> logger, IMethodBinderStore methodBinderStore, IServiceProvider serviceProvider) : base(logger, methodBinderStore, serviceProvider)
        {
        }

        protected override Task<object> InvokeMethodAsync(Func<object, Task<object>> resultConverter, MethodInfo mi, object[] args)
        {
            var service = ServiceProvider.GetService(mi.DeclaringType);
            if(service == null)
            {
                throw new Exception("Cannot find service:" + mi.DeclaringType.FullName);
            }
            var proxy = service as ISolidProxy;
            if(proxy == null)
            {
                // the service is not proxied - invoke directly
                return resultConverter(mi.Invoke(service, args));
            }

            var methodBinding = MethodBinderStore.GetMethodBinding(mi);
            if(methodBinding == null)
            {
                // service does not have a openapi specification - invoke directly
                return resultConverter(mi.Invoke(service, args));
            }

            // set securiity key if needed.
            IDictionary<string, object> invocationValues = null;
            var securityKey = methodBinding.SecurityKey;
            if (securityKey != null)
            {
                invocationValues = new Dictionary<string, object>()
                {
                    { securityKey.Value.Key, new StringValues(securityKey.Value.Value) }
                };

            }
            return resultConverter(proxy.Invoke(mi, args, invocationValues));
        }
    }
}
