using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models = SolidRpc.Test.Vitec.Types.Viewing.Models;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IViewing {
        /// <summary>
        /// H�mta visningar f�r en bostad.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Viewing>> ViewingGet(
            string estateId,
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta visningar f�r en bostad.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Viewing>> ViewingGet2(
            string estateId,
            string customerId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Tar bort en visningsdeltagare p� en visning.
        /// </summary>
        /// <param name="viewingId">Visningsid</param>
        /// <param name="timeSlotId">Id p� visningstillf�llet</param>
        /// <param name="contactId">Id p� kontakten.</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Viewing.ViewingRemoveViewingParticipant.NoContentException">No Content</exception>
        Task ViewingRemoveViewingParticipant(
            string viewingId,
            string timeSlotId,
            string contactId,
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// L�gg till befintlig visningsdeltagare p� en bostad.
        /// </summary>
        /// <param name="viewingId">Visningsid</param>
        /// <param name="timeSlotId">Id p� visningstillf�llet</param>
        /// <param name="contactId">Id p� kontakten.</param>
        /// <param name="participant">Information om deltagaren.</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Viewing.ViewingAddViewingParticipant2.NoContentException">No Content</exception>
        Task ViewingAddViewingParticipant2(
            string viewingId,
            string timeSlotId,
            string contactId,
            Models.AddViewingParticipant participant,
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Tar bort en visningsdeltagare p� en visning.
        /// </summary>
        /// <param name="viewingId">Visningsid</param>
        /// <param name="timeSlotId">Id p� visningstillf�llet</param>
        /// <param name="contactId">Id p� kontakten.</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Viewing.ViewingRemoveViewingParticipant2.NoContentException">No Content</exception>
        Task ViewingRemoveViewingParticipant2(
            string viewingId,
            string timeSlotId,
            string contactId,
            string customerId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// L�gg till befintlig visningsdeltagare p� en bostad.
        /// </summary>
        /// <param name="viewingId">Visningsid</param>
        /// <param name="timeSlotId">Id p� visningstillf�llet</param>
        /// <param name="contactId">Id p� kontakten.</param>
        /// <param name="participant">Information om deltagaren.</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.Viewing.ViewingAddViewingParticipant.NoContentException">No Content</exception>
        Task ViewingAddViewingParticipant(
            string viewingId,
            string timeSlotId,
            string contactId,
            Models.AddViewingParticipant participant,
            string customerId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// L�gg till ny visningsdeltagare p� en befintligt bostad
        /// </summary>
        /// <param name="viewingId">Visningsid</param>
        /// <param name="timeSlotId">Id p� visningstillf�llet</param>
        /// <param name="participant">Visningsdeltagaren.</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<string> ViewingAddNewViewingParticipant2(
            string viewingId,
            string timeSlotId,
            Models.AddNewViewingParticipant participant,
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// L�gg till ny visningsdeltagare p� en befintligt bostad
        /// </summary>
        /// <param name="viewingId">Visningsid</param>
        /// <param name="timeSlotId">Id p� visningstillf�llet</param>
        /// <param name="participant">Visningsdeltagaren.</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<string> ViewingAddNewViewingParticipant(
            string viewingId,
            string timeSlotId,
            Models.AddNewViewingParticipant participant,
            string customerId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}