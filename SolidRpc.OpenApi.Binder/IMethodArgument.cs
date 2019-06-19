using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolidRpc.OpenApi.Binder
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
        /// Binds this argument to the supplied request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="val"></param>
        Task BindArgumentAsync(IHttpRequest request, object val);

        Task<object> ExtractArgumentAsync(IHttpRequest request);
    }
}