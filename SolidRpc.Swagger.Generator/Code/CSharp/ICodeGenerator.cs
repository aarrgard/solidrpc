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
    }
}
