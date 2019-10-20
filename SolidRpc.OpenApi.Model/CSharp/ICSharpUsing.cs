using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// Represents a "using" clause.
    /// </summary>
    public interface ICSharpUsing : ICSharpMember
    {
        /// <summary>
        /// The namespace name
        /// </summary>
        string NsName { get; }
    }
}
