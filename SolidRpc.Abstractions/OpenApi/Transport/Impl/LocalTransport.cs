using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Invoker;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Transport.Impl
{
    /// <summary>
    /// Contains the settings for the queue transport.
    /// </summary>
    public class LocalTransport : Transport, ILocalTransport
    {
        /// <summary>
        /// The default instance
        /// </summary>
        public static ITransport Instance { get; } = new LocalTransport(InvocationStrategy.Invoke, InvocationOptions.MessagePriorityNormal);

        /// <summary>
        /// Represents a queue transport
        /// </summary>
        /// <param name="invocationStrategy"></param>
        /// <param name="messagePriority"></param>
        /// <param name="preInvokeCallback"></param>
        /// <param name="postInvokeCallback"></param>
        public LocalTransport(InvocationStrategy invocationStrategy,
            int messagePriority,
            Func<IHttpRequest, Task> preInvokeCallback = null,
            Func<IHttpResponse, Task> postInvokeCallback = null)
            : base("Local", invocationStrategy, messagePriority, preInvokeCallback, postInvokeCallback)
        {
        }


        /// <summary>
        /// Returns the operation address
        /// </summary>
        public override Uri OperationAddress => null;

        /// <summary>
        ///  configures this transport
        /// </summary>
        /// <param name="methodBinding"></param>
        public override void Configure(IMethodBinding methodBinding)
        {
        }
    }
}
