using System.Collections.Generic;
namespace SolidRpc.Abstractions.Types.OAuth2
{
    /// <summary>
    /// Represents a set of keys
    /// </summary>
    public class OpenIDKeys {
        /// <summary>
        /// The keys
        /// </summary>
        public IEnumerable<OpenIDKey> Keys { get; set; }
    
    }
}