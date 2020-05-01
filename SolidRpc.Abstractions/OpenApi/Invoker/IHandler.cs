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
    public interface IHandler
    {
        /// <summary>
        /// The transport type that this handler uses.
        /// </summary>
        string TransportType { get; }

        /// <summary>
        /// Invokes the method
        /// </summary>
        /// <typeparam name="TResp"></typeparam>
        /// <param name="methodBinding"></param>
        /// <param name="transport"></param>
        /// <param name="httpReq"></param>
        /// <param name="invocationOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IHttpResponse> InvokeAsync<TResp>(
            IMethodBinding methodBinding, 
            ITransport transport, 
            IHttpRequest httpReq, 
            InvocationOptions invocationOptions, 
            CancellationToken cancellationToken = default(CancellationToken));


        /// <summary>
        /// Invokes the supplied method
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="args"></param>
        /// <param name="invocationOptions"></param>
        /// <returns></returns>
        Task<TRes> InvokeAsync<TRes>(
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
}
