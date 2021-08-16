using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Transport;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Invoker
{
    /// <summary>
    /// The invocation handler is responsible for doing
    /// the actual invokation
    /// </summary>
    public interface ITransportHandler
    {
        /// <summary>
        /// The transport type that this handler uses.
        /// </summary>
        string TransportType { get; }

        /// <summary>
        /// Invokes the method
        /// </summary>
        /// <param name="methodBinding"></param>
        /// <param name="transport"></param>
        /// <param name="httpReq"></param>
        /// <param name="invocationOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IHttpResponse> InvokeAsync(
            IMethodBinding methodBinding, 
            ITransport transport, 
            IHttpRequest httpReq, 
            InvocationOptions invocationOptions, 
            CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Invokes the supplied method
        /// </summary>
        /// <param name="methodBinding"></param>
        /// <param name="target"></param>
        /// <param name="mi"></param>
        /// <param name="args"></param>
        /// <param name="invocationOptions"></param>
        /// <returns></returns>
        Task<object> InvokeAsync(
            IMethodBinding methodBinding,
            object target,
            MethodInfo mi,
            object[] args,
            InvocationOptions invocationOptions);

        /// <summary>
        /// Sends the httpRequest representing the call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodBinding"></param>
        /// <param name="transport"></param>
        /// <param name="args"></param>
        /// <param name="invocationOptions"></param>
        /// <returns></returns>
        Task<T> InvokeAsync<T>(
            IMethodBinding methodBinding, 
            ITransport transport, 
            object[] args,
            InvocationOptions invocationOptions);
    }

    /// <summary>
    /// Represents a transport handler for supplied configuration
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
    public interface ITransportHandler<TConfig> : ITransportHandler where TConfig : ITransport
    {
        /// <summary>
        /// Configures the supplied transport
        /// </summary>
        /// <param name="methodBinding"></param>
        /// <param name="transport"></param>
        void Configure(IMethodBinding methodBinding, TConfig transport);

    }
}
