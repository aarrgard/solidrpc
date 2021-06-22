using System;
namespace SolidRpc.Tests.Swagger.CodeGen.SecurityPermissionAttribute.Security {
    /// <summary>
    /// 
    /// </summary>
    public class PermissionsAttribute : System.Attribute {
        /// <summary>
        /// 
        /// </summary>
        public String[] Scopes { get; set; }
    
    }
}