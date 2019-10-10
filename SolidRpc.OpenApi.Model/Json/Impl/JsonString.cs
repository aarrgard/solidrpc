using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json.Impl
{
    /// <summary>
    /// Represents a Json string.
    /// </summary>
    public class JsonString : JsonStruct, IJsonString
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        public JsonString(IJsonStruct parent) : base(parent)
        {
        }

        /// <summary>
        /// The json string.
        /// </summary>
        public string String { get; set; }
    }
}
