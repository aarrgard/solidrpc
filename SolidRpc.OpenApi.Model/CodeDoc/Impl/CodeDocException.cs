using System;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    /// <summary>
    /// Represents an excpetion in the documentation
    /// </summary>
    public class CodeDocException : CodeDocMember, ICodeDocException
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="methodDocumentation"></param>
        /// <param name="cref"></param>
        /// <param name="comment"></param>
        public CodeDocException(CodeDocMethod methodDocumentation, string cref, string comment)
        {
            MethodDocumentation = methodDocumentation ?? throw new ArgumentNullException(nameof(methodDocumentation));
            ExceptionType = GetClassName(cref) ?? throw new ArgumentNullException(nameof(cref));
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }

        /// <summary>
        /// The method documentation
        /// </summary>
        public ICodeDocMethod MethodDocumentation { get; }

        /// <summary>
        /// The exception type
        /// </summary>
        public string ExceptionType { get; }

        /// <summary>
        /// The comment
        /// </summary>
        public string Comment { get; }

    }
}