using SolidRpc.Abstractions.OpenApi.Binder;
using SolidRpc.Abstractions.OpenApi.Transport;
using System.Collections.Generic;
using System.Reflection;
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
        /// Returns the transport that the handler uses
        /// </summary>
        ITransport GetTransport(IEnumerable<ITransport> transports);

        /// <summary>
        /// Invokes the supplied method
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<TRes> InvokeAsync<TRes>(MethodInfo mi, object[] args);

        /// <summary>
        /// Sends the httpRequest representing the call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodBinding"></param>
        /// <param name="transport"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<T> InvokeAsync<T>(
            IMethodBinding methodBinding, 
            ITransport transport, 
            object[] args);
    }
}
