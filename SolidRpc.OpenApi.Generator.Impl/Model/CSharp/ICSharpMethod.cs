using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Generator.Model.CSharp
{
    /// <summary>
    /// Represents a c# method
    /// </summary>
    public interface ICSharpMethod : ICSharpMember
    {
        /// <summary>
        /// The type that the method returns
        /// </summary>
        ICSharpType ReturnType { get; }

        /// <summary>
        /// Returns the method parameters
        /// </summary>
        IEnumerable<ICSharpMethodParameter> Parameters { get; }
    }
}
