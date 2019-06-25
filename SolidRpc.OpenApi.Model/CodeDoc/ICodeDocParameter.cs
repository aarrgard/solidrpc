namespace SolidRpc.OpenApi.Model.CodeDoc
{
    /// <summary>
    /// Defines support for accessing a parameter documentation.
    /// </summary>
    public interface ICodeDocParameter
    {
        /// <summary>
        /// The method documentation that this parameter belongs to.
        /// </summary>
        ICodeDocMethod MethodDocumentation { get; }

        /// <summary>
        /// The parameter name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The comment.
        /// </summary>
        string Comment { get; }
    }
}