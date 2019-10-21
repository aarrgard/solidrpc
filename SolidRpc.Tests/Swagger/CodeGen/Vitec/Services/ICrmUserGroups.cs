using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.CRM.Usergroup;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmUserGroups {
        /// <summary>
        /// H&#228;mta anv&#228;ndargrupp
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<UserGroup>> CrmUserGroupsGet(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}