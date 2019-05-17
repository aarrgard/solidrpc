using System.Collections.Generic;

namespace SolidRpc.Swagger.Binder
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
        IEnumerable<string> Path { get; }

        /// <summary>
        /// Binds this argument to the supplied request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="val"></param>
        void BindArgument(IHttpRequest request, object val);
    }
}