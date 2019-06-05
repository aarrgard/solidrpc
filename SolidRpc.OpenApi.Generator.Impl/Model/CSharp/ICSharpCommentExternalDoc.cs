using System;

namespace SolidRpc.OpenApi.Generator.Model.CSharp
{
    /// <summary>
    /// Represents an external document
    /// </summary>
    public interface ICSharpCommentExternalDoc
    {
        /// <summary>
        /// The description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The location of the document.
        /// </summary>
        Uri Url { get; }
    }
}