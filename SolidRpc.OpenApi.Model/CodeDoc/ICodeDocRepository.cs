using System.Reflection;

namespace SolidRpc.OpenApi.Model.CodeDoc
{
    /// <summary>
    /// Defines support for getting code documentation.
    /// </summary>
    public interface ICodeDocRepository
    {
        /// <summary>
        /// Returns the documentation for supplied assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        ICodeDocAssembly GetAssemblyDoc(Assembly assembly);

        /// <summary>
        /// Returns the documentation for supplied method.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        ICodeDocMethod GetMethodDoc(MethodInfo methodInfo);
    }
}
