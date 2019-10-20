using System.CodeDom.Compiler;
using System.Threading.Tasks;
using Models = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Link.Models;
using System.Threading;
using System.Collections.Generic;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmLink {
        /// <summary>
        /// H�mtar l�nkar. &lt;p&gt;H�mtar l�nk.&lt;/p&gt;
        /// &lt;p&gt;F�r att kunna h�mta en l�nk s� kr�vs det en giltig API nyckel och ett kundid.
        /// Det kr�vs �ven ett giltig l�nkid f�r att kunna h�mta en l�nk.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="linkId">l�nkid</param>
        /// <param name="cancellationToken"></param>
        Task<Models.Link> CrmLinkGet(
            string customerId,
            string linkId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att l�gga till en ny l�nk  till en bostad.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="linkData">L�nk</param>
        /// <param name="cancellationToken"></param>
        Task<string> CrmLinkCreate(
            string customerId,
            string estateId,
            Models.LinkData linkData,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att h�mta l�nkkategorier
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.LinkCategories>> CrmLinkCategories(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}