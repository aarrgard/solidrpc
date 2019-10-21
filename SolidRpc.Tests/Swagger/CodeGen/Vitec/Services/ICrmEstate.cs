using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Bid.Models;
using System.Threading;
using Models = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Invoice.Models;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Marketplace.Models;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.File.Models;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.IntakeSource.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmEstate {
        /// <summary>
        /// H&#228;mtar budgivningsinst&#228;llningar. &lt;p&gt;H&#228;mtar budgivningsinst&#228;llningar.&lt;/p&gt;
        /// &lt;p&gt;F&#246;r att kunna h&#228;mta bidinst&#228;llningar s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">bostadsid</param>
        /// <param name="cancellationToken"></param>
        Task<BiddingSettings> CrmEstateGetBiddingSettings(
            string customerId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdatera budgivningsinst&#228;llningar. &lt;p&gt;Uppdatera budgivningsinst&#228;llningar.&lt;/p&gt;
        /// &lt;p&gt;F&#246;r att kunna h&#228;mta bidinst&#228;llningar s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="biddingSettings">Bidgivninginställingar</param>
        /// <param name="cancellationToken"></param>
        Task<bool> CrmEstatePutBiddingSettings(
            string customerId,
            string estateId,
            BiddingSettings biddingSettings,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar fakturanummer. &lt;p&gt;H&#228;mtar fakturanummer.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">bostadsid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.Invoice>> CrmEstateGetInvoices(
            string customerId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar provisionsjusteringar &lt;p&gt;H&#228;mtar provisionsjusteringar (till&#228;gg/avdrag)&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.CommissionAdjustment>> CrmEstateGetAdjustment(
            string customerId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Provisionsjustering &lt;p&gt;Provisionsjustering (till&#228;gg/avdrag)&lt;/p&gt;
        /// &lt;p&gt;Ange ett negativt belopp f&#246;r avdrag p&#229; provisionen&lt;/p&gt;
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
        /// Marknadsf&#246;r utlandsbost&#228;der p&#229; marknadsplatser
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="estateId"></param>
        /// <param name="estateAdvertising"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.CrmEstate.CrmEstateUpdateAdvertising.NoContentException">No Content</exception>
        Task CrmEstateUpdateAdvertising(
            string customerId,
            string estateId,
            CrmEstateAdvertising estateAdvertising,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar marknadsplatser f&#246;r utlandsbost&#228;der
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="criteriaEstateType"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<CrmEstateMarketplace>> CrmEstateGetMarketplaces(
            string customerId,
            string criteriaEstateType = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar alla filer tillh&#246;rande ett objekt eller projekt
        /// </summary>
        /// <param name="customerId">kundid</param>
        /// <param name="estateId">id på objekt/projekt</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<FileInfo>> CrmEstateGetFiles(
            string customerId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar intagsk&#228;llor
        /// </summary>
        /// <param name="customerId">kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<AssignmentSource>> CrmEstateGetIntakesSources(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}