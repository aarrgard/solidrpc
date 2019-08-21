using SolidProxy.Core.Configuration;
using SolidProxy.Core.Configuration.Runtime;
using SolidProxy.Core.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceCall
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="callback"></param>
        public ServiceCall(MethodInfo methodInfo, Func<object[], object> callback)
        {
            MethodInfo = methodInfo;
            Callback = callback;
        }
        /// <summary>
        /// The call that we are intercepting
        /// </summary>
        public MethodInfo MethodInfo { get; }

        /// <summary>
        /// The callback to invoke.
        /// </summary>
        public Func<object[], object> Callback { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IServiceInterceptorAdviceConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// The service calls
        /// </summary>
        IList<ServiceCall> ServiceCalls { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TMethod"></typeparam>
    /// <typeparam name="TAdvice"></typeparam>
    public class ServiceInterceptorAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        private static Func<TMethod, Task<TAdvice>> s_TMethodToTAdviceConverter = TypeConverter.CreateConverter<TMethod, Task<TAdvice>>();
        public ServiceInterceptorAdvice()
        {

        }
        /// <summary>
        /// The service calls
        /// </summary>
        public IList<ServiceCall> ServiceCalls { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public void Configure(IServiceInterceptorAdviceConfig config)
        {
            if (ServiceCalls != null)
            {
                throw new Exception("Service calls exists");
            }
            ServiceCalls = config.ServiceCalls ?? new ServiceCall[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            var methodInfo = invocation.SolidProxyInvocationConfiguration.MethodInfo;
            var availableMethods = ServiceCalls.Select(o => o.MethodInfo.Name);
            invocation.ServiceProvider.LogTrace<ServiceInterceptorAdvice<TObject, TMethod, TAdvice>>($"Picking method({methodInfo.Name}) from available methods:{string.Join(",", availableMethods)}.");
            var serviceCall = ServiceCalls.FirstOrDefault(o => o.MethodInfo == methodInfo);
            if(serviceCall == null)
            {
                throw new Exception($"No service call registered for method({methodInfo.Name}). Available methods are {string.Join(",", availableMethods)}.");
            }
            var res = (TMethod)serviceCall.Callback(invocation.Arguments);
            return s_TMethodToTAdviceConverter.Invoke(res);
        }
    }
}
