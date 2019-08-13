using System.Collections.Generic;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// 
    /// </summary>
    public class OpenIDKey {
        /// <summary>
        /// 
        /// </summary>
        public string Kty { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string Use { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string Kid { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string K5t { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string N { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string E { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> X5c { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }
    
    }
}