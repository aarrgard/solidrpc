using System;
namespace SolidRpc.Test.Petstore.Security {
    /// <summary>
    /// 
    /// </summary>
    public class ApiKeyAttribute : System.Attribute {
        /// <summary>
        /// 
        /// </summary>
        public String[] Scopes { get; set; }
    
    }
}