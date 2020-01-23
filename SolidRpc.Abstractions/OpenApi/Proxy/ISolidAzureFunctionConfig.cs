using SolidProxy.Core.Configuration;
using SolidProxy.Core.Proxy;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Proxy
{
    /// <summary>
    /// Interface used to determine how the azure function
    /// settings should be.
    /// </summary>
    public interface ISolidAzureFunctionConfig : ISolidProxyInvocationAdviceConfig
    {
        /// <summary>
        /// The authorization level.
        /// </summary>
        string AuthLevel { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TMethod"></typeparam>
    /// <typeparam name="TAdvice"></typeparam>
    public class SolidAzureFunctionAdvice<TObject, TMethod, TAdvice> : ISolidProxyInvocationAdvice<TObject, TMethod, TAdvice> where TObject:class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool Configure(ISolidAzureFunctionConfig config)
        {
            return false;
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
