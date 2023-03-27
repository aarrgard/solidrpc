using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.CSharp
{
    /// <summary>
    /// Represents a c# method
    /// </summary>
    public interface ICSharpProperty : ICSharpMember
    {
        /// <summary>
        /// The property type.
        /// </summary>
        ICSharpType PropertyType { get; }

        /// <summary>
        /// Can we read this property
        /// </summary>
        bool CanRead { get; }

        /// <summary>
        /// Can we write this property
        /// </summary>
        bool CanWrite { get; }

        /// <summary>
        /// The getter
        /// </summary>
        StringBuilder Getter { get; }

        /// <summary>
        /// The setter
        /// </summary>
        StringBuilder Setter { get; }
    }
}
