namespace SolidRpc.OpenApi.Model.CodeDoc
{
    /// <summary>
    /// Defines support for accessing documentation regarding the exception
    /// </summary>
    public interface ICodeDocException
    {
        /// <summary>
        /// The method documentation that this parameter belongs to.
        /// </summary>
        ICodeDocMethod MethodDocumentation { get; }

        /// <summary>
        /// The exception type
        /// </summary>
        string ExceptionType { get; }


        /// <summary>
        /// The comment.
        /// </summary>
        string Comment { get; }
    }
}