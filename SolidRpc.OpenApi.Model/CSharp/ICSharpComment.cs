using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// Represents a comment
    /// </summary>
    public interface ICSharpComment
    {
        /// <summary>
        /// The summary
        /// </summary>
        string Summary { get; }
        
        /// <summary>
        /// The external doc
        /// </summary>
        ICSharpCommentExternalDoc ExternalDoc { get; }

        /// <summary>
        /// The exceptions
        /// </summary>
        IEnumerable<ICSharpCommentException> Exceptions { get; }
    }
}
