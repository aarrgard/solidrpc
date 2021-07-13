using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Transport.Impl
{
    /// <summary>
    /// Base type for the transports
    /// </summary>
    public abstract class Transport : ITransport
    {
        /// <summary>
        /// The defaullt pre invoke callback(Does nothing)
        /// </summary>
        /// <param name="httpReq"></param>
        /// <returns></returns>
        private static Task DefaultPreInvokeCallback(IHttpRequest httpReq)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// The defaullt post invoke callback(Does nothing)
        /// </summary>
        /// <param name="httpReq"></param>
        /// <returns></returns>
        private static Task DefaultPostInvokeCallback(IHttpResponse httpReq)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="transportType"></param>
        /// <param name="invocationStrategy"></param>
        /// <param name="messagePriority"></param>
        /// <param name="preInvokeCallback"></param>
        /// <param name="postInvokeCallback"></param>
        public Transport(
            string transportType, 
            InvocationStrategy invocationStrategy,
            int messagePriority,
            Func<IHttpRequest, Task> preInvokeCallback,
            Func<IHttpResponse, Task> postInvokeCallback)
        {
            TransportType = transportType ?? throw new ArgumentNullException(nameof(transportType));
            MessagePriority = messagePriority;
            InvocationStrategy = invocationStrategy;
            PreInvokeCallback = preInvokeCallback ?? DefaultPreInvokeCallback;
            PostInvokeCallback = postInvokeCallback ?? DefaultPostInvokeCallback;
        }

        /// <summary>
        /// The transport type.
        /// </summary>
        public string TransportType { get; }

        /// <summary>
        /// The transport type.
        /// </summary>
        public int MessagePriority { get; }

        /// <summary>
        /// Returns the operation address
        /// </summary>
        public abstract Uri OperationAddress { get; }

        /// <summary>
        /// Returns the invocation strategy
        /// </summary>
        public InvocationStrategy InvocationStrategy { get; }

        /// <summary>
        /// The pre invoke callbacks
        /// </summary>
        public Func<IHttpRequest, Task> PreInvokeCallback { get; }

        /// <summary>
        /// The post invoke callbacks
        /// </summary>
        public Func<IHttpResponse, Task> PostInvokeCallback { get; }

        /// <summary>
        /// Method that can be overridden to configure a transport.
        /// </summary>
        /// <param name="methodBinding"></param>
        public virtual void Configure(IMethodBinding methodBinding)
        {
        }
    }
}
