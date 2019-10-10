using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json.Impl
{
    /// <summary>
    /// Represents a Json integer.
    /// </summary>
    public class JsonInteger : JsonStruct, IJsonInteger
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        public JsonInteger(IJsonStruct parent) : base(parent)
        {
        }

        /// <summary>
        /// The json integer.
        /// </summary>
        public long Integer { get; set; }
    }
}
