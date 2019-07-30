using System;
using System.Threading.Tasks;
using SolidProxy.Core.Proxy;
using SolidRpc.Abstractions.OpenApi.Proxy;

namespace SolidRpc.OpenApi.Binder.Proxy
{
    /// <summary>
    /// Configures the bindings for the rpc proxy.
    /// </summary>
    public class SolidRpcOpenApiAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject : class
    {
        /// <summary>
        /// Configures this advice. By returning false this advice will not be added to the 
        /// adcvice pipeline.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool Configure(ISolidRpcOpenApiConfig config)
        {
            config.GetOpenApiConfiguration();
            return false;
        }
        public virtual Task<TAdvice> Handle(Func<Task<TAdvice>> next, ISolidProxyInvocation<TObject, TMethod, TAdvice> invocation)
        {
            throw new NotImplementedException("This advice should never be invoked.");
        }
    }
}
