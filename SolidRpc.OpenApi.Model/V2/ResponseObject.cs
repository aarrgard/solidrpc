﻿using System;
using System.Linq;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Describes a single response from an API Operation.
    /// </summary>
    /// <a href="https://swagger.io/specification/v2/#responseObject"/>
    public class ResponseObject : ModelBase
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="parent"></param>
        public ResponseObject(ModelBase parent) : base(parent)
        {

        }
        /// <summary>
        /// A definition of a GET operation on this path.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false, IsRequired = true)]
        public string Description { get; set; }

        /// <summary>
        /// A definition of the response structure. It can be a primitive, an array or an object. If this field does not exist, it means no content is returned as part of the response. As an extension to the Schema Object, its root type value may also be "file". This SHOULD be accompanied by a relevant produces mime-type.
        /// </summary>
        [DataMember(Name = "schema", EmitDefaultValue = false)]
        public SchemaObject Schema { get; set; }

        /// <summary>
        /// A list of headers that are sent with the response.
        /// </summary>
        [DataMember(Name = "headers", EmitDefaultValue = false)]
        public HeadersObject Headers { get; set; }

        /// <summary>
        /// A list of headers that are sent with the response.
        /// </summary>
        [DataMember(Name = "examples", EmitDefaultValue = false)]
        public ExampleObject examples { get; set; }

        /// <summary>
        /// The status
        /// </summary>
        public string GetStatus()
        {
            if (Parent is ResponsesObject defObj)
            {
                return defObj.Where(o => ReferenceEquals(o.Value, this)).First().Key;
            }
            else
            {
                throw new Exception("Cannot handle object type:" + Parent?.GetType().FullName);
            }
        }

        /// <summary>
        /// Set the return type based on supplied type
        /// </summary>
        /// <param name="type"></param>
        public void SetTypeInfo(Type type)
        {
            Description = $"The return type for {GetStatus()} responses.";
            Schema = SchemaObject.CreateSchemaObject(this, type);
        }
    }
}