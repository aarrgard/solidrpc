namespace SolidRpc.OpenApi.Model.CodeDoc
{
    public interface ICodeDocField : ICodeDocMemberWithSummary
    {
        /// <summary>
        /// The class documentation that this method belongs to.
        /// </summary>
        ICodeDocClass ClassDocumentation { get; }

        /// <summary>
        /// Returns the code comments. These are the comments associated with the field.
        /// </summary>
        string CodeComments { get; }
    }
}