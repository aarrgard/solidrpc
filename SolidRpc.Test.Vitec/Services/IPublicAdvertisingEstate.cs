using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.PublicAdvertisement.Models;
using System.Threading;
using SolidRpc.Test.Vitec.Types.Estate.Models;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingEstate {
        /// <summary>
        /// H&#228;mta en lista &#246;ver bost&#228;der som kunden vill ha p&#229; marknadsplatsen. Anv&#228;nd f&#246;r att synka en kund.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="criteriaIncludeHouses">Inkludera villor</param>
        /// <param name="criteriaIncludeHousingCooperatives">Inkludera lägenheter (bostadsrätter)</param>
        /// <param name="criteriaIncludeCottages">Inkludera fritidshus</param>
        /// <param name="criteriaIncludePlots">Inkludera tomter</param>
        /// <param name="criteriaIncludeFarms">Inkludera lantbruk</param>
        /// <param name="criteriaIncludeCommercialProperties">Inkludera kommersiella objekt</param>
        /// <param name="criteriaIncludeCondominiums">Inkludera lägenheter (äganderätter)</param>
        /// <param name="criteriaIncludeForeignProperties">Inkludera utlandsobjekt</param>
        /// <param name="criteriaIncludePremises">Inkludera lokaler</param>
        /// <param name="criteriaIncludeProjects">Inkludera projekt</param>
        /// <param name="criteriaIncludeForSale">Inkludera bostäder med status till salu</param>
        /// <param name="criteriaIncludeFutureSale">Inkludera bostäder med status kommande</param>
        /// <param name="criteriaIncludeSoonForSale">Inkludera bostäder med status snart till salu</param>
        /// <param name="criteriaPrimaryAgentId">Urval på huvudhandläggare</param>
        /// <param name="criteriaEstateId">Urval på bostadsid</param>
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
        /// Uppdatera marknadsf&#246;ringsstatus f&#246;r bostaden p&#229; aktuell marknadsplats.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Bostadens id</param>
        /// <param name="cancellationToken"></param>
        Task<PublicAdvertisingEstateStatus> PublicAdvertisingEstateGetAdvertisementStatus(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdatera marknadsf&#246;ringsstatus f&#246;r bostaden p&#229; aktuell marknadsplats.
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="id">Bostadens id</param>
        /// <param name="status">Statusinformation</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.PublicAdvertisingEstate.PublicAdvertisingEstateUpdateAdvertisementStatus.NoContentException">No Content</exception>
        Task PublicAdvertisingEstateUpdateAdvertisementStatus(
            string customerId,
            string id,
            PublicAdvertisingEstateStatus status,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar information om ett otypat objekt
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