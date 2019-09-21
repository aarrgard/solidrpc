namespace SolidRpc.OpenApi.Model.CodeDoc
{
    /// <summary>
    /// Defines suport for accessing the documentation for a property.
    /// </summary>
    public interface ICodeDocProperty
    {
        /// <summary>
        /// The class documentation that this property belongs to.
        /// </summary>
        ICodeDocClass ClassDocumentation { get; }

        /// <summary>
        /// The property name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the comments for this property.
        /// </summary>
        string CodeComments { get; }

        /// <summary>
        /// The summary in the comment
        /// </summary>
        string Summary { get; }
    }
}