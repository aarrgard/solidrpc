using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json.Impl
{
    /// <summary>
    /// Represents a Json object.
    /// </summary>
    public class JsonStruct : IJsonStruct
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        public JsonStruct(IJsonStruct parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// The parent structure
        /// </summary>
        public IJsonStruct Parent { get; }
    }
}
