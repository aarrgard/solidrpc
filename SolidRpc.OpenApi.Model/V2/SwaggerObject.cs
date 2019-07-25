using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// This is the root document object for the API specification. It combines what previously was the Resource Listing and API Declaration (version 1.2 and earlier) together into one document.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#swaggerObject"/>
    public class SwaggerObject : ModelBase, IOpenApiSpec
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SwaggerObject(ModelBase parent) : base(parent)
        {
            Swagger = "2.0";
        }

        /// <summary>
        /// Required. Specifies the Swagger Specification version being used. It can be used by the Swagger UI and other clients to interpret the API listing. The value MUST be "2.0".
        /// </summary>
        [DataMember(Name = "swagger", IsRequired = true, EmitDefaultValue = false)]
        public string Swagger { get; set; }

        /// <summary>
        /// Required. Provides metadata about the API. The metadata can be used by the clients if needed.
        /// </summary>
        [DataMember(Name = "info", IsRequired = true, EmitDefaultValue = false)]
        public InfoObject Info { get; set; }

        /// <summary>
        /// The host (name or ip) serving the API. This MUST be the host only and does not include the scheme nor sub-paths. It MAY include a port. If the host is not included, the host serving the documentation is to be used (including the port). The host does not support path templating.
        /// </summary>
        [DataMember(Name = "host", EmitDefaultValue = false)]
        public string Host { get; set; }

        /// <summary>
        /// The base path on which the API is served, which is relative to the host. If it is not included, the API is served directly under the host. The value MUST start with a leading slash (/). The basePath does not support path templating.
        /// </summary>
        [DataMember(Name = "basePath", EmitDefaultValue = false)]
        public string BasePath { get; set; }

        /// <summary>
        /// The transfer protocol of the API. Values MUST be from the list: "http", "https", "ws", "wss". If the schemes is not included, the default scheme to be used is the one used to access the Swagger definition itself.
        /// </summary>
        [DataMember(Name = "schemes", EmitDefaultValue = false)]
        public string[] Schemes { get; set; }

        /// <summary>
        /// A list of MIME types the APIs can consume. This is global to all APIs but can be overridden on specific API calls. Value MUST be as described under Mime Types.
        /// </summary>
        [DataMember(Name = "consumes", EmitDefaultValue = false)]
        public string[] Consumes { get; set; }

        /// <summary>
        /// A list of MIME types the APIs can produce. This is global to all APIs but can be overridden on specific API calls. Value MUST be as described under Mime Types.
        /// </summary>
        [DataMember(Name = "produces", EmitDefaultValue = false)]
        public string[] Produces { get; set; }

        /// <summary>
        /// Required. The available paths and operations for the API.
        /// </summary>
        [DataMember(Name = "paths", IsRequired = true, EmitDefaultValue = false)]
        public PathsObject Paths { get; set; }

        /// <summary>
        /// Required. The available paths and operations for the API.
        /// </summary>
        [DataMember(Name = "definitions", EmitDefaultValue = false)]
        public DefinitionsObject Definitions { get; set; }

        /// <summary>
        /// An object to hold parameters that can be used across operations. This property does not define global parameters for all operations.
        /// </summary>
        [DataMember(Name = "parameters", EmitDefaultValue = false)]
        public ParametersDefinitionsObject Parameters { get; set; }

        /// <summary>
        /// An object to hold responses that can be used across operations. This property does not define global responses for all operations.
        /// </summary>
        [DataMember(Name = "responses", EmitDefaultValue = false)]
        public ResponsesDefinitionsObject Responses { get; set; }

        /// <summary>
        /// Security scheme definitions that can be used across the specification.
        /// </summary>
        [DataMember(Name = "securityDefinitions", EmitDefaultValue = false)]
        public SecurityDefinitionsObject SecurityDefinitions { get; set; }

        /// <summary>
        /// A declaration of which security schemes are applied for the API as a whole. The list of values describes alternative security schemes that can be used (that is, there is a logical OR between the security requirements). Individual operations can override this definition.
        /// </summary>
        [DataMember(Name = "security", EmitDefaultValue = false)]
        public IEnumerable<SecurityRequirementObject> Security { get; set; }

        /// <summary>
        /// A list of tags used by the specification with additional metadata. The order of the tags can be used to reflect on their order by the parsing tools. Not all tags that are used by the Operation Object must be declared. The tags that are not declared may be organized randomly or based on the tools' logic. Each tag name in the list MUST be unique.
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public IEnumerable<TagObject> Tags { get; set; }

        /// <summary>
        /// Additional external documentation.
        /// </summary>
        [DataMember(Name = "externalDocs", EmitDefaultValue = false)]
        public ExternalDocumentationObject ExternalDocs { get; set; }

        public string OpenApiVersion => "2.0";

        /// <summary>
        /// Returns the paths object.
        /// </summary>
        /// <returns></returns>
        public PathsObject GetPaths()
        {
            if (Paths == null)
            {
                Paths = new PathsObject(this);
            }
            return Paths;
        }

        /// <summary>
        /// Returns the path item
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public PathItemObject GetPath(string path)
        {
            var paths = GetPaths();
            if (paths[path] == null)
            {
                paths[path] = new PathItemObject(this);
            }
            return paths[path];
        }

        /// <summary>
        /// Returns the get operation.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public OperationObject GetGetOperation(string path)
        {
            var pathItem = GetPath(path);
            if(pathItem.Get == null)
            {
                pathItem.Get = new OperationObject(pathItem);
            }
            return pathItem.Get;
        }

        /// <summary>
        /// Returns the paths object.
        /// </summary>
        /// <returns></returns>
        public ResponsesDefinitionsObject GetResponses()
        {
            if (Responses == null)
            {
                Responses = new ResponsesDefinitionsObject(this);
            }
            return Responses;
        }

        /// <summary>
        /// Updates the host and port so that it reflects the supplied address
        /// </summary>
        /// <param name="rootAddress"></param>
        public void SetSchemeAndHostAndPort(Uri rootAddress)
        {
            Schemes = new[] { rootAddress.Scheme };
            Host = rootAddress.Host;
            if(!rootAddress.IsDefaultPort)
            {
                Host = $"{Host}:{rootAddress.Port}";
            }
        }

        public string WriteAsJsonString()
        {
            return OpenApiParserV2.WriteSwaggerDoc(this);
        }
    }
}
