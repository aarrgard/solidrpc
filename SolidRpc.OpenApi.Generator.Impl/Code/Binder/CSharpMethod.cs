using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Generator.Code.Binder
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
        /// The interface the this method belongs to
        /// </summary>
        public QualifiedName InterfaceName { get; set; }

        /// <summary>
        /// The full class name
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// The method parameters.
        /// </summary>
        public IEnumerable<CSharpMethodParameter> Parameters { get; set; }

        /// <summary>
        /// The summary to use for the class.
        /// </summary>
        public string ClassSummary { get; set; }

        /// <summary>
        /// The summary for the method
        /// </summary>
        public string MethodSummary { get; set; }
    }
}
