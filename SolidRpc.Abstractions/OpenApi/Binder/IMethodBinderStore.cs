using System.Collections.Generic;
using System.Reflection;

namespace SolidRpc.Abstractions.OpenApi.Binder
{
    /// <summary>
    /// A store that contains bindings for an assebly and open api configuration.
    /// </summary>
    public interface IMethodBinderStore
    {
        /// <summary>
        /// Returns all the method binders.
        /// </summary>
        IEnumerable<IMethodBinder> MethodBinders { get; }

        /// <summary>
        /// Returns the binder for supplied open api spec and assembly
        /// </summary>
        /// <param name="openApiSpec"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IMethodBinder GetMethodBinder(string openApiSpec, Assembly assembly);

        /// <summary>
        /// Returns the method info for supplied open api spec and reflected method
        /// </summary>
        /// <param name="openApiSpec"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        IMethodInfo GetMethodInfo(string openApiSpec, MethodInfo methodInfo);
    }
}
