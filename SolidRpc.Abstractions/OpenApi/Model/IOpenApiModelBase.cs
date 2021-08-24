using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.OpenApi.Model
{
    /// <summary>
    /// Represents a model object
    /// </summary>
    public interface IOpenApiModelBase
    {
        /// <summary>
        /// Returns the parent node.
        /// </summary>
        IOpenApiModelBase Parent { get; }

        /// <summary>
        /// Sets the parent node.
        /// </summary>
        void SetParent(IOpenApiModelBase parent);
    }
}
