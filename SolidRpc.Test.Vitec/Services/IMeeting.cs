using System.CodeDom.Compiler;
using System.Threading.Tasks;
using Models = SolidRpc.Test.Vitec.Types.Meeting.Models;
using System.Threading;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IMeeting {
        /// <summary>
        /// H�mtar ett bokat m�te
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">M�tets id</param>
        /// <param name="cancellationToken"></param>
        Task<Models.Meeting> MeetingGetSingle(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar bokade intagsm�ten.
        /// </summary>
        /// <param name="criteria">Urval</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Meeting>> MeetingGetMeetings(
            Models.MeetingCreateria criteria,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Boka intagsm�te
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Objektid</param>
        /// <param name="assignmentMeeting">Intagsm�te</param>
        /// <param name="cancellationToken"></param>
        Task<string> MeetingPostAssignmentMeeting(
            string customerId,
            string estateId,
            Models.AssignmentMeeting assignmentMeeting,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}