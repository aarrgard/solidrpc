using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models = SolidRpc.Test.Vitec.Types.User.Models;
using System.Threading;
using User = SolidRpc.Test.Vitec.Types.CRM.User;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IUser {
        /// <summary>
        /// H&#228;mtar lista &#246;ver publika anv&#228;ndare. H&#228;mta anv&#228;ndarlista f&#246;r publika anv&#228;ndare.
        /// F&#246;r att kunna h&#228;mta anv&#228;ndarlista s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="criteriaUserId">Användarid</param>
        /// <param name="criteriaSearchText">Text som filtrerar på namn eller titel</param>
        /// <param name="criteriaCustomerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.User>> UserGetUser(
            string criteriaUserId = default(string),
            string criteriaSearchText = default(string),
            string criteriaCustomerId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H&#228;mtar lista &#246;ver alla anv&#228;ndare. H&#228;mta samtliga anv&#228;ndare.
        /// F&#246;r att kunna h&#228;mta anv&#228;ndarlista s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="criteriaUserId">Användarid</param>
        /// <param name="criteriaSearchText">Text som filtrerar på namn eller titel</param>
        /// <param name="criteriaCustomerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<User.User>> UserGetAllUsers(
            string criteriaUserId = default(string),
            string criteriaSearchText = default(string),
            string criteriaCustomerId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Autentisierar anv&#228;ndare
        /// </summary>
        /// <param name="criteriaCustomerId">Kundid</param>
        /// <param name="criteriaUserName">Användarnamn</param>
        /// <param name="criteriaPassword">Lösenord</param>
        /// <param name="cancellationToken"></param>
        Task<bool> UserAuthenticateUser(
            string criteriaCustomerId = default(string),
            string criteriaUserName = default(string),
            string criteriaPassword = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}