using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Threading;
using Models = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.SearchProfile.Models;
using System.Collections.Generic;
using SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Searchprofile.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ISearchProfile {
        /// <summary>
        /// Radera s�kprofil
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">S�kprofilen</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.SearchProfile.SearchProfileDeleteSearchProfile.NoContentException">No Content</exception>
        Task SearchProfileDeleteSearchProfile(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar en lista av s�kprofiler
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="criteriaContactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.SearchProfile>> SearchProfileGetSearchProfiles(
            string customerId,
            string criteriaContactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Ska kontaktens s�kprofiler matchas
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="matchSearchprofile"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.SearchProfile.SearchProfileMatchSearchProfiles.NoContentException">No Content</exception>
        Task SearchProfileMatchSearchProfiles(
            string customerId,
            MatchSearchprofile matchSearchprofile,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mta en s�kprofil
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">S�kprofilid</param>
        /// <param name="cancellationToken"></param>
        Task<Models.SearchProfile> SearchProfileGetSearchProfile(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdatera en s�kprofil f�r en kontakt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">S�kprofilid</param>
        /// <param name="searchProfile">S�kprofil</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.SearchProfile.SearchProfileUpdateSearchProfile.NoContentException">No Content</exception>
        Task SearchProfileUpdateSearchProfile(
            string customerId,
            string id,
            Models.SearchProfileUpdate searchProfile,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Raderar alla s�kprofiler p� en kontakt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="contactId">Kontaktid</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.SearchProfile.SearchProfileDeleteAllSearchProfiles.NoContentException">No Content</exception>
        Task SearchProfileDeleteAllSearchProfiles(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skapa s�kprofil
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="contactId">Kontaktid</param>
        /// <param name="searchProfile">S�kprofilen</param>
        /// <param name="cancellationToken"></param>
        Task<string> SearchProfileCreateSearchProfile(
            string customerId,
            string contactId,
            Models.SearchProfileCreate searchProfile,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skapa s�kprofil utland
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="contactId">Kontaktid</param>
        /// <param name="searchProfile">S�kprofilen</param>
        /// <param name="cancellationToken"></param>
        Task<string> SearchProfileCreateForeignSearchProfile(
            string customerId,
            string contactId,
            Models.ForeignSearchProfileCreate searchProfile,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}