using System.Runtime.Serialization;

namespace SolidRpc.Swagger.V2
{
    /// <summary>
    /// Describes a single response from an API Operation.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#responseObject"/>
    public class ResponseObject
    {
        /// <summary>
        /// A definition of a GET operation on this path.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// A definition of the response structure. It can be a primitive, an array or an object. If this field does not exist, it means no content is returned as part of the response. As an extension to the Schema Object, its root type value may also be "file". This SHOULD be accompanied by a relevant produces mime-type.
        /// </summary>
        [DataMember(Name = "schema")]
        public SchemaObject Schema { get; set; }

        /// <summary>
        /// A list of headers that are sent with the response.
        /// </summary>
        [DataMember(Name = "headers")]
        public HeadersObject Headers { get; set; }

        /// <summary>
        /// A list of headers that are sent with the response.
        /// </summary>
        [DataMember(Name = "examples")]
        public ExampleObject examples { get; set; }
    }
}