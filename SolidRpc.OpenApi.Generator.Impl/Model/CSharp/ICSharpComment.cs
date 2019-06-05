using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Generator.Model.CSharp
{
    /// <summary>
    /// Represents a comment
    /// </summary>
    /// <see cref=""/>
    public interface ICSharpComment
    {
        /// <summary>
        /// The summary
        /// </summary>
        string Summary { get; }
        
        ICSharpCommentExternalDoc ExternalDoc { get; }
    }
}
