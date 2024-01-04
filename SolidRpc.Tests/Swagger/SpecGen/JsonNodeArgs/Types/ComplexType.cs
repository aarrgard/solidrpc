using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.Tests.Swagger.SpecGen.JsonNodeArgs.Types
{
    /// <summary>
    /// The complex type
    /// </summary>
    public class ComplexType
    {
        /// <summary>
        /// Some string data
        /// </summary>
        public string StringData { get; set; }

        /// <summary>
        /// The JsonNode
        /// </summary>
        public JsonNode JsonNode { get; set; }
    }
}
