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
        Task<FileData> UploadFile(FileData fileData, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the file data as null
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FileData> NullData(CancellationToken cancellationToken = default(CancellationToken));
    }
}
