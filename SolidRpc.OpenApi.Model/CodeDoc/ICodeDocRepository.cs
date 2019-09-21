using System;
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

        /// <summary>
        /// Returns the code documentation for supplied property
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        ICodeDocProperty GetPropertyDoc(PropertyInfo pi);

        /// <summary>
        /// Returns the class doc for supplied type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ICodeDocClass GetClassDoc(Type type);
    }
}
