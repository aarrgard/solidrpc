using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.SecurityPermissionAttribute.Security;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.SecurityPermissionAttribute.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ISecurityPermission {
        /// <summary>
        /// Tests the security attribute with permissions
        /// </summary>
        /// <param name="cancellationToken"></param>
        [Permissions]
        Task Securitytest(
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}