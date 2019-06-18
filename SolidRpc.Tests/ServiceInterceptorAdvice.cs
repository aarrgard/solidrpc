using SolidProxy.Core.Configuration;
using SolidProxy.Core.Proxy;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceInterceptorAdviceConfig : ISolidProxyInvocationAdviceConfig
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TMethod"></typeparam>
    /// <typeparam name="TAdvice"></typeparam>
    public class ServiceInterceptorAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public void Configure(IServiceInterceptorAdviceConfig config)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            throw new NotImplementedException();
        }
    }
}
