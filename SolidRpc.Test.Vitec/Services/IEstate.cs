using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Common.Estate;
using System.Threading;
using SolidRpc.Test.Vitec.Types.List.Estate;
using SolidRpc.Test.Vitec.Types.Criteria.Estate;
using SolidRpc.Test.Vitec.Types.Estate.Models;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IEstate {
        /// <summary>
        /// H�mtar en lista �ver statusar f�r bost�der.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Status>> EstateGetStatuses(
            string customerId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar en lista �ver bost�der
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<EstateList>> EstateGetEstateList(
            EstateCriteria criteria,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar komplett information om en villa. F�r att h�mta komplett information f�r en villa s� kr�vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<House> EstateGetHouse(
            string estateId,
            string customerId = default(string),
            bool onlyFutureViewings = default(bool),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar komplett information om en utlandsbostad. F�r att h�mta komplett information f�r en utlandsbostad s� kr�vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<ForeignProperty> EstateGetForeignProperty(
            string estateId,
            string customerId = default(string),
            bool onlyFutureViewings = default(bool),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar komplett information om en bostadsr�tt. F�r att h�mta komplett information f�r en bostadsr�tt s� kr�vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<HousingCooperative> EstateGetHousingCooperative(
            string estateId,
            string customerId = default(string),
            bool onlyFutureViewings = default(bool),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar komplett information om ett fritidshus. F�r att h�mta komplett information f�r ett fritidshus s� kr�vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Cottage> EstateGetCottage(
            string estateId,
            string customerId = default(string),
            bool onlyFutureViewings = default(bool),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar komplett information om en tomt. F�r att h�mta komplett information f�r en tomt s� kr�vs ett tomtid.
        /// </summary>
        /// <param name="estateId">tomtid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Plot> EstateGetPlot(
            string estateId,
            string customerId = default(string),
            bool onlyFutureViewings = default(bool),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar komplett information om ett projekt. F�r att h�mta komplett information om ett projekt s� kr�vs ett projektid.
        /// </summary>
        /// <param name="projectId">Projektid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Project> EstateGetProject(
            string projectId,
            string customerId = default(string),
            bool onlyFutureViewings = default(bool),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar komplett information om en g�rd. F�r att h�mta komplett information f�r en g�rd s� kr�vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Farm> EstateGetFarm(
            string estateId,
            string customerId = default(string),
            bool onlyFutureViewings = default(bool),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar komplett information om ett kommersiellt objekt. F�r att h�mta komplett information f�r ett kommersiellt objekt s� kr�vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<CommercialProperty> EstateGetCommercialProperty(
            string estateId,
            string customerId = default(string),
            bool onlyFutureViewings = default(bool),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar komplett information om en �garl�genhet. F�r att h�mta komplett information f�r en �garl�genhet s� kr�vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Condominium> EstateGetCondominium(
            string estateId,
            string customerId = default(string),
            bool onlyFutureViewings = default(bool),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar komplett information om en lokal. F�r att h�mta komplett information f�r en lokal s� kr�vs ett lokalid.
        /// </summary>
        /// <param name="estateId">Lokalsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Premises> EstateGetPremises(
            string estateId,
            string customerId = default(string),
            bool onlyFutureViewings = default(bool),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}