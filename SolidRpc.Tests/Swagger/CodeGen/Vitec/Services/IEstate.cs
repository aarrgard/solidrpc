using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Common.Estate;
using System.Threading;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.List.Estate;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Criteria.Estate;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Estate.Models;
using System;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IEstate {
        /// <summary>
        /// H&#228;mtar en lista &#246;ver statusar f&#246;r bost&#228;der.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Status>> EstateGetStatuses(
            string customerId = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar en lista &#246;ver bost&#228;der
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<EstateList>> EstateGetEstateList(
            EstateCriteria criteria,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar komplett information om en villa. F&#246;r att h&#228;mta komplett information f&#246;r en villa s&#229; kr&#228;vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<House> EstateGetHouse(
            string estateId,
            string customerId = null,
            bool? onlyFutureViewings = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar komplett information om en utlandsbostad. F&#246;r att h&#228;mta komplett information f&#246;r en utlandsbostad s&#229; kr&#228;vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<ForeignProperty> EstateGetForeignProperty(
            string estateId,
            string customerId = null,
            bool? onlyFutureViewings = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar komplett information om en bostadsr&#228;tt. F&#246;r att h&#228;mta komplett information f&#246;r en bostadsr&#228;tt s&#229; kr&#228;vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<HousingCooperative> EstateGetHousingCooperative(
            string estateId,
            string customerId = null,
            bool? onlyFutureViewings = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar komplett information om ett fritidshus. F&#246;r att h&#228;mta komplett information f&#246;r ett fritidshus s&#229; kr&#228;vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Cottage> EstateGetCottage(
            string estateId,
            string customerId = null,
            bool? onlyFutureViewings = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar komplett information om en tomt. F&#246;r att h&#228;mta komplett information f&#246;r en tomt s&#229; kr&#228;vs ett tomtid.
        /// </summary>
        /// <param name="estateId">tomtid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Plot> EstateGetPlot(
            string estateId,
            string customerId = null,
            bool? onlyFutureViewings = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar komplett information om ett projekt. F&#246;r att h&#228;mta komplett information om ett projekt s&#229; kr&#228;vs ett projektid.
        /// </summary>
        /// <param name="projectId">Projektid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Project> EstateGetProject(
            string projectId,
            string customerId = null,
            bool? onlyFutureViewings = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar komplett information om en g&#229;rd. F&#246;r att h&#228;mta komplett information f&#246;r en g&#229;rd s&#229; kr&#228;vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Farm> EstateGetFarm(
            string estateId,
            string customerId = null,
            bool? onlyFutureViewings = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar komplett information om ett kommersiellt objekt. F&#246;r att h&#228;mta komplett information f&#246;r ett kommersiellt objekt s&#229; kr&#228;vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<CommercialProperty> EstateGetCommercialProperty(
            string estateId,
            string customerId = null,
            bool? onlyFutureViewings = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar komplett information om en &#228;garl&#228;genhet. F&#246;r att h&#228;mta komplett information f&#246;r en &#228;garl&#228;genhet s&#229; kr&#228;vs ett bostadsid.
        /// </summary>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Condominium> EstateGetCondominium(
            string estateId,
            string customerId = null,
            bool? onlyFutureViewings = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar komplett information om en lokal. F&#246;r att h&#228;mta komplett information f&#246;r en lokal s&#229; kr&#228;vs ett lokalid.
        /// </summary>
        /// <param name="estateId">Lokalsid</param>
        /// <param name="customerId">Kundid</param>
        /// <param name="onlyFutureViewings">Visa enbart kommande visningar</param>
        /// <param name="cancellationToken"></param>
        Task<Premises> EstateGetPremises(
            string estateId,
            string customerId = null,
            bool? onlyFutureViewings = null,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}