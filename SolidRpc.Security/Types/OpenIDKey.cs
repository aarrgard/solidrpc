using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
namespace SolidRpc.Security.Types {
    /// <summary>
    /// 
    /// </summary>
    public class OpenIDKey {
        /// <summary>
        /// (Algorithm) Parameter
        /// </summary>
        [DataMember(Name="alg")]
        public string Alg { get; set; }
    
        /// <summary>
        /// (Key Type) Parameter
        /// </summary>
        [DataMember(Name="kty")]
        public string Kty { get; set; }
    
        /// <summary>
        /// (Public Key Use) Parameter
        /// </summary>
        [DataMember(Name="use")]
        public string Use { get; set; }
    
        /// <summary>
        /// (Key ID) Parameter
        /// </summary>
        [DataMember(Name="kid")]
        public string Kid { get; set; }
    
        /// <summary>
        /// (X.509 URL) Parameter
        /// </summary>
        [DataMember(Name="x5u")]
        public Uri X5u { get; set; }
    
        /// <summary>
        /// (X.509 Certificate SHA-1 Thumbprint) Parameter
        /// </summary>
        [DataMember(Name="x5t")]
        public string X5t { get; set; }
    
        /// <summary>
        /// (X.509 Certificate Chain) Parameter
        /// </summary>
        [DataMember(Name="x5c")]
        public IEnumerable<string> X5c { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="n")]
        public string N { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="e")]
        public string E { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="issuer")]
        public string Issuer { get; set; }
    
    }
}