using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Transport
{
    /// <summary>
    /// Represents the settings for the http transport
    /// </summary>
    public interface IHttpTransport : ITransport
    {
        /// <summary>
        /// The base address of the method.
        /// </summary>
        Uri BaseAddress { get; }

        /// <summary>
        /// The path relative to the base method.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// The method to transform the Uri. This delegate is invoked to determine
        /// the base Uri for the service. Supplied uri is the one obtained from
        /// the openapi config.
        /// </summary>
        MethodAddressTransformer MethodAddressTransformer { get; }

        /// <summary>
        /// Transforms the invocation address before it is invoked
        /// </summary>
        /// <param name="methodAddressTransformer"></param>
        /// <returns></returns>
        IHttpTransport SetMethodAddressTransformer(MethodAddressTransformer methodAddressTransformer);
        
        /// <summary>
        /// Sets the invocation strategy
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IHttpTransport SetInvocationStrategy(InvocationStrategy value);

        /// <summary>
        /// Adds a pre invoke callback
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        IHttpTransport AddPreInvokeCallback(Func<IHttpRequest, Task> callback);

        /// <summary>
        /// Adds a post invoke callback
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        IHttpTransport AddPostInvokeCallback(Func<IHttpResponse, Task> callback);



    }
}