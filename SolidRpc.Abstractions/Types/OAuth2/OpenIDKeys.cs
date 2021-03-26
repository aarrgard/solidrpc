using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.Abstractions.Types.OAuth2
{
    /// <summary>
    /// Represents a set of keys
    /// </summary>
    public class OpenIDKeys {
        /// <summary>
        /// The keys
        /// </summary>
        [DataMember(Name = "keys", EmitDefaultValue = false)]
        public IEnumerable<OpenIDKey> Keys { get; set; }
    
    }
}