using System.Runtime.Serialization;
using System.Collections.Generic;
namespace SolidRpc.Abstractions.Types.Code {
    /// <summary>
    /// Represents a method argument
    /// </summary>
    public class CodeMethodArg {
        /// <summary>
        /// A description of the the argument
        /// </summary>
        [DataMember(Name="description")]
        public string Description { get; set; }
    
        /// <summary>
        /// The name of the argument
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    
        /// <summary>
        /// The argument type(fully qualified)
        /// </summary>
        [DataMember(Name="argType")]
        public IEnumerable<string> ArgType { get; set; }
    
        /// <summary>
        /// Specifies if this argument is optional(not required)
        /// </summary>
        [DataMember(Name="optional")]
        public bool Optional { get; set; }
    
        /// <summary>
        /// The name of the argument in the http protocol.
        /// </summary>
        [DataMember(Name="httpName")]
        public string HttpName { get; set; }
    
        /// <summary>
        /// The location of the argument(&#39;path&#39;, &#39;query&#39;, &#39;header&#39;, &#39;body&#39;, &#39;body-inline&#39;)
        /// </summary>
        [DataMember(Name="httpLocation")]
        public string HttpLocation { get; set; }
    
    }
}