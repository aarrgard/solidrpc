﻿using System.Collections.Generic;


namespace SolidRpc.OpenApi.Model.Agnostic
{
    /// <summary>
    /// Represents a CSharp method
    /// </summary>
    public class CSharpMethod
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public CSharpMethod()
        {
            Exceptions = new CSharpObject[0];
            Parameters = new CSharpMethodParameter[0];
        }

        /// <summary>
        /// The exceptions rasised by this method
        /// </summary>
        public IEnumerable<CSharpObject> Exceptions { get; set; }

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
        public CSharpDescription ClassSummary { get; set; }

        /// <summary>
        /// The summary for the method
        /// </summary>
        public CSharpDescription MethodSummary { get; set; }

        /// <summary>
        /// The security attribute
        /// </summary>
        public IEnumerable<IDictionary<string, IEnumerable<string>>> SecurityAttribute { get; set; }
    }
}
