using System.CodeDom.Compiler;
using System.Threading.Tasks;
using Models = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Meeting.Models;
using System.Threading;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IMeeting {
        /// <summary>
        /// H&#228;mtar ett bokat m&#246;te
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Mötets id</param>
        /// <param name="cancellationToken"></param>
        Task<Models.Meeting> MeetingGetSingle(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar bokade intagsm&#246;ten.
        /// </summary>
        /// <param name="criteria">Urval</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Meeting>> MeetingGetMeetings(
            Models.MeetingCreateria criteria,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Boka intagsm&#246;te
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Objektid</param>
        /// <param name="assignmentMeeting">Intagsmöte</param>
        /// <param name="cancellationToken"></param>
        Task<string> MeetingPostAssignmentMeeting(
            string customerId,
            string estateId,
            Models.AssignmentMeeting assignmentMeeting,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}