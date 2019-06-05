using System.Runtime.Serialization;

namespace SolidRpc.OpenApi.Model.V2
{
    /// <summary>
    /// Contact information for the exposed API.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#contactObject"/>
    public class ContactObject : ModelBase
    {
        public ContactObject(ModelBase parent) : base(parent) { }
        /// <summary>
        /// The identifying name of the contact person/organization.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// The URL pointing to the contact information. MUST be in the format of a URL
        /// </summary>
        [DataMember(Name = "url", EmitDefaultValue = false)]
        public string Url { get; set; }

        /// <summary>
        /// The email address of the contact person/organization. MUST be in the format of an email address.
        /// </summary>
        [DataMember(Name = "email", EmitDefaultValue = false)]
        public string Email { get; set; }
    }
}