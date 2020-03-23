using SolidProxy.Core.Configuration;
using SolidProxy.Core.Proxy;
using System;
using System.Collections.Generic;
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
        /// The protocols tha the function should use.
        /// "http", "queue", etc.
        /// </summary>
        ICollection<string> Protocols { get; }

        /// <summary>
        /// The authorization level in the http protocol
        /// "anonymous", "admin", "function"
        /// </summary>
        string HttpAuthLevel { get; set; }

        /// <summary>
        /// The name of the queue to poll - if not set it will be based on the openapi operation.
        /// </summary>
        string QueueName { get; set; }

        /// <summary>
        /// The the connection string to connect to the queue.
        /// </summary>
        string QueueConnection { get; set; }
    }

    /// <summary>
    /// We nned the advice for the configuration...
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
