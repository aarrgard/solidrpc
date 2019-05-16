using System.Runtime.Serialization;

namespace SolidRpc.Swagger.V2
{
    /// <summary>
    /// A metadata object that allows for more fine-tuned XML model definitions.
    /// When using arrays, XML element names are not inferred(for singular/plural forms) and the name property should be used to add that information.See examples for expected behavior.
    /// </summary>
    /// <see cref="https://swagger.io/specification/v2/#xmlObject"/>
    public class XmlObject
    {
        /// <summary>
        /// Replaces the name of the element/attribute used for the described schema property. When defined within the Items Object (items), it will affect the name of the individual XML elements within the list. When defined alongside type being array (outside the items), it will affect the wrapping element and only if wrapped is true. If wrapped is false, it will be ignored.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// The URL of the namespace definition. Value SHOULD be in the form of a URL.
        /// </summary>
        [DataMember(Name = "namespace", EmitDefaultValue = false)]
        public string Namespace { get; set; }

        /// <summary>
        /// The prefix to be used for the name.
        /// </summary>
        [DataMember(Name = "prefix", EmitDefaultValue = false)]
        public string Prefix { get; set; }

        /// <summary>
        /// Declares whether the property definition translates to an attribute instead of an element. Default value is false.
        /// </summary>
        [DataMember(Name = "attribute", EmitDefaultValue = false)]
        public bool Attribute { get; set; }

        /// <summary>
        /// MAY be used only for an array definition. Signifies whether the array is wrapped (for example, <books><book/><book/></books>) or unwrapped (<book/><book/>). Default value is false. The definition takes effect only when defined alongside type being array (outside the items).
        /// </summary>
        [DataMember(Name = "wrapped", EmitDefaultValue = false)]
        public bool Wrapped { get; set; }
    }
}