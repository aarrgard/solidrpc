using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Threading;
using Models = SolidRpc.Test.Vitec.Types.SearchProfile.Models;
using System.Collections.Generic;
using SolidRpc.Test.Vitec.Types.Searchprofile.Models;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ISearchProfile {
        /// <summary>
        /// Radera s&#246;kprofil
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Sökprofilen</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.SearchProfile.SearchProfileDeleteSearchProfile.NoContentException">No Content</exception>
        Task SearchProfileDeleteSearchProfile(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar en lista av s&#246;kprofiler
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="criteriaContactId">Kontaktid.</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.SearchProfile>> SearchProfileGetSearchProfiles(
            string customerId,
            string criteriaContactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Ska kontaktens s&#246;kprofiler matchas
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="matchSearchprofile"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.SearchProfile.SearchProfileMatchSearchProfiles.NoContentException">No Content</exception>
        Task SearchProfileMatchSearchProfiles(
            string customerId,
            MatchSearchprofile matchSearchprofile,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mta en s&#246;kprofil
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Sökprofilid</param>
        /// <param name="cancellationToken"></param>
        Task<Models.SearchProfile> SearchProfileGetSearchProfile(
            string customerId,
            string id,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdatera en s&#246;kprofil f&#246;r en kontakt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="id">Sökprofilid</param>
        /// <param name="searchProfile">Sökprofil</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.SearchProfile.SearchProfileUpdateSearchProfile.NoContentException">No Content</exception>
        Task SearchProfileUpdateSearchProfile(
            string customerId,
            string id,
            Models.SearchProfileUpdate searchProfile,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Raderar alla s&#246;kprofiler p&#229; en kontakt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="contactId">Kontaktid</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Test.Vitec.Types.Services.SearchProfile.SearchProfileDeleteAllSearchProfiles.NoContentException">No Content</exception>
        Task SearchProfileDeleteAllSearchProfiles(
            string customerId,
            string contactId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skapa s&#246;kprofil
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="contactId">Kontaktid</param>
        /// <param name="searchProfile">Sökprofilen</param>
        /// <param name="cancellationToken"></param>
        Task<string> SearchProfileCreateSearchProfile(
            string customerId,
            string contactId,
            Models.SearchProfileCreate searchProfile,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Skapa s&#246;kprofil utland
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="contactId">Kontaktid</param>
        /// <param name="searchProfile">Sökprofilen</param>
        /// <param name="cancellationToken"></param>
        Task<string> SearchProfileCreateForeignSearchProfile(
            string customerId,
            string contactId,
            Models.ForeignSearchProfileCreate searchProfile,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}