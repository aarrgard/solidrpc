using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Generator.Code.Binder
{
    /// <summary>
    /// Represents a CSharp method
    /// </summary>
    public class CSharpMethod
    {

        /// <summary>
        /// The return type.
        /// </summary>
        public CSharpObject ReturnType { get; set; }

        /// <summary>
        /// The full class name
        /// </summary>
        public QualifiedName ClassName { get; set; }

        /// <summary>
        /// The full class name
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// The method parameters.
        /// </summary>
        public IEnumerable<CSharpMethodParameter> Parameters { get; set; }
    }
}
