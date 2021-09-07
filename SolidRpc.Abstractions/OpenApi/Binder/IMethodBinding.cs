using SolidProxy.Core.Configuration;
using SolidRpc.Abstractions.OpenApi.Http;
using SolidRpc.Abstractions.OpenApi.Transport;
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
        /// Returns the configuration for specified advice.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetSolidProxyConfig<T>() where T : class, ISolidProxyInvocationAdviceConfig;

        /// <summary>
        /// Returns true if the underlying method ends in an 
        /// InvocationAdvice. False otherwise.
        /// </summary>
        bool IsLocal { get; }

        /// <summary>
        /// Returns true if this method is enabled
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Returns the configured transports orderd by the invocation ordinal.
        /// </summary>
        IEnumerable<ITransport> Transports { get; }

        /// <summary>
        /// Returns the operation id for this method.
        /// </summary>
        string OperationId { get; }

        /// <summary>
        /// The method(POST,GET,HEAD,etc) to use when activating the method.
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Returns the local path.(Represents a Uri.LocalPath ie not url encoded AbsolutePath)
        /// </summary>
        string LocalPath { get; }

        /// <summary>
        /// Returns the relative path of the operation. Relative to the base url and does not start with a slash.
        /// </summary>
        string RelativePath { get; }

        /// <summary>
        /// All the arguments.
        /// </summary>
        IEnumerable<IMethodArgument> Arguments { get; }

        /// <summary>
        /// Creates a uri with the path and query arguments from the supplied request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="addressOverride"></param>
        /// <returns></returns>
        Uri BindUri(IHttpRequest request, Uri addressOverride = null);

        /// <summary>
        /// Binds the arguments to the supplied request according to
        /// the swagger spec.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="args"></param>
        /// <param name="addressOverride"></param>
        Task BindArgumentsAsync(IHttpRequest request, object[] args, Uri addressOverride = null);

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

        /// <summary>
        /// Returns the respone.
        /// </summary>
        /// <param name="responseType"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        object ExtractResponse(Type responseType, IHttpResponse response);
    }
}