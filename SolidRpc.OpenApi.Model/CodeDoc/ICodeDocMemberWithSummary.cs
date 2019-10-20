using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.CodeDoc
{
    /// <summary>
    /// A code member with a summary
    /// </summary>
    public interface ICodeDocMemberWithSummary
    {

        /// <summary>
        /// Returns the summary
        /// </summary>
        string Summary { get; }
    }
}
