using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Models = SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.File.Models;
namespace SolidRpc.Tests.Swagger.CodeGen.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface ICrmFile {
        /// <summary>
        /// H&#228;mtar fil. Exempelvis bilagor i pdf. &lt;p&gt;H&#228;mta fil.&lt;/p&gt;
        /// &lt;p&gt;F&#246;r att kunna h&#228;mta en fil s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// Det kr&#228;vs &#228;ven ett giltig fil id f&#246;r att kunna h&#228;mta en fil.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="fileId">Filid</param>
        /// <param name="cancellationToken"></param>
        Task<Stream> CrmFileGetFile(
            string customerId,
            string fileId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att l&#228;gga till ett nytt dokument till en bostad.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="fileData">Fildata</param>
        /// <param name="cancellationToken"></param>
        Task<string> CrmFilePost(
            string customerId,
            string estateId,
            Models.File fileData,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f&#246;r att ta bort en fil
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="fileId">Filid</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="SolidRpc.Tests.Swagger.CodeGen.Vitec.Types.Services.CrmFile.CrmFileDelete.NoContentException">No Content</exception>
        Task CrmFileDelete(
            string customerId,
            string fileId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}