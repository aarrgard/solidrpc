using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligence.Models;
using System;
using System.Threading;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models;
using Models = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.BusinessIntelligense.Models;
using BusinessIntelligense = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estates.BusinessIntelligense;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Budget.Models;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Hits.BusinessIntelligense;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IBusinessIntelligence {
        /// <summary>
        /// H&#228;mtar beslutst&#246;dsstatistik f&#246;r kontor.
        /// Man kan h&#228;mta statistik f&#246;r ett enskilt kontor eller en grupp.
        /// Genom att ange ett datumintervall h&#228;mtar man enbart statistik fr&#229;n det givna intervallet. H&#228;mtar information f&#246;r ett eller flera kontor. G&#229;r &#228;ven att selektera p&#229; datumintervall.
        /// </summary>
        /// <param name="licenseId">Kund-id (kontor eller grupp)</param>
        /// <param name="dateFrom">Datum från</param>
        /// <param name="dateTo">Datum till</param>
        /// <param name="assignment">Uppdragstyp</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<OfficeStatisticsSummary>> BusinessIntelligenceOfficeStatistics(
            string licenseId,
            System.DateTimeOffset? dateFrom = null,
            System.DateTimeOffset? dateTo = null,
            string assignment = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar beslutst&#246;dsstatistik f&#246;r m&#228;klare.
        /// Man kan h&#228;mta statistik f&#246;r m&#228;klare p&#229; ett enskilt kontor eller en grupp.
        /// Genom att ange ett datumintervall h&#228;mtar man enbart statistik fr&#229;n det givna intervallet. H&#228;mtar information f&#246;r en m&#228;klare kopplad till ett eller flera kontor. G&#229;r &#228;ven att selektera p&#229; datumintervall.
        /// </summary>
        /// <param name="licenseId">Kund-id (kontor eller grupp)</param>
        /// <param name="dateFrom">Datum från</param>
        /// <param name="dateTo">Datum till</param>
        /// <param name="assignment">Uppdragstyp</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<RealtorStatisticsSummary>> BusinessIntelligenceRealtorStatistics(
            string licenseId,
            System.DateTimeOffset? dateFrom = null,
            System.DateTimeOffset? dateTo = null,
            string assignment = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar beslutst&#246;dsstatistik f&#246;r kontor och sorterar informationen per &#229;r och m&#229;nad.
        /// Man kan h&#228;mta statistik f&#246;r ett enskilt kontor eller en grupp.
        /// Genom att ange ett datumintervall h&#228;mtar man enbart statistik fr&#229;n det givna intervallet. H&#228;mtar information, sorterad p&#229; &#229;r och m&#229;nad, f&#246;r ett eller flera kontor. G&#229;r &#228;ven att selektera p&#229; datumintervall.
        /// </summary>
        /// <param name="licenseId">Kund-id (kontor eller grupp)</param>
        /// <param name="dateFrom">Datum från</param>
        /// <param name="dateTo">Datum till</param>
        /// <param name="assignment">Uppdragstyp</param>
        /// <param name="cancellationToken"></param>
        Task<OfficeStatisticsByMonth> BusinessIntelligenceOfficeStatisticsByMonth(
            string licenseId,
            System.DateTimeOffset? dateFrom = null,
            System.DateTimeOffset? dateTo = null,
            string assignment = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar f&#246;rs&#228;ljningsrapport med provisioner f&#246;r en bostad med angivet bostadsid eller bostadsnummer.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="estateId">Bostadsid.</param>
        /// <param name="estateNumber">Bostadsnummer.</param>
        /// <param name="cancellationToken"></param>
        Task<SalesReportForEstateResponse> BusinessIntelligenceGetSalesReportForEstate(
            string customerId,
            string estateId = null,
            string estateNumber = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar f&#246;rs&#228;ljningsrapport med provisioner f&#246;r kundens bost&#228;der med kontraktsdag inom ett intervall. H&#228;mtar f&#246;rs&#228;ljningsrapport f&#246;r kontorets bost&#228;der med kontraktsdag som valbart intervall.
        /// </summary>
        /// <param name="customerId">Kund-id (kontor eller grupp)</param>
        /// <param name="selectionType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="cancellationToken"></param>
        Task<SalesReportResponse> BusinessIntelligenceGetSalesReport(
            string customerId,
            string selectionType,
            System.DateTimeOffset? dateFrom = null,
            System.DateTimeOffset? dateTo = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar f&#246;rs&#228;ljningsrapport med provisioner f&#246;r kundens bost&#228;der med kontraktsdag inom ett intervall. H&#228;mtar f&#246;rs&#228;ljningsrapport f&#246;r kontorets bost&#228;der med kontraktsdag som valbart intervall.
        /// </summary>
        /// <param name="customerId">Kund-id (kontor eller grupp)</param>
        /// <param name="selectionType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="cancellationToken"></param>
        Task<BusinessIntelligense.SalesReportResponse> BusinessIntelligenceGetEstateReportProdDb(
            string customerId,
            string selectionType,
            System.DateTimeOffset? dateFrom = null,
            System.DateTimeOffset? dateTo = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar en rapport med provisioner f&#246;r kundens bost&#228;der med kontraktsdag inom ett intervall. H&#228;mtar en rapport f&#246;r kontorets bost&#228;der med kontraktsdag som valbart intervall.
        /// </summary>
        /// <param name="customerId">Kund-id (kontor eller grupp)</param>
        /// <param name="selectionType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="cancellationToken"></param>
        Task<SalesReportResponse> BusinessIntelligenceGetEstatesReport(
            string customerId,
            string selectionType,
            System.DateTimeOffset? dateFrom = null,
            System.DateTimeOffset? dateTo = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar f&#246;rs&#228;ljningsrapport p&#229; antal s&#229;lda bost&#228;der med kontraktsdatum inom ett intervall
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<EstateReferences>> BusinessIntelligenceEstates(
            string customerId,
            System.DateTimeOffset? dateFrom = null,
            System.DateTimeOffset? dateTo = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdaterar budgetuppgifter
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="officeId">Unik identifierare på butiken som budgeten avser</param>
        /// <param name="userId">Användar-id på användaren som budgeten avser</param>
        /// <param name="year">År</param>
        /// <param name="month">Månad</param>
        /// <param name="realtorBudget">Budget</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.BusinessIntelligence.BusinessIntelligenceBudget.NoContentException">No Content</exception>
        Task BusinessIntelligenceBudget(
            string customerId,
            string officeId,
            string userId,
            int year,
            int month,
            RealtorBudget realtorBudget,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar en rapport &#246;ver skickade och mottagna tips och leads, datumintervallet f&#229;r inte &#246;verstiga tv&#229; m&#229;nader
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="dateFrom">Från</param>
        /// <param name="dateTo">Till</param>
        /// <param name="cancellationToken"></param>
        Task<TipsAndLeadsReport> BusinessIntelligenceGet(
            string customerId,
            DateTimeOffset dateFrom,
            DateTimeOffset dateTo,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att rapportera bes&#246;ksstatistik f&#246;r marknadsplatser
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="estateHit">Besök</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.BusinessIntelligence.BusinessIntelligenceHits.NoContentException">No Content</exception>
        Task BusinessIntelligenceHits(
            string customerId,
            IEnumerable<EstateHit> estateHit,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att rapportera daglig bes&#246;ksstatistik f&#246;r marknadsplatser
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="dailyestateHits"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.BusinessIntelligence.BusinessIntelligenceDailyHits.NoContentException">No Content</exception>
        Task BusinessIntelligenceDailyHits(
            string customerId,
            IEnumerable<DailyEstateHit> dailyestateHits,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}