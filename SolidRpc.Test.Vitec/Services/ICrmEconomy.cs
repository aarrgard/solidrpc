using System.CodeDom.Compiler;
using System.Threading.Tasks;
using SolidRpc.Test.Vitec.Types.Economy.Models;
using System.Threading;
using SolidRpc.Test.Vitec.Types.Common.Cms;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmEconomy {
        /// <summary>
        /// H&#228;mtar handpenningsuppgifter. &lt;p&gt;H&#228;mtar uppgifter om handpenning avseende en bostad.&lt;/p&gt;
        /// &lt;p&gt;F&#246;r att kunna h&#228;mta uppgifter s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">bostadsid</param>
        /// <param name="cancellationToken"></param>
        Task<Deposit> CrmEconomyGetDeposit(
            string customerId,
            string estateId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdaterar handpenningsupppgifter. &lt;p&gt;Uppdaterar handpenningsupppgifter.&lt;/p&gt;
        /// &lt;p&gt;F&#246;r att kunna h&#228;mta uppgifter s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">bostadsid</param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        Task<string> CrmEconomyPutDeposit(
            string customerId,
            string estateId,
            DepositData data,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Uppdatera driftkostnader p&#229; ett objekt
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">bostadsid</param>
        /// <param name="operation">driftskostnader</param>
        /// <param name="cancellationToken"></param>
        Task<string> CrmEconomyUpdateOperatingCosts(
            string customerId,
            string estateId,
            Operation operation,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att skapa ett befintligt l&#229;n
        /// </summary>
        /// <param name="customerId">Kund-id</param>
        /// <param name="estateId">bostadsid</param>
        /// <param name="loan">Information om det befintliga lånet</param>
        /// <param name="cancellationToken"></param>
        Task<string> CrmEconomyCreateLoan(
            string customerId,
            string estateId,
            Loan loan,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}