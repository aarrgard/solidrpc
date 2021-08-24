using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SolidRpc.Tests.Swagger.SpecGen.OAuth2.Types
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