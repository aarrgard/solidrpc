using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Transport;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Invoker
{
    /// <summary>
    /// Contains additional invocation options.
    /// </summary>
    public class InvocationOptions
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
        /// The high message prio
        /// </summary>
        public const int MessagePriorityHigh = 3;
        /// <summary>
        /// The normal message prio
        /// </summary>
        public const int MessagePriorityNormal = 5;
        /// <summary>
        /// The low message prio
        /// </summary>
        public const int MessagePriorityLow = 7;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="transportType"></param>
        /// <param name="priority"></param>
        /// <param name="preInvokeCallback"></param>
        /// <param name="postInvokeCallback"></param>
        public InvocationOptions(string transportType, int priority, Func<IHttpRequest, Task> preInvokeCallback = null, Func<IHttpResponse, Task> postInvokeCallback = null)
        {
            TransportType = transportType;
            Priority = priority;
            PreInvokeCallback = preInvokeCallback ?? DefaultPreInvokeCallback;
            PostInvokeCallback = postInvokeCallback ?? DefaultPostInvokeCallback;
        }

        /// <summary>
        /// The preferred transport type. Defaults to "Http"
        /// </summary>
        public string TransportType { get; }

        /// <summary>
        /// The invocation priority.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// The pre invoke callback
        /// </summary>
        public Func<IHttpRequest, Task> PreInvokeCallback { get; }

        /// <summary>
        /// The post invoke callback
        /// </summary>
        public Func<IHttpResponse, Task> PostInvokeCallback { get; }

        /// <summary>
        /// Returns a copy of this instance with another priority.
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        public InvocationOptions SetPriority(int priority)
        {
            return new InvocationOptions(TransportType, priority, PreInvokeCallback, PostInvokeCallback);
        }

        /// <summary>
        /// Sets the transport to use.
        /// </summary>
        /// <param name="transportType"></param>
        /// <returns></returns>
        public InvocationOptions SetTransport(string transportType)
        {
            return new InvocationOptions(transportType, Priority, PreInvokeCallback, PostInvokeCallback);
        }

        /// <summary>
        /// Adds a pre invokation callback
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public InvocationOptions AddPreInvokeCallback(Func<IHttpRequest, Task> callback)
        {
            var oldCallback = PreInvokeCallback;
            return new InvocationOptions(TransportType, Priority, async (req) =>
            {
                await callback(req);
                await oldCallback(req);
            }, PostInvokeCallback);
        }

        /// <summary>
        /// Adds a pre invokation callback
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public InvocationOptions AddPostInvokeCallback(Func<IHttpResponse, Task> callback)
        {
            var oldCallback = PostInvokeCallback;
            return new InvocationOptions(TransportType, Priority, PreInvokeCallback, async (resp) =>
            {
                await callback(resp);
                await oldCallback(resp);
            });
        }
    }
}
