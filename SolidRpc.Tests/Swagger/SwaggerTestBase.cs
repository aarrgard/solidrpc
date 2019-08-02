using System.IO;
using System.Linq;

namespace SolidRpc.Tests.Swagger
{
    /// <summary>
    /// Base class for the swagger tests
    /// </summary>
    public abstract class SwaggerTestBase : WebHostMvcTest
    {
        /// <summary>
        /// Reads the api config
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected string ReadOpenApiConfiguration(string folderName)
        {
            var folder = GetSpecFolder(folderName);
            var jsonFile = folder.GetFiles("*.json").Single();
            using (var sr = jsonFile.OpenText())
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Returns the spec folder.
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected abstract DirectoryInfo GetSpecFolder(string folderName);
    }
}
