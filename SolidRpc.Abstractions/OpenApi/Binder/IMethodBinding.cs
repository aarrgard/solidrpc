using SolidRpc.Abstractions.OpenApi.Http;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// Represents the method info structure in a swagger specification.
    /// </summary>
    public interface IMethodBinding
    {
        /// <summary>
        /// The binder that this method information belongs to.
        /// </summary>
        IMethodBinder MethodBinder { get; }

        /// <summary>
        /// The method info structure this binding represents.
        /// </summary>
        MethodInfo MethodInfo { get; }
        
        /// <summary>
        /// Returns the operation id for this method.
        /// </summary>
        string OperationId { get; }

        /// <summary>
        /// The method(POST,GET,HEAD,etc) to use when activating the method.
        /// </summary>
        string Method { get; }

        /// <summary>
        /// The base address to this method.
        /// </summary>
        Uri Address { get; set; }

        /// <summary>
        /// All the arguments.
        /// </summary>
        IEnumerable<IMethodArgument> Arguments { get; }

        /// <summary>
        /// Binds the arguments to the supplied request according to
        /// the swagger spec.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="args"></param>
        Task BindArgumentsAsync(IHttpRequest request, object[] args);

        /// <summary>
        /// Extracts the arguments from supplied request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object[]> ExtractArgumentsAsync(IHttpRequest request);

        /// <summary>
        /// Binds the response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="obj"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        Task BindResponseAsync(IHttpResponse response, object obj, Type objType);

        /// <summary>
        /// Returns the respone.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        T ExtractResponse<T>(IHttpResponse response);
    }
}