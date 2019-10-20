using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.CRM.Usergroup;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmUserGroups {
        /// <summary>
        /// H�mta anv�ndargrupp
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<UserGroup>> CrmUserGroupsGet(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}