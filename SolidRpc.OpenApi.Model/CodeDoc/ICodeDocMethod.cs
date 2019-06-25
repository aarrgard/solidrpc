using System.Collections.Generic;

namespace SolidRpc.OpenApi.Model.CodeDoc
{
    /// <summary>
    /// Defines support for accessing documentation for a method
    /// </summary>
    public interface ICodeDocMethod
    {
        /// <summary>
        /// The class documentation that this method belongs to.
        /// </summary>
        ICodeDocClass ClassDocumentation { get; }

        /// <summary>
        /// The method name.
        /// </summary>
        string MethodName { get; }

        /// <summary>
        /// The comment.
        /// </summary>
        string Comment { get; }

        /// <summary>
        /// The parameter documentation.
        /// </summary>
        IEnumerable<ICodeDocParameter> ParameterDocumentation { get; }

        /// <summary>
        /// The parameter documentation.
        /// </summary>
        IEnumerable<ICodeDocException> ExceptionDocumentation { get; }
    }
}