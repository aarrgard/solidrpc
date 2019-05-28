using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Swagger.Generator.Model.CSharp
{
    /// <summary>
    /// Represents a c# method
    /// </summary>
    public interface ICSharpProperty : ICSharpMember
    {
        /// <summary>
        /// The property type.
        /// </summary>
        ICSharpType PropertyType { get; set; }
    }
}
