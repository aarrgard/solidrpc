using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Bid.Models;
using System.Threading;
using Models = SolidRpc.Test.Vitec.Types.Invoice.Models;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Marketplace.Models;
using SolidRpc.Test.Vitec.Types.File.Models;
using SolidRpc.Test.Vitec.Types.IntakeSource.Models;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmEstate {
        /// <summary>
        /// H�mtar budgivningsinst�llningar. &lt;p&gt;H�mtar budgivningsinst�llningar.&lt;/p&gt;
        /// &lt;p&gt;F�r att kunna h�mta bidinst�llningar s� kr�vs det en giltig API nyckel och ett kundid.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">bostadsid</param>
        /// <param name="cancellationToken"></param>
        Task<BiddingSettings> CrmEstateGetBiddingSettings(
            string customerId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdatera budgivningsinst�llningar. &lt;p&gt;Uppdatera budgivningsinst�llningar.&lt;/p&gt;
        /// &lt;p&gt;F�r att kunna h�mta bidinst�llningar s� kr�vs det en giltig API nyckel och ett kundid.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="biddingSettings">Bidgivninginst�llingar</param>
        /// <param name="cancellationToken"></param>
        Task<bool> CrmEstatePutBiddingSettings(
            string customerId,
            string estateId,
            BiddingSettings biddingSettings,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar fakturanummer. &lt;p&gt;H�mtar fakturanummer.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">bostadsid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Invoice>> CrmEstateGetInvoices(
            string customerId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar provisionsjusteringar &lt;p&gt;H�mtar provisionsjusteringar (till�gg/avdrag)&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.CommissionAdjustment>> CrmEstateGetAdjustment(
            string customerId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Provisionsjustering &lt;p&gt;Provisionsjustering (till�gg/avdrag)&lt;/p&gt;
        /// &lt;p&gt;Ange ett negativt belopp f�r avdrag p� provisionen&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="commisionAdjustments">Justring</param>
        /// <param name="cancellationToken"></param>
        Task<string> CrmEstatePostAdjustment(
            string customerId,
            string estateId,
            Models.CommissionAdjustment commisionAdjustments,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Marknadsf�r utlandsbost�der p� marknadsplatser
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="estateId"></param>
        /// <param name="estateAdvertising"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.CrmEstate.CrmEstateUpdateAdvertising.NoContentException">No Content</exception>
        Task CrmEstateUpdateAdvertising(
            string customerId,
            string estateId,
            CrmEstateAdvertising estateAdvertising,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar marknadsplatser f�r utlandsbost�der
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="criteriaEstateType"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmEstateMarketplace>> CrmEstateGetMarketplaces(
            string customerId,
            string criteriaEstateType = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar alla filer tillh�rande ett objekt eller projekt
        /// </summary>
        /// <param name="customerId">kundid</param>
        /// <param name="estateId">id p� objekt/projekt</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<FileInfo>> CrmEstateGetFiles(
            string customerId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar intagsk�llor
        /// </summary>
        /// <param name="customerId">kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<AssignmentSource>> CrmEstateGetIntakesSources(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}