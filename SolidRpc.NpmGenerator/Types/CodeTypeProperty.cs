using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.NpmGenerator.Types {
    /// <summary>
    /// 
    /// </summary>
    public class CodeTypeProperty {
        /// <summary>
        /// A description of the the property
        /// </summary>
        [DataMember(Name="description")]
        public string Description { get; set; }
    
        /// <summary>
        /// The name of the property
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    
        /// <summary>
        /// The property type(fully qualified)
        /// </summary>
        [DataMember(Name="propertyType")]
        public IEnumerable<string> PropertyType { get; set; }
    
        /// <summary>
        /// The name of the property in the http protocol.
        /// </summary>
        [DataMember(Name="httpName")]
        public string HttpName { get; set; }
    
    }
}