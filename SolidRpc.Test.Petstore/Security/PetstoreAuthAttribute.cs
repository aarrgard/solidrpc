using System;
namespace SolidRpc.Test.Petstore.Security {
    /// <summary>
    /// 
    /// </summary>
    public class PetstoreAuthAttribute : System.Attribute {
        /// <summary>
        /// 
        /// </summary>
        public String[] Scopes { get; set; }
    
    }
}