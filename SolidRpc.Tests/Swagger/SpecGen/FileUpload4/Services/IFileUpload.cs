using SolidRpc.Tests.Swagger.SpecGen.FileUpload4.Types;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGen.FileUpload4.Services
{
    /// <summary>
    /// Test interface to upload a file
    /// </summary>
    public interface IFileUpload
    {
        /// <summary>
        /// Uploads a file. This method will get a "file" parameter. 
        /// </summary>
        /// <param name="additionalData">The additional data</param>
        /// <param name="fileData">The file data</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileData> UploadFile(ComplexType additionalData, FileData fileData, CancellationToken cancellationToken = default(CancellationToken));
    }
}
