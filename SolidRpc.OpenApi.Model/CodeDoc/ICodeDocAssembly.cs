using System;
using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.CodeDoc
{
    /// <summary>
    /// Defines suport for accessing code doumentation for an assembly
    /// </summary>
    public interface ICodeDocAssembly
    {
        /// <summary>
        /// The assembly name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the class documentation.
        /// </summary>
        IEnumerable<ICodeDocClass> ClassDoumentation { get; }

        /// <summary>
        /// Returns the class documentation for supplied type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ICodeDocClass GetClassDocumentation(Type type);
    }
}