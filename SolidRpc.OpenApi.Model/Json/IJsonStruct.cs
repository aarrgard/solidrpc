using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json
{
    /// <summary>
    /// base class for all Json strutures
    /// </summary>
    public interface IJsonStruct
    {
        /// <summary>
        /// Returns the parent structure.
        /// </summary>
        IJsonStruct Parent { get; }

    }
}
