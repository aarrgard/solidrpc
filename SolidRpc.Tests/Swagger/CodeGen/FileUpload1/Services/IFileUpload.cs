using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
namespace SolidRpc.Tests.Swagger.CodeGen.FileUpload1.Services {
    /// <summary>
    /// Test interface to upload a file
    /// </summary>
    [GeneratedCode("OpenApiCodeGeneratorV2","1.0.0.0")]
    public interface IFileUpload {
        /// <summary>
        /// Uploads a file. This method will get a &quot;file&quot; parameter.
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="cancellationToken"></param>
        Task UploadFile(
            Stream fileStream = default(Stream),
            CancellationToken cancellationToken = default(CancellationToken));
    
    }
}