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
        /// H�mtar lista �ver publika anv�ndare. H�mta anv�ndarlista f�r publika anv�ndare.
        /// F�r att kunna h�mta anv�ndarlista s� kr�vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="criteriaUserId">Anv�ndarid</param>
        /// <param name="criteriaSearchText">Text som filtrerar p� namn eller titel</param>
        /// <param name="criteriaCustomerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.User>> UserGetUser(
            string criteriaUserId = default(string),
            string criteriaSearchText = default(string),
            string criteriaCustomerId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// H�mtar lista �ver alla anv�ndare. H�mta samtliga anv�ndare.
        /// F�r att kunna h�mta anv�ndarlista s� kr�vs det en giltig API nyckel och ett kundid.
        /// </summary>
        /// <param name="criteriaUserId">Anv�ndarid</param>
        /// <param name="criteriaSearchText">Text som filtrerar p� namn eller titel</param>
        /// <param name="criteriaCustomerId">Kundid</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<User.User>> UserGetAllUsers(
            string criteriaUserId = default(string),
            string criteriaSearchText = default(string),
            string criteriaCustomerId = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Autentisierar anv�ndare
        /// </summary>
        /// <param name="criteriaCustomerId">Kundid</param>
        /// <param name="criteriaUserName">Anv�ndarnamn</param>
        /// <param name="criteriaPassword">L�senord</param>
        /// <param name="cancellationToken"></param>
        Task<bool> UserAuthenticateUser(
            string criteriaCustomerId = default(string),
            string criteriaUserName = default(string),
            string criteriaPassword = default(string),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}