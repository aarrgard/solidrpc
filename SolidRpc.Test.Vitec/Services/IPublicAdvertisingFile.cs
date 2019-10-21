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
        /// H&#228;mtar filer. &lt;p&gt;H&#228;mtar fil.&lt;/p&gt;
        /// &lt;p&gt;F&#246;r att kunna h&#228;mta en fil s&#229; kr&#228;vs det en giltig API nyckel och ett kundid.
        /// Det kr&#228;vs &#228;ven ett giltigt fil id f&#246;r att kunna h&#228;mta en fil.&lt;/p&gt;
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