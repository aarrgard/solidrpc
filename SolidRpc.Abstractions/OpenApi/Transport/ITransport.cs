using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// Base definitions for a transport
    /// </summary>
    public interface ITransport
    {
        /// <summary>
        /// The transport type. 
        /// </summary>
        string TransportType { get; }

        /// <summary>
        /// The operation address
        /// </summary>
        Uri OperationAddress { get; }

        /// <summary>
        /// The invocation strategy to use when handling calls
        /// on this transport.
        /// </summary>
        InvocationStrategy InvocationStrategy { get; }


        /// <summary>
        /// The pre invoke callback
        /// </summary>
        Func<IHttpRequest, Task> PreInvokeCallback { get; }

        /// <summary>
        /// The post invoke callback
        /// </summary>
        Func<IHttpResponse, Task> PostInvokeCallback { get; }


        /// <summary>
        /// Configures the transport for supplied binding.
        /// </summary>
        /// <param name="methodBinding"></param>
        void Configure(IMethodBinding methodBinding);
    }
}
