using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    /// <summary>
    /// Interface to access the code generator.
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// Returns the namespace
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        INamespace GetNamespace(string ns);

        /// <summary>
        /// Writes all the namespaces to supplied code writer.
        /// </summary>
        /// <param name="codeWriter"></param>
        void WriteCode(ICodeWriter codeWriter);
    }
}
