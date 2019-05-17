using System.Runtime.Serialization;

namespace SolidRpc.Swagger.Model.V2
{
    /// <summary>
    /// License information for the exposed API.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#licenseObject"/>
    public class LicenseObject : ModelBase
    {
        /// <summary>
        /// Required. The license name used for the API.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// 	A URL to the license used for the API. MUST be in the format of a URL.
        /// </summary>
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url { get; set; }
    }
}