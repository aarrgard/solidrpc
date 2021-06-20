using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Abstractions.Types.Code
{
    /// <summary>
    /// Represents a type
    /// </summary>
    public class CodeType {
        /// <summary>
        /// A description of the the type
        /// </summary>
        [DataMember(Name="description")]
        public string Description { get; set; }
    
        /// <summary>
        /// The name of the type
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }

        /// <summary>
        /// The type that this type extends
        /// </summary>
        [DataMember(Name = "extends")]
        public IEnumerable<string> Extends { get; set; }

        /// <summary>
        /// The method arguments
        /// </summary>
        [DataMember(Name="properties")]
        public IEnumerable<CodeTypeProperty> Properties { get; set; }
    
    }
}