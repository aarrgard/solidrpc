using System.IO;

namespace SolidRpc.OpenApi.SwaggerUI.Types
{
    /// <summary>
    /// Represents some file content
    /// </summary>
    public class FileContent
    {
        /// <summary>
        /// The file body.
        /// </summary>
        public Stream Body { get; set; }

        /// <summary>
        /// The content type.
        /// </summary>
        public string ContentType { get; set; }

    }
}
