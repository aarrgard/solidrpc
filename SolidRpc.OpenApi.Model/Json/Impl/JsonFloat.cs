using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json.Impl
{
    /// <summary>
    /// Represents a Json float.
    /// </summary>
    public class JsonFloat : JsonStruct, IJsonFloat
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        public JsonFloat(IJsonStruct parent) : base(parent)
        {
        }

        /// <summary>
        /// The json float.
        /// </summary>
        public double Float { get; set; }
    }
}
