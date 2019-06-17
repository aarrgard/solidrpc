using SolidRpc.Tests.Swagger.SpecGen.FileUpload2.Types;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGen.FileUpload2.Services
{
    /// <summary>
    /// Test interface to upload a file
    /// </summary>
    public interface IFileUpload
    {
        /// <summary>
        /// Uploads a file. This method will get a "file" parameter. 
        /// </summary>
        /// <param name="fileData">The file data</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UploadFile(FileData fileData, CancellationToken cancellationToken = default(CancellationToken));
    }
}
