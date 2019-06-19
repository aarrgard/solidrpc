using SolidProxy.Core.Proxy;
using SolidRpc.OpenApi.Binder;
using System;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.AspNetCore
{
    /// <summary>
    /// This advice is used to setup the proxy. The advice is never invoked but the proxy
    /// configuration of the IoC container does not wrap implementations with proxies unless
    /// there is an active advice.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TMethod"></typeparam>
    /// <typeparam name="TAdvice"></typeparam>
    public class SolidRpcAspNetCoreAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        public IMethodInfo MethodInfo { get; private set; }

        public bool Configure(ISolidRpcAspNetCoreConfig config)
        {
            return false;
        }

        /// <summary>
        /// Handles the invocation
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            throw new Exception("This advice should never be invoked.");
        }
    }
}
