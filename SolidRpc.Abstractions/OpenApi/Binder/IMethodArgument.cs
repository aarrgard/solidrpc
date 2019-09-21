using SolidRpc.Abstractions.OpenApi.Http;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// Represents a method argument.
    /// </summary>
    public interface IMethodArgument
    {
        /// <summary>
        /// Returns the name of the argument.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Specifies where this argument is located.
        /// </summary>
        IEnumerable<string> ArgumentPath { get; }

        /// <summary>
        /// The location of the argument, May be either:
        /// * path
        /// * query
        /// * header
        /// * formData
        /// * body (one)
        /// * body-inline(several)
        /// The formData, body and body-inline cannot be combined.
        /// </summary>
        string Location { get; }

        /// <summary>
        /// The parameter info.
        /// </summary>
        ParameterInfo ParameterInfo { get; }

        /// <summary>
        /// Specifies if this argument is optional
        /// </summary>
        bool Optional { get; }

        /// <summary>
        /// Binds this argument to the supplied request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="val"></param>
        Task BindArgumentAsync(IHttpRequest request, object val);

        /// <summary>
        /// Extracts the arguments from supplied request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> ExtractArgumentAsync(IHttpRequest request);
    }
}