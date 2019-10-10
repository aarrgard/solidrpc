using System;
using System.Collections.Generic;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json
{
    /// <summary>
    /// Represents a Json object
    /// </summary>
    public interface IJsonObject : IJsonStruct
    {
        /// <summary>
        /// Returns the json structure for supplied name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IJsonStruct this[string name] { get; set; }

        /// <summary>
        /// Returns a proxy that represents the underlying structure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T AsObject<T>() where T : IJsonObject;
    }
}
