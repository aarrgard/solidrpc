using System.CodeDom.Compiler;
using System.Threading.Tasks;
using Models = SolidRpc.Test.Vitec.Types.Association.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IAssociation {
        /// <summary>
        /// H&#228;mtar bostadsr&#228;ttsf&#246;rening.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="associationId"></param>
        /// <param name="cancellationToken"></param>
        Task<Models.Association> AssociationGetAssociation(
            string customerId,
            string associationId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}