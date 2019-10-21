using System.CodeDom.Compiler;
using System.Threading.Tasks;
using Models = SolidRpc.Test.Vitec.Types.Link.Models;
using System.Threading;
using System.Collections.Generic;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmLink {
        /// <summary>
        /// H&#228;mtar l&#228;nkar. &lt;p&gt;H&#228;mtar l&#228;nk.&lt;/p&gt;
        /// &lt;p&gt;F&#246;r att kunna h&#228;mta en l&#228;nk s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// Det kr&#228;vs &#228;ven ett giltig l&#228;nkid f&#246;r att kunna h&#228;mta en l&#228;nk.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="linkId">länkid</param>
        /// <param name="cancellationToken"></param>
        Task<Models.Link> CrmLinkGet(
            string customerId,
            string linkId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att l&#228;gga till en ny l&#228;nk  till en bostad.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="linkData">Länk</param>
        /// <param name="cancellationToken"></param>
        Task<string> CrmLinkCreate(
            string customerId,
            string estateId,
            Models.LinkData linkData,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att h&#228;mta l&#228;nkkategorier
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<Models.LinkCategories>> CrmLinkCategories(
            string customerId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}