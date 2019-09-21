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
        string Summary { get; }

        /// <summary>
        /// The parameter documentation.
        /// </summary>
        IEnumerable<ICodeDocParameter> ParameterDocumentation { get; }

        /// <summary>
        /// The parameter documentation.
        /// </summary>
        IEnumerable<ICodeDocException> ExceptionDocumentation { get; }

        /// <summary>
        /// Returns the comments for this method.
        /// </summary>
        string CodeComments { get; }

        /// <summary>
        /// Returns the parameter documentation for supplied paramter name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ICodeDocParameter GetParameterDocumentation(string name);
    }
}