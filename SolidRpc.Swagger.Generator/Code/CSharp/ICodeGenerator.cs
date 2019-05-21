using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    /// <summary>
    /// Interface to access the code generator.
    /// </summary>
    public interface ICodeGenerator : IMember
    {
        /// <summary>
        /// Returns the namespace
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        INamespace GetNamespace(string ns);

        /// <summary>
        /// Creates a generic class from supplied arguments.
        /// </summary>
        /// <param name="genericTypeDef"></param>
        /// <param name="classFullName"></param>
        /// <returns></returns>
        IClass CreateGenericType(string genericTypeDef, string classFullName);
    }
}
