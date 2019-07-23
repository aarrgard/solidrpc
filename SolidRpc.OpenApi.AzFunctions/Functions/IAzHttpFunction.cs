using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.AzFunctions.Functions
{
    /// <summary>
    /// Represents a timer function
    /// </summary>
    public interface IAzHttpFunction : IAzFunction
    {
        /// <summary>
        /// The path
        /// </summary>
        string Route { get; set; }

        /// <summary>
        /// The methods
        /// </summary>
        IEnumerable<string> Methods { get; set; }
    }
}
