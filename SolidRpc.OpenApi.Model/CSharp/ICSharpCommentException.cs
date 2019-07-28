using System;

namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// Represents an exception
    /// </summary>
    public interface ICSharpCommentException
    {
        /// <summary>
        /// The description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The location of the document.
        /// </summary>
        string Cref { get; }
    }
}