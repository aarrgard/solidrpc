using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.Swagger.Model.V2
{
    /// <summary>
    /// Describes a single API operation on a path.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#operationObject"/>
    public class OperationObject : ModelBase
    {
        public OperationObject(ModelBase parent) : base(parent) { }
        /// <summary>
        /// A list of tags for API documentation control. Tags can be used for logical grouping of operations by resources or any other qualifier.
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// A short summary of what the operation does. For maximum readability in the swagger-ui, this field SHOULD be less than 120 characters.
        /// </summary>
        [DataMember(Name = "summary", EmitDefaultValue = false)]
        public string Summary { get; set; }

        /// <summary>
        /// A verbose explanation of the operation behavior. GFM syntax can be used for rich text representation.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Additional external documentation for this operation.
        /// </summary>
        [DataMember(Name = "externalDocs", EmitDefaultValue = false)]
        public ExternalDocumentationObject ExternalDocs { get; set; }

        /// <summary>
        /// Unique string used to identify the operation. The id MUST be unique among all operations described in the API. Tools and libraries MAY use the operationId to uniquely identify an operation, therefore, it is recommended to follow common programming naming conventions.
        /// </summary>
        [DataMember(Name = "operationId", EmitDefaultValue = false)]
        public string OperationId { get; set; }

        /// <summary>
        /// A list of MIME types the operation can consume. This overrides the consumes definition at the Swagger Object. An empty value MAY be used to clear the global definition. Value MUST be as described under Mime Types.
        /// </summary>
        [DataMember(Name = "consumes", EmitDefaultValue = false)]
        public IEnumerable<string> Consumes { get; set; }

        /// <summary>
        /// A list of MIME types the operation can produce. This overrides the produces definition at the Swagger Object. An empty value MAY be used to clear the global definition. Value MUST be as described under Mime Types.
        /// </summary>
        [DataMember(Name = "produces", EmitDefaultValue = false)]
        public IEnumerable<string> Produces { get; set; }

        /// <summary>
        /// A list of parameters that are applicable for this operation. If a parameter is already defined at the Path Item, the new definition will override it, but can never remove it. The list MUST NOT include duplicated parameters. A unique parameter is defined by a combination of a name and location. The list can use the Reference Object to link to parameters that are defined at the Swagger Object's parameters. There can be one "body" parameter at most.
        /// </summary>
        [DataMember(Name = "parameters", EmitDefaultValue = false)]
        public IEnumerable<ParameterObject> Parameters { get; set; }

        /// <summary>
        /// Required. The list of possible responses as they are returned from executing this operation.
        /// </summary>
        [DataMember(Name = "responses", EmitDefaultValue = false)]
        public ResponsesObject Responses { get; set; }

        /// <summary>
        /// The transfer protocol for the operation. Values MUST be from the list: "http", "https", "ws", "wss". The value overrides the Swagger Object schemes definition.
        /// </summary>
        [DataMember(Name = "schemes", EmitDefaultValue = false)]
        public IEnumerable<string> Schemes { get; set; }

        /// <summary>
        /// Declares this operation to be deprecated. Usage of the declared operation should be refrained. Default value is false.
        /// </summary>
        [DataMember(Name = "deprecated", EmitDefaultValue = false)]
        public bool Deprecated { get; set; }

        /// <summary>
        /// A declaration of which security schemes are applied for this operation. The list of values describes alternative security schemes that can be used (that is, there is a logical OR between the security requirements). This definition overrides any declared top-level security. To remove a top-level security declaration, an empty array can be used.
        /// </summary>
        [DataMember(Name = "security", EmitDefaultValue = false)]
        public IEnumerable<SecurityRequirementObject> Security { get; set; }

        /// <summary>
        /// The method to use to access this operation.
        /// </summary>
        public string GetMethod() {
            if (Parent is PathItemObject pathItem)
            {
                if (pathItem.Delete == this)
                {
                    return "DELETE";
                }
                else if (pathItem.Get == this)
                {
                    return "GET";
                }
                else if (pathItem.Head == this)
                {
                    return "HEAD";
                }
                else if (pathItem.Patch == this)
                {
                    return "PATCH";
                }
                else if (pathItem.Post == this)
                {
                    return "POST";
                }
                else if (pathItem.Put == this)
                {
                    return "PUT";
                }
                else
                {
                    throw new System.Exception("Cannot find operation object.");
                }
            }
            throw new System.Exception("Cannot determine method.");
        }

        /// <summary>
        /// The path to this operation.
        /// </summary>
        public string GetPath()
        {
            if (Parent is PathItemObject pathItem)
            {
                return pathItem.Path;
            }
            throw new System.Exception("Cannot determine method.");
        }
        public IEnumerable<string> GetProduces()
        {
            if(Produces == null)
            {
                return GetParent<SwaggerObject>().Produces ?? new string[0];
            }
            return Produces;
        }
    }
}