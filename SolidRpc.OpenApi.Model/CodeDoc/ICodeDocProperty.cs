namespace SolidRpc.OpenApi.Model.CodeDoc
{
    /// <summary>
    /// Defines suport for accessing the documentation for a property.
    /// </summary>
    public interface ICodeDocProperty
    {
        /// <summary>
        /// The property name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The comment.
        /// </summary>
        string Comment { get; }
    }
}