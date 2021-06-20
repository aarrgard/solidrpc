using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Abstractions.Types.Code
{
    /// <summary>
    /// Represents an interface
    /// </summary>
    public class CodeInterface {
        /// <summary>
        /// The description of this interface
        /// </summary>
        [DataMember(Name="description")]
        public string Description { get; set; }
    
        /// <summary>
        /// The name of this interface
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    
        /// <summary>
        /// The methods in the interface
        /// </summary>
        [DataMember(Name="methods")]
        public IEnumerable<CodeMethod> Methods { get; set; }
    
    }
}