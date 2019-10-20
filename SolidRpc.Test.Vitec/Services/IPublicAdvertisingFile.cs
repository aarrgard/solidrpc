using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
namespace SolidRpc.Test.Vitec.Services {
    /// <summary>
    /// 
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IPublicAdvertisingFile {
        /// <summary>
        /// H�mtar filer. &lt;p&gt;H�mtar fil.&lt;/p&gt;
        /// &lt;p&gt;F�r att kunna h�mta en fil s� kr�vs det en giltig API nyckel och ett kundid.
        /// Det kr�vs �ven ett giltigt fil id f�r att kunna h�mta en fil.&lt;/p&gt;
        /// </summary>
        /// <param name="customerId">Kundid</param>
        /// <param name="fileId">Filid</param>
        /// <param name="cancellationToken"></param>
        Task<Stream> PublicAdvertisingFileGet(
            string customerId,
            string fileId,
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}