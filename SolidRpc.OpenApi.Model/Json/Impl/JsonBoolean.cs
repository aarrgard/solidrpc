using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json.Impl
{
    /// <summary>
    /// Represents a Json boolean.
    /// </summary>
    public class JsonBoolean : JsonStruct, IJsonBoolean
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        public JsonBoolean(IJsonStruct parent) : base(parent)
        {
        }

        /// <summary>
        /// The json boolean.
        /// </summary>
        public bool Boolean { get; set; }
    }
}
