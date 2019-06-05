using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// DescribesThe object provides metadata about the API. The metadata can be used by the clients if needed, and can be presented in the Swagger-UI for convenience.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#infoObject"/>
    public class InfoObject : ModelBase
    {
        public InfoObject(ModelBase parent) : base(parent) { }
        /// <summary>
        /// Required. The title of the application.
        /// </summary>
        [DataMember(Name = "title", EmitDefaultValue = false, IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// A short description of the application. GFM syntax can be used for rich text representation.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// The Terms of Service for the API.
        /// </summary>
        [DataMember(Name = "termsOfService", EmitDefaultValue = false)]
        public string TermsOfService { get; set; }

        /// <summary>
        /// The contact information for the exposed API.
        /// </summary>
        [DataMember(Name = "contact", EmitDefaultValue = false)]
        public ContactObject Contact { get; set; }

        /// <summary>
        /// The license information for the exposed API.
        /// </summary>
        [DataMember(Name = "license", EmitDefaultValue = false)]
        public LicenseObject License { get; set; }

        /// <summary>
        /// Required Provides the version of the application API (not to be confused with the specification version).
        /// </summary>
        [DataMember(Name = "version", EmitDefaultValue = false)]
        public string Version { get; set; }
    }
}