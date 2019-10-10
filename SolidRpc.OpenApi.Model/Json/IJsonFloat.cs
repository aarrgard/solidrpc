using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json
{
    /// <summary>
    /// Represents a Json string
    /// </summary>
    public interface IJsonFloat : IJsonStruct
    {
        /// <summary>
        /// Gets/sets the float value
        /// </summary>
        double Float { get; set; }
    }
}
