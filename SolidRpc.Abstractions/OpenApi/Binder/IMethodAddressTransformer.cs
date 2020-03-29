using System;
using System.Reflection;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// Interface that can be implemented to resolve the base uri.
    /// </summary>
    public interface IMethodAddressTransformer
    {
        /// <summary>
        /// Returns the uri for supplied method. If no method is supplied
        /// the base address for the open api spec is determined.
        /// </summary>
        Uri TransformUriAsync(Uri uri, MethodInfo methodInfo = null);
    }
}
