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
    public interface IFile {
        /// <summary>
        /// H�mtar fil. Exempelvis bilagor i pdf. &lt;p&gt;H�mtar fil.&lt;/p&gt;
        /// &lt;p&gt;F�r att kunna h�mta en fil s� kr�vs det en giltig API nyckel och ett kundid.
        /// Det kr�vs �ven ett giltig fil id f�r att kunna h�mta en fil.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="fileId">Filid</param>
        /// <param name="cancellationToken"></param>
        Task<Stream> FileGetFile(
            string customerId,
            string fileId,
            CancellationToken cancellationToken = default(CancellationToken));
    
        /// <summary>
        /// Metod f�r att l�gga till ett nytt dokument till en bostad.
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="estateId">Bostadsid</param>
        /// <param name="fileData">Fildata</param>
        /// <param name="cancellationToken"></param>
        Task<string> FilePost(
            string customerId,
            string estateId,
            Models.File fileData,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}