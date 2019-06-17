using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SolidRpc.Tests.Swagger.SpecGen.FileUpload1.Services
{
    /// <summary>
    /// Test interface to upload a file
    /// </summary>
    public interface IFileUpload
    {
        /// <summary>
        /// Uploads a file. This method will get a "file" parameter. 
        /// </summary>
        /// <param name="fileStream">The file stream</param>
        /// <param name="fileName">The file name</param>
        /// <param name="contentType">The content type</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UploadFile(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default(CancellationToken));
        Task UploadFile(object p1, object p2, object p3, object none);
    }
}
