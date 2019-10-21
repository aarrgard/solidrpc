using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.ParticipantFollowUp.Models;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IViewingParticipant {
        /// <summary>
        /// H&#228;mta uppf&#246;ljning om en visningsdeltagare f&#246;r en visning
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="contactId"></param>
        /// <param name="viewingId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.ParticipantFollowUp>> ViewingParticipantGet(
            string customerId,
            string contactId,
            string viewingId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdatera uppf&#246;ljning om en visningsdeltagare
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="participantFollowUp"></param>
        /// <param name="cancellationToken"></param>
        Task<bool> ViewingParticipantPut(
            string customerId,
            Models.ParticipantFollowUp participantFollowUp,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}