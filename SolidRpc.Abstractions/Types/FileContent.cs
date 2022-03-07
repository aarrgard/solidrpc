using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SolidRpc.Abstractions.Types
{
    /// <summary>
    /// Represents a file content
    /// </summary>
    public class FileContent
    {
        /// <summary>
        /// The file content.
        /// </summary>
        public Stream Content { get; set; }

        /// <summary>
        /// The content charset
        /// </summary>
        public string CharSet { get; set; }

        /// <summary>
        /// The content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The last modified date of the resource.
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }

        /// <summary>
        /// The location of the content
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The ETag.
        /// </summary>
        public string ETag { get; set; }

        /// <summary>
        /// The SetCookie.
        /// </summary>
        public string SetCookie { get; set; }
    }

    /// <summary>
    /// Provides extension methods
    /// </summary>
    public static class FileContentExtensions
    {
        /// <summary>
        /// Returns the content as stream
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public static async Task<string> AsStringAsync(this FileContent fileContent)
        {
            StreamReader r;
            if(!string.IsNullOrEmpty(fileContent.CharSet))
            {
                r = new StreamReader(fileContent.Content, Encoding.GetEncoding(fileContent.CharSet));
            }
            else
            {
                r = new StreamReader(fileContent.Content);
            }
            using(r)
            {
                return await r.ReadToEndAsync();
            }
        }
    }
}
