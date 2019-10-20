using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Speculator.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmSpeculator {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="contactId"></param>
        /// <param name="estateId"></param>
        /// <param name="speculatorRelation"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.CrmSpeculator.CrmSpeculatorPut.NoContentException">No Content</exception>
        Task CrmSpeculatorPut(
            string customerId,
            string contactId,
            string estateId,
            SpeculatorRelation speculatorRelation,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}