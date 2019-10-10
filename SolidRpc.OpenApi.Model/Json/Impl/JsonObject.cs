using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SolidRpc.OpenApi.Model.Json.Impl
{
    /// <summary>
    /// Represents a Json object.
    /// </summary>
    public class JsonObject : JsonStruct, IJsonObject
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        public JsonObject(IJsonStruct parent) : base(parent)
        {
            Properties = new Dictionary<string, IJsonStruct>();
        }

        private Dictionary<string, IJsonStruct> Properties { get; }

        /// <summary>
        /// Sets or gets the property.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IJsonStruct this[string name] { 
            get 
            {
                return Properties[name];
            } 
            set 
            {
                Properties[name] = value;
            } 
        }

        public T AsObject<T>() where T : IJsonObject
        {
            var proxy = DispatchProxy.Create<T, ObjectProxy<T>>();
            ((ObjectProxy<T>)(object)proxy).JsonObject = this;
            return proxy;
        }
    }
}
