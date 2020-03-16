using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Threading;
using Models = SolidRpc.Test.Vitec.Types.Bid.Models;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmBid {
        /// <summary>
        /// Annulera ett bud
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Budid</param>
        /// <param name="cancellationToken"></param>
        Task<bool> CrmBidDelete(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta information om ett bud
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Budid</param>
        /// <param name="cancellationToken"></param>
        Task<Models.Bid> CrmBidGet(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta information om synliga bud kopplat till en budgivare och/eller bostad.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="criteriaEstateId">Bostadsid</param>
        /// <param name="criteriaContactId">Kontaktid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Bid>> CrmBidGetList(
            string customerId,
            string criteriaEstateId = null,
            string criteriaContactId = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta information om dolda bud kopplat till en budgivare och/eller bostad.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="criteriaEstateId">Bostadsid</param>
        /// <param name="criteriaContactId">Kontaktid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Bid>> CrmBidGetHiddenBids(
            string customerId,
            string criteriaEstateId = null,
            string criteriaContactId = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att l&#228;gga bud p&#229; en bostad.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="contactId">KontaktId</param>
        /// <param name="bidData">Buddata</param>
        /// <param name="cancellationToken"></param>
        Task<string> CrmBidAdd(
            string customerId,
            string estateId,
            string contactId,
            Models.BidData bidData,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}