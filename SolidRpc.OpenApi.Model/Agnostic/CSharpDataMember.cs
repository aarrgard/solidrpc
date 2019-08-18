using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Agnostic
{
    /// <summary>
    /// Represents a "DataMember" attribute.
    /// </summary>
    public class CSharpDataMember
    {
        /// <summary>
        /// The name of the serialized data
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The order of the data member.
        /// </summary>
        public int Order { get; set; }
    }
}
