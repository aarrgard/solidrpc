using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Petstore.Security {
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