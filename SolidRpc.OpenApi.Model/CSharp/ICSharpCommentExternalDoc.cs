using System;

namespace SolidRpc.OpenApi.Model.CSharp
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
        string Url { get; }
    }
}