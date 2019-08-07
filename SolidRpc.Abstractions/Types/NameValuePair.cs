using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Represents a name/value pair
    /// </summary>
    public class NameValuePair
    {
        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value
        /// </summary>
        public string Value { get; set; }
    }
}
