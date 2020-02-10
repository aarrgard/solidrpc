using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// License information for the exposed API.
    /// </summary>
    /// <a href="https://swagger.io/specification/v2/#licenseObject"/>
    public class LicenseObject : ModelBase
    {
        public LicenseObject(ModelBase parent) : base(parent) { }

        /// <summary>
        /// Required. The license name used for the API.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false, IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// 	A URL to the license used for the API. MUST be in the format of a URL.
        /// </summary>
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url { get; set; }
    }
}