using System.Runtime.Serialization;
using System.Collections.Generic;
using SolidRpc.NpmGenerator.Types;
namespace SolidRpc.NpmGenerator.Types {
    /// <summary>
    /// 
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
        /// The method arguments
        /// </summary>
        [DataMember(Name="properties")]
        public IEnumerable<CodeTypeProperty> Properties { get; set; }
    
    }
}