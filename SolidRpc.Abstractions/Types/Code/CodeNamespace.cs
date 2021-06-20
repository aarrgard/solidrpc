using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Abstractions.Types.Code
{
    /// <summary>
    /// represents a namespace
    /// </summary>
    public class CodeNamespace {
        /// <summary>
        /// The name of this namespace part(not fully qualified).
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    
        /// <summary>
        /// The namespaces within this namespace
        /// </summary>
        [DataMember(Name="namespaces")]
        public IEnumerable<CodeNamespace> Namespaces { get; set; }
    
        /// <summary>
        /// The interfaces within this namespace
        /// </summary>
        [DataMember(Name="interfaces")]
        public IEnumerable<CodeInterface> Interfaces { get; set; }
    
        /// <summary>
        /// The types within this namespace
        /// </summary>
        [DataMember(Name="types")]
        public IEnumerable<CodeType> Types { get; set; }
    
    }
}