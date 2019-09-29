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
        [DataMember(Name="alg",EmitDefaultValue=false)]
        public string Alg { get; set; }
    
        /// <summary>
        /// (Key Type) Parameter
        /// </summary>
        [DataMember(Name="kty",EmitDefaultValue=false)]
        public string Kty { get; set; }
    
        /// <summary>
        /// (Public Key Use) Parameter
        /// </summary>
        [DataMember(Name="use",EmitDefaultValue=false)]
        public string Use { get; set; }
    
        /// <summary>
        /// (Key ID) Parameter
        /// </summary>
        [DataMember(Name="kid",EmitDefaultValue=false)]
        public string Kid { get; set; }
    
        /// <summary>
        /// (X.509 URL) Parameter
        /// </summary>
        [DataMember(Name="x5u",EmitDefaultValue=false)]
        public Uri X5u { get; set; }
    
        /// <summary>
        /// (X.509 Certificate SHA-1 Thumbprint) Parameter
        /// </summary>
        [DataMember(Name="x5t",EmitDefaultValue=false)]
        public string X5t { get; set; }
    
        /// <summary>
        /// (X.509 Certificate Chain) Parameter
        /// </summary>
        [DataMember(Name="x5c",EmitDefaultValue=false)]
        public IEnumerable<string> X5c { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="n",EmitDefaultValue=false)]
        public string N { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="e",EmitDefaultValue=false)]
        public string E { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name="issuer",EmitDefaultValue=false)]
        public string Issuer { get; set; }
    
    }
}