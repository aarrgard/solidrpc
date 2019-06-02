using System.Runtime.Serialization;

namespace SolidRpc.Swagger.Model.V2
{
    /// <summary>
    /// Allows the definition of a security scheme that can be used by the operations. Supported schemes are basic authentication, an API key (either as a header or as a query parameter) and OAuth2's common flows (implicit, password, application and access code).
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#securitySchemeObject"/>
    public class SecuritySchemeObject : ModelBase
    {
        public SecuritySchemeObject(ModelBase parent) : base(parent)
        {

        }

        /// <summary>
        /// Required. The type of the security scheme. Valid values are "basic", "apiKey" or "oauth2".
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// A short description for security scheme.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Required. The name of the header or query parameter to be used.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Required The location of the API key. Valid values are "query" or "header".
        /// </summary>
        [DataMember(Name = "in", EmitDefaultValue = false)]
        public string In { get; set; }

        /// <summary>
        /// Required. The flow used by the OAuth2 security scheme. Valid values are "implicit", "password", "application" or "accessCode".
        /// </summary>
        [DataMember(Name = "flow", EmitDefaultValue = false)]
        public string Flow { get; set; }

        /// <summary>
        /// Required. The authorization URL to be used for this flow. This SHOULD be in the form of a URL.
        /// </summary>
        [DataMember(Name = "authorizationUrl", EmitDefaultValue = false)]
        public string AuthorizationUrl { get; set; }

        /// <summary>
        /// Required. The token URL to be used for this flow. This SHOULD be in the form of a URL.
        /// </summary>
        [DataMember(Name = "tokenUrl", EmitDefaultValue = false)]
        public string TokenUrl { get; set; }

        /// <summary>
        ///Required. The available scopes for the OAuth2 security scheme.
        /// </summary>
        [DataMember(Name = "scopes", EmitDefaultValue = false)]
        public ScopesObject Scopes { get; set; }
    }
}