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
        /// H�mtar beslutst�dsstatistik f�r kontor.
        /// Man kan h�mta statistik f�r ett enskilt kontor eller en grupp.
        /// Genom att ange ett datumintervall h�mtar man enbart statistik fr�n det givna intervallet. H�mtar information f�r ett eller flera kontor. G�r �ven att selektera p� datumintervall.
        /// </summary>
        /// <param name="licenseId">Kund-id (kontor eller grupp)</param>
        /// <param name="dateFrom">Datum fr�n</param>
        /// <param name="dateTo">Datum till</param>
        /// <param name="assignment">Uppdragstyp</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<OfficeStatisticsSummary>> BusinessIntelligenceOfficeStatistics(
            string licenseId,
            DateTimeOffset dateFrom = default(DateTimeOffset),
            DateTimeOffset dateTo = default(DateTimeOffset),
            string assignment = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar beslutst�dsstatistik f�r m�klare.
        /// Man kan h�mta statistik f�r m�klare p� ett enskilt kontor eller en grupp.
        /// Genom att ange ett datumintervall h�mtar man enbart statistik fr�n det givna intervallet. H�mtar information f�r en m�klare kopplad till ett eller flera kontor. G�r �ven att selektera p� datumintervall.
        /// </summary>
        /// <param name="licenseId">Kund-id (kontor eller grupp)</param>
        /// <param name="dateFrom">Datum fr�n</param>
        /// <param name="dateTo">Datum till</param>
        /// <param name="assignment">Uppdragstyp</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<RealtorStatisticsSummary>> BusinessIntelligenceRealtorStatistics(
            string licenseId,
            DateTimeOffset dateFrom = default(DateTimeOffset),
            DateTimeOffset dateTo = default(DateTimeOffset),
            string assignment = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar beslutst�dsstatistik f�r kontor och sorterar informationen per �r och m�nad.
        /// Man kan h�mta statistik f�r ett enskilt kontor eller en grupp.
        /// Genom att ange ett datumintervall h�mtar man enbart statistik fr�n det givna intervallet. H�mtar information, sorterad p� �r och m�nad, f�r ett eller flera kontor. G�r �ven att selektera p� datumintervall.
        /// </summary>
        /// <param name="licenseId">Kund-id (kontor eller grupp)</param>
        /// <param name="dateFrom">Datum fr�n</param>
        /// <param name="dateTo">Datum till</param>
        /// <param name="assignment">Uppdragstyp</param>
        /// <param name="cancellationToken"></param>
        Task<OfficeStatisticsByMonth> BusinessIntelligenceOfficeStatisticsByMonth(
            string licenseId,
            DateTimeOffset dateFrom = default(DateTimeOffset),
            DateTimeOffset dateTo = default(DateTimeOffset),
            string assignment = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar f�rs�ljningsrapport med provisioner f�r en bostad med angivet bostadsid eller bostadsnummer.
        /// </summary>
        /// <param name="customerId">Kund-id.</param>
        /// <param name="estateId">Bostadsid.</param>
        /// <param name="estateNumber">Bostadsnummer.</param>
        /// <param name="cancellationToken"></param>
        Task<SalesReportForEstateResponse> BusinessIntelligenceGetSalesReportForEstate(
            string customerId,
            string estateId = default(string),
            string estateNumber = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar f�rs�ljningsrapport med provisioner f�r kundens bost�der med kontraktsdag inom ett intervall. H�mtar f�rs�ljningsrapport f�r kontorets bost�der med kontraktsdag som valbart intervall.
        /// </summary>
        /// <param name="customerId">Kund-id (kontor eller grupp)</param>
        /// <param name="selectionType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="cancellationToken"></param>
        Task<SalesReportResponse> BusinessIntelligenceGetSalesReport(
            string customerId,
            string selectionType,
            DateTimeOffset dateFrom = default(DateTimeOffset),
            DateTimeOffset dateTo = default(DateTimeOffset),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar f�rs�ljningsrapport med provisioner f�r kundens bost�der med kontraktsdag inom ett intervall. H�mtar f�rs�ljningsrapport f�r kontorets bost�der med kontraktsdag som valbart intervall.
        /// </summary>
        /// <param name="customerId">Kund-id (kontor eller grupp)</param>
        /// <param name="selectionType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="cancellationToken"></param>
        Task<BusinessIntelligense.SalesReportResponse> BusinessIntelligenceGetEstateReportProdDb(
            string customerId,
            string selectionType,
            DateTimeOffset dateFrom = default(DateTimeOffset),
            DateTimeOffset dateTo = default(DateTimeOffset),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar en rapport med provisioner f�r kundens bost�der med kontraktsdag inom ett intervall. H�mtar en rapport f�r kontorets bost�der med kontraktsdag som valbart intervall.
        /// </summary>
        /// <param name="customerId">Kund-id (kontor eller grupp)</param>
        /// <param name="selectionType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="cancellationToken"></param>
        Task<SalesReportResponse> BusinessIntelligenceGetEstatesReport(
            string customerId,
            string selectionType,
            DateTimeOffset dateFrom = default(DateTimeOffset),
            DateTimeOffset dateTo = default(DateTimeOffset),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar f�rs�ljningsrapport p� antal s�lda bost�der med kontraktsdatum inom ett intervall
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<EstateReferences>> BusinessIntelligenceEstates(
            string customerId,
            DateTimeOffset dateFrom = default(DateTimeOffset),
            DateTimeOffset dateTo = default(DateTimeOffset),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdaterar budgetuppgifter
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="officeId">Unik identifierare p� butiken som budgeten avser</param>
        /// <param name="userId">Anv�ndar-id p� anv�ndaren som budgeten avser</param>
        /// <param name="year">�r</param>
        /// <param name="month">M�nad</param>
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
        /// H�mtar en rapport �ver skickade och mottagna tips och leads, datumintervallet f�r inte �verstiga tv� m�nader
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="dateFrom">Fr�n</param>
        /// <param name="dateTo">Till</param>
        /// <param name="cancellationToken"></param>
        Task<TipsAndLeadsReport> BusinessIntelligenceGet(
            string customerId,
            DateTimeOffset dateFrom,
            DateTimeOffset dateTo,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att rapportera bes�ksstatistik f�r marknadsplatser
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="estateHit">Bes�k</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.BusinessIntelligence.BusinessIntelligenceHits.NoContentException">No Content</exception>
        Task BusinessIntelligenceHits(
            string customerId,
            IEnumerable<EstateHit> estateHit,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att rapportera daglig bes�ksstatistik f�r marknadsplatser
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