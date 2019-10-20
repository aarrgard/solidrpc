using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingEstate {
        /// <summary>
        /// H�mta en lista �ver bost�der som kunden vill ha p� marknadsplatsen. Anv�nd f�r att synka en kund.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="criteriaIncludeHouses">Inkludera villor</param>
        /// <param name="criteriaIncludeHousingCooperatives">Inkludera l�genheter (bostadsr�tter)</param>
        /// <param name="criteriaIncludeCottages">Inkludera fritidshus</param>
        /// <param name="criteriaIncludePlots">Inkludera tomter</param>
        /// <param name="criteriaIncludeFarms">Inkludera lantbruk</param>
        /// <param name="criteriaIncludeCommercialProperties">Inkludera kommersiella objekt</param>
        /// <param name="criteriaIncludeCondominiums">Inkludera l�genheter (�gander�tter)</param>
        /// <param name="criteriaIncludeForeignProperties">Inkludera utlandsobjekt</param>
        /// <param name="criteriaIncludePremises">Inkludera lokaler</param>
        /// <param name="criteriaIncludeProjects">Inkludera projekt</param>
        /// <param name="criteriaIncludeForSale">Inkludera bost�der med status till salu</param>
        /// <param name="criteriaIncludeFutureSale">Inkludera bost�der med status kommande</param>
        /// <param name="criteriaIncludeSoonForSale">Inkludera bost�der med status snart till salu</param>
        /// <param name="criteriaPrimaryAgentId">Urval p� huvudhandl�ggare</param>
        /// <param name="criteriaEstateId">Urval p� bostadsid</param>
        /// <param name="cancellationToken"></param>
        Task<PublicAdvertisingEstateList> PublicAdvertisingEstateGetList(
            string customerId,
            bool criteriaIncludeHouses = default(bool),
            bool criteriaIncludeHousingCooperatives = default(bool),
            bool criteriaIncludeCottages = default(bool),
            bool criteriaIncludePlots = default(bool),
            bool criteriaIncludeFarms = default(bool),
            bool criteriaIncludeCommercialProperties = default(bool),
            bool criteriaIncludeCondominiums = default(bool),
            bool criteriaIncludeForeignProperties = default(bool),
            bool criteriaIncludePremises = default(bool),
            bool criteriaIncludeProjects = default(bool),
            bool criteriaIncludeForSale = default(bool),
            bool criteriaIncludeFutureSale = default(bool),
            bool criteriaIncludeSoonForSale = default(bool),
            string criteriaPrimaryAgentId = default(string),
            string criteriaEstateId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdatera marknadsf�ringsstatus f�r bostaden p� aktuell marknadsplats.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Bostadens id</param>
        /// <param name="cancellationToken"></param>
        Task<PublicAdvertisingEstateStatus> PublicAdvertisingEstateGetAdvertisementStatus(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdatera marknadsf�ringsstatus f�r bostaden p� aktuell marknadsplats.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Bostadens id</param>
        /// <param name="status">Statusinformation</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.PublicAdvertisingEstate.PublicAdvertisingEstateUpdateAdvertisementStatus.NoContentException">No Content</exception>
        Task PublicAdvertisingEstateUpdateAdvertisementStatus(
            string customerId,
            string id,
            PublicAdvertisingEstateStatus status,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar information om ett otypat objekt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Lokalsid</param>
        /// <param name="cancellationToken"></param>
        Task<EstateInformation> PublicAdvertisingEstateGetEstateLookup(
            string customerId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}