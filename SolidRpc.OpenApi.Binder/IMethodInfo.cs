using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder
{
    /// <summary>
    /// Represents the method info structure in a swagger specification.
    /// </summary>
    public interface IMethodInfo
    {
        /// <summary>
        /// The method info structure this binding represents.
        /// </summary>
        MethodInfo MethodInfo { get; }
        
        /// <summary>
        /// Returns the operation id for this method.
        /// </summary>
        string OperationId { get; }

        /// <summary>
        /// The path to thes method. This includes the basepath and the path element.
        /// </summary>
        string Path { get; }

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
        /// Returns the respone.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        T GetResponse<T>(IHttpResponse response);
    }
}